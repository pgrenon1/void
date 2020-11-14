using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public float maxEffectSqrDistanceFromPlayer = 100f;

    private Dictionary<TreeInstance, float> _trees = new Dictionary<TreeInstance, float>();

    void Start()
    {
        var terrain = GetComponent<Terrain>();
        var trees = terrain.terrainData.treeInstances;

        foreach (var tree in trees)
        {
            _trees.Add(tree, 1f);
        }
    }

    void Update()
    {
        foreach (KeyValuePair<TreeInstance, float> entry in _trees)
        {
            TreeInstance tree = entry.Key;
            float value = entry.Value;

            var delta = tree.position - GameManager.Instance.Player.transform.position;
            if (delta.sqrMagnitude < maxEffectSqrDistanceFromPlayer)
            {
                var disappearanceRatio = Mathf.Lerp(0f, 1f, delta.sqrMagnitude);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
