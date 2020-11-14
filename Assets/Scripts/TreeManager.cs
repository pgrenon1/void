using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    //public float dissolutionRate = 0.1f;
    //public AnimationCurve dissolutionRateOverDistanceSqr;
    //public float maxEffectDistanceFromPlayer = 10f;

    //private List<DissolvingTree> _dissolvingTrees = new List<DissolvingTree>();
    //private Color32 _dissolvedColor;

    //private float MaxEffectDistanceFromPlayerSqr
    //{
    //    get
    //    {
    //        return maxEffectDistanceFromPlayer * maxEffectDistanceFromPlayer;
    //    }
    //}

    //public Terrain Terrain { get; private set; }

    //void Start()
    //{
    //    Terrain = GetComponent<Terrain>();
    //    var trees = Terrain.terrainData.treeInstances;

    //    foreach (var tree in trees)
    //    {
    //        _dissolvingTrees.Add(new DissolvingTree(tree, 1f));
    //    }

    //    Debug.Log(_dissolvingTrees.Count);
    //}

    //void Update()
    //{
    //    var playerPosition = GameManager.Instance.Player.PlayerMovementController.transform.position;

    //    foreach (var dissolvingTree in _dissolvingTrees)
    //    {
    //        var treePosition = dissolvingTree.tree.position;
    //        treePosition.Scale(Terrain.terrainData.size);
    //        var delta = treePosition - playerPosition;
    //        var modifiedDissolutionRate = dissolutionRateOverDistanceSqr.Evaluate(delta.magnitude / maxEffectDistanceFromPlayer);
    //        if (delta.sqrMagnitude < MaxEffectDistanceFromPlayerSqr && dissolvingTree.value > 0f)
    //        {
    //            dissolvingTree.value -= modifiedDissolutionRate * Time.deltaTime * modifiedDissolutionRate;
    //            dissolvingTree.tree.color = Color32.Lerp(dissolvingTree.startingColor, Color.black, dissolvingTree.value);
    //        }
    //    }
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    if (!Application.isPlaying)
    //        return;

    //    var guiStyle = new GUIStyle();
    //    guiStyle.fontSize = 20;

    //    var playerPosition = GameManager.Instance.Player.PlayerMovementController.transform.position;

    //    foreach (var dissolvingTree in _dissolvingTrees)
    //    {
    //        var treePosition = dissolvingTree.tree.position;
    //        treePosition.Scale(Terrain.terrainData.size);
    //        var delta = treePosition - playerPosition;
    //        if (delta.sqrMagnitude < MaxEffectDistanceFromPlayerSqr)
    //        {
    //            Handles.Label(treePosition + Vector3.up, dissolvingTree.value.ToString(), guiStyle);
    //        }
    //    }
    //}
}
