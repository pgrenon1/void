using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

public class TerrainManager : MonoBehaviour
{
    public float dissolveRate;
    public float maxDistanceToDissolve;

    private TreeInstance[] _originalTreeInstances;
    private RuntimeTreeReplacer _treeReplacer;
    private Material _material;
    private Texture2D _dissolveTexture;
    private float _maxDissolveDistanceSqr;
    private NativeArray<float> _blackValues;

    private Player _player;
    public Player Player
    {
        get
        {
            if (_player == null)
                _player = GameManager.Instance.Player;

            return _player;
        }
    }

    private void Awake()
    {
        _maxDissolveDistanceSqr = maxDistanceToDissolve * maxDistanceToDissolve;

        _material = Terrain.activeTerrain.materialTemplate;
        var mainTex = _material.GetTexture("_MainTex");
        _dissolveTexture = new Texture2D(mainTex.width, mainTex.height);
        Color[] colors = new Color[mainTex.width * mainTex.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        _dissolveTexture.SetPixels(colors);

        var colorsLength = _dissolveTexture.GetPixels().Length;
        _blackValues = new NativeArray<float>(colorsLength, Allocator.Persistent);

        SetupTrees();
    }

    private void SetupTrees()
    {
        SaveOriginalTerrainTrees();

        _treeReplacer = new RuntimeTreeReplacer();
        _treeReplacer.Convert(Terrain.activeTerrain);

        ClearTerrainTrees();
    }

    private void Update()
    {
        var terrain = Terrain.activeTerrain;
        var terrainData = terrain.terrainData;

        var playerPos = Player.transform.position - terrain.transform.position;
        var xPosNormalized = Mathf.InverseLerp(0f, terrainData.size.x, playerPos.x);
        var yPosNormalized = Mathf.InverseLerp(0f, terrainData.size.z, playerPos.z);

        var xPos = Mathf.Lerp(0f, _dissolveTexture.width, xPosNormalized);
        var yPos = Mathf.Lerp(0f, _dissolveTexture.height, yPosNormalized);

        var xInt = (System.Int32)xPos;
        var yInt = (System.Int32)yPos;

        var playerPosInTexture = new Vector2Int(xInt, yInt);

        var colors = _dissolveTexture.GetPixels();
        var colorsNative = new NativeArray<Color>(colors, Allocator.TempJob);

        var job = new UpdateColorJob
        {
            BlackValues = _blackValues,
            Colors = colorsNative,
            PlayerPosition = playerPosInTexture,
            TextureWidth = _dissolveTexture.width,
            TargetColor = Color.black,
            MaxDissolveDistanceSqr = _maxDissolveDistanceSqr,
            DeltaTime = Time.deltaTime,
            DissolutionRate = dissolveRate
        };
        var jobHandle = job.Schedule(colorsNative.Length, 1);
        jobHandle.Complete();

        colorsNative.CopyTo(colors);

        _dissolveTexture.SetPixels(colors);

        colorsNative.Dispose();

        _dissolveTexture.Apply(false);
        _material.SetTexture("_DissolveTexture", _dissolveTexture);
    }

    private void OnDisable()
    {
        _blackValues.Dispose();
    }

    private void OnApplicationQuit()
    {
        _treeReplacer.Clear();

        ApplyOriginalTerrainTrees();
    }

    private void ClearTerrainTrees()
    {
        Terrain.activeTerrain.terrainData.SetTreeInstances(new TreeInstance[0], false);
    }

    private void SaveOriginalTerrainTrees()
    {
        _originalTreeInstances = Terrain.activeTerrain.terrainData.treeInstances;
    }

    private void ApplyOriginalTerrainTrees()
    {
        Terrain.activeTerrain.terrainData.treeInstances = _originalTreeInstances;
    }
}

[BurstCompile(CompileSynchronously = true)]
public struct UpdateColorJob : IJobParallelFor
{
    public NativeArray<Color> Colors;
    public NativeArray<float> BlackValues;
    public Vector2Int PlayerPosition;
    public int TextureWidth;
    public Color TargetColor;
    public float MaxDissolveDistanceSqr;
    public float DissolutionRate;
    public float DeltaTime;
    public float DissolveValue;

    public void Execute(int index)
    {
        if (BlackValues[index] > 1f)
            return;

        var x = index % TextureWidth;
        var y = index / TextureWidth;
        var pixelPosition = new Vector2(x, y);

        var delta = PlayerPosition - pixelPosition;
        var distanceRatio = delta.sqrMagnitude / MaxDissolveDistanceSqr;

        if (distanceRatio < 1f || BlackValues[index] > 0f)
        {
            BlackValues[index] += DissolutionRate /** (1 - distanceRatio)*/ * DeltaTime;
            Colors[index] = Color.Lerp(Colors[index], TargetColor, BlackValues[index]);
        }
    }
}

//public struct PixelData
//{
//    public Vector2Int PlayerPosition;
//    public NativeArray<Color32> Colors;
//    public int TextureWidth;
//    public Color32 TargetColor;
//    public int Index;
//    public float MaxDissolveDistanceSqr;
//    public float DissolveRate;
//    public float DeltaTime;
//    public float DissolveValue;
//}