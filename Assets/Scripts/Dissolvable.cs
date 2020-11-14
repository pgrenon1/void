using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Param
{

}

public class Dissolvable : MonoBehaviour
{
    public float dissolutionRate = 0.2f;
    public float randomAmplitude = 0.03f;
    public AnimationCurve dissolutionRateOverDistance;
    public float maxEffectDistanceFromPlayer = 10f;

    public bool IsDissolvedCompletly { get; set; }

    private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();
    private float _dissolveRatio;
    private float _blackRatio;
    private GUIStyle _guiStyle;
    private float _maxEffectDistanceFromPlayerSqr;
    private List<MaterialPropertyBlock> _materialPropertyBlocks = new List<MaterialPropertyBlock>();
    private float _dissolutionRate;

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
        _dissolutionRate = dissolutionRate + Random.Range(-randomAmplitude, randomAmplitude);

        _guiStyle = new GUIStyle();
        _guiStyle.fontSize = 20;
        _maxEffectDistanceFromPlayerSqr = maxEffectDistanceFromPlayer * maxEffectDistanceFromPlayer;
        GetComponentsInChildren<MeshRenderer>(_meshRenderers);

        foreach (var meshRenderer in _meshRenderers)
        {
            _materialPropertyBlocks.Add(new MaterialPropertyBlock());
        }

        ResetRatios();
    }

    private void ResetRatios()
    {
        _dissolveRatio = 0f;
    }

    private void Update()
    {
        ApplyRatios();
    }

    private void ApplyRatios()
    {

        for (int i = 0; i < _meshRenderers.Count; i++)
        {
            var meshRenderer = _meshRenderers[i];
            var propBlock = _materialPropertyBlocks[i];

            meshRenderer.GetPropertyBlock(propBlock);

            propBlock.SetFloat("_GrayscaleValue", _dissolveRatio);

            meshRenderer.SetPropertyBlock(propBlock);
        }
    }

    public void Dissolve(float distanceSqr)
    {
        var distanceRatio = distanceSqr / _maxEffectDistanceFromPlayerSqr;

        if (distanceRatio < 1f)
        {
            var ratioDelta = _dissolutionRate * dissolutionRateOverDistance.Evaluate(Mathf.Abs(1 - distanceRatio)) * Time.deltaTime;

            if (_dissolveRatio <= 1f)
                _dissolveRatio += ratioDelta;
            else if (_blackRatio <= 1f)
                _blackRatio += ratioDelta;
            else
                IsDissolvedCompletly = true;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying)
    //        return;

    //    if ((_grayscaleRatio < 1f && _grayscaleRatio > 0f) || (_blackRatio < 1f && _blackRatio > 0f))
    //        Handles.Label(transform.position + Vector3.up * 2f, String.Format("{0}, {1}", _grayscaleRatio.ToString("#.##"), _blackRatio.ToString("#.##")), _guiStyle);
    //}
}
