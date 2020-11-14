using UnityEngine;

public class DissolvingTree
{
    public TreeInstance tree;
    public float value;
    public Color32 startingColor;

    public DissolvingTree(TreeInstance tree, float value)
    {
        this.tree = tree;
        this.value = value;
        startingColor = tree.color;
    }
}