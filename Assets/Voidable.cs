using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Voidable : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    public float _blackWhiteRatio;
    public float _blackRatio;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        ResetRatios();
    }

    private void ResetRatios()
    {
        _blackWhiteRatio = 0f;
        _blackRatio = 0f;
    }

    private void Update()
    {
        ApplyRatios();
    }

    private void ApplyRatios()
    {

    }

    public void Vacuum(float ratioDelta)
    {
        if (_blackWhiteRatio <= 1f)
            _blackWhiteRatio -= ratioDelta;
        else if (_blackRatio <= 1f)
            _blackRatio -= ratioDelta;
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position + Vector3.up * 2f, String.Format("{0}, {1}", _blackWhiteRatio, _blackRatio));
    }
}
