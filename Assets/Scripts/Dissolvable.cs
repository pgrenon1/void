using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dissolvable : MonoBehaviour
{
    public float dissolutionRate = 0.2f;
    public float randomAmplitude = 0.03f;
    public AnimationCurve dissolutionRateOverDistance;
    public float maxEffectDistanceFromPlayer = 10f;

    public bool IsDissolvedCompletly { get; set; }

    private List<MeshRenderer> _meshRenderers = new List<MeshRenderer>();
    private float _indirectDiffuseRatio;
    private float _grayscaleRatio;
    private float _rimRatio;
    private float _blackRatio;
    private GUIStyle _guiStyle;
    private float _maxEffectDistanceFromPlayerSqr;
    private List<MaterialPropertyBlock> _materialPropertyBlocks = new List<MaterialPropertyBlock>();
    private float _dissolutionRate;
    public float _greatestDissolveRate;

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
        _grayscaleRatio = 0f;
    }

    private void Update()
    {
        UpdateDissolve();

        ApplyRatios();
    }

    private void ApplyRatios()
    {
        for (int i = 0; i < _meshRenderers.Count; i++)
        {
            var meshRenderer = _meshRenderers[i];
            var propBlock = _materialPropertyBlocks[i];

            meshRenderer.GetPropertyBlock(propBlock);

            if (_indirectDiffuseRatio > 0f && _indirectDiffuseRatio < 1f)
                propBlock.SetFloat("_IndirectDiffuseContribution", _indirectDiffuseRatio);

            if (_grayscaleRatio > 0f && _grayscaleRatio < 1f)
                propBlock.SetFloat("_GrayscaleValue", _grayscaleRatio);

            if (_blackRatio > 0f && _blackRatio < 1f)
                propBlock.SetFloat("_BlackValue", _blackRatio);

            if (_rimRatio > 0f && _rimRatio < 1f)
                propBlock.SetFloat("_RimValue", _rimRatio);

            meshRenderer.SetPropertyBlock(propBlock);
        }
    }

    private void UpdateDissolve()
    {
        if (_indirectDiffuseRatio <= 1f)
            _indirectDiffuseRatio += _greatestDissolveRate;
        else if (_grayscaleRatio <= 1f)
            _grayscaleRatio += _greatestDissolveRate;
        else if (_blackRatio <= 1f)
        {
            _blackRatio += _greatestDissolveRate;
            _rimRatio += _greatestDissolveRate;
        }
        else
            IsDissolvedCompletly = true;
    }

    public void Dissolve(float distanceSqr)
    {
        var distanceRatio = distanceSqr / _maxEffectDistanceFromPlayerSqr;

        if (distanceRatio < 1f)
        {
            var ratioDelta = _dissolutionRate * dissolutionRateOverDistance.Evaluate(Mathf.Abs(1 - distanceRatio)) * Time.deltaTime;

            if (ratioDelta > _greatestDissolveRate)
                _greatestDissolveRate = ratioDelta;
        }
    }
}
