using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private TreeInstance[] _originalTreeInstances;
    private RuntimeTreeReplacer _treeReplacer;
    private Material _material;
    private Texture2D _dissolveTexture;
    private float _maxDissolveDistanceSqr;

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

    private void Start()
    {
        _maxDissolveDistanceSqr = Player.maxDistanceToDissolve * Player.maxDistanceToDissolve;

        _material = Terrain.activeTerrain.materialTemplate;
        var mainTex = _material.GetTexture("_MainTex");
        _dissolveTexture = new Texture2D(mainTex.width, mainTex.height);
        Color[] colors = new Color[mainTex.width * mainTex.height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        _dissolveTexture.SetPixels(colors);

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
        //var terrain = Terrain.activeTerrain;
        //var terrainData = terrain.terrainData;

        //var playerPos = Player.transform.position - terrain.transform.position;
        //var xPosNormalized = Mathf.InverseLerp(0f, terrainData.size.x, playerPos.x);
        //var yPosNormalized = Mathf.InverseLerp(0f, terrainData.size.z, playerPos.z);

        ////var texture2D = _material.GetTexture("_DissolveTexture") as Texture2D;

        //var xPos = Mathf.Lerp(0f, _dissolveTexture.width, xPosNormalized);
        //var yPos = Mathf.Lerp(0f, _dissolveTexture.height, yPosNormalized);

        //var xInt = (System.Int32)xPos;
        //var yInt = (System.Int32)yPos;

        //var playerPosInTexture = new Vector2Int(xInt, yInt);

        //for (int x = 0; x < _dissolveTexture.width; x++)
        //{
        //    for (int y = 0; y < _dissolveTexture.height; y++)
        //    {
        //        var pixelPos = new Vector2Int(x, y);
        //        var delta = playerPosInTexture - pixelPos;
        //        var distanceRatio = delta.sqrMagnitude / _maxDissolveDistanceSqr;
        //        if (distanceRatio < 1f)
        //        {
        //            var oldColor = _dissolveTexture.GetPixel(x, y);
        //            var newColor = Color.Lerp(Color.black, oldColor, distanceRatio);
        //        }
        //    }
        //}

        ////float rSquared = dissolveRadius * dissolveRadius;
        ////for (int u = xInt - dissolveRadius; u < xInt + dissolveRadius + 1; u++)
        ////{
        ////    for (int v = yInt - dissolveRadius; v < yInt + dissolveRadius + 1; v++)
        ////    {
        ////        if ((xInt - u) * (xInt - u) + (yInt - v) * (yInt - v) < rSquared)
        ////            _dissolveTexture.SetPixel(u, v, Color.black);
        ////    }
        ////}

        ////texture2D.SetPixel(xInt, yInt, Color.black);

        //_dissolveTexture.Apply(false);
        //_material.SetTexture("_DissolveTexture", _dissolveTexture);
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
