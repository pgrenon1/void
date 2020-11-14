using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dissolvable : MonoBehaviour
{
    public float dissolutionRate = 0.3f;
    public AnimationCurve dissolutionRateOverDistance;
    public float maxEffectDistanceFromPlayer = 10f;

    private MeshRenderer _meshRenderer;
    private float _blackWhiteRatio;
    private float _blackRatio;
    private GUIStyle _guiStyle;
    private float _maxEffectDistanceFromPlayerSqr;

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
        _guiStyle = new GUIStyle();
        _guiStyle.fontSize = 20;
        _maxEffectDistanceFromPlayerSqr = maxEffectDistanceFromPlayer * maxEffectDistanceFromPlayer;
        _meshRenderer = GetComponent<MeshRenderer>();
        ResetRatios();
    }

    private void ResetRatios()
    {
        _blackWhiteRatio = 1f;
        _blackRatio = 1f;
    }

    private void Update()
    {
        UpdateDissolve();

        ApplyRatios();
    }

    private void UpdateDissolve()
    {
        var deltaToPlayer = Player.transform.position - transform.position;
        var distanceSqr = deltaToPlayer.sqrMagnitude;
        var distanceRatioSqr = distanceSqr / _maxEffectDistanceFromPlayerSqr;
        if (distanceRatioSqr < 1f)
        {
            var ratioDelta = dissolutionRate * dissolutionRateOverDistance.Evaluate(Mathf.Abs(1 - distanceRatioSqr)) * Time.deltaTime;
            Dissolve(-ratioDelta);
        }
    }

    private void ApplyRatios()
    {

    }

    public void Dissolve(float ratioDelta)
    {
        if (_blackWhiteRatio >= 0f)
            _blackWhiteRatio += ratioDelta;
        else if (_blackRatio >= 0f)
            _blackRatio += ratioDelta;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if ((_blackWhiteRatio < 1f && _blackWhiteRatio > 0f) || (_blackRatio < 1f && _blackRatio > 0f))
            Handles.Label(transform.position + Vector3.up * 2f, String.Format("{0}, {1}", _blackWhiteRatio.ToString("#.##"), _blackRatio.ToString("#.##")), _guiStyle);
    }
}
