using UnityEngine;

public class RuntimeTreeReplacer
    {
        public void Convert(Terrain terrain)
        {
            TerrainData data = terrain.terrainData;
            float width = data.size.x;
            float height = data.size.z;
            float y = data.size.y;
            // Create parent
            GameObject parent = GameObject.Find("TREES_GENERATED");
            if (parent == null)
            {
                parent = new GameObject("TREES_GENERATED");
            }
            // Create trees
            foreach (TreeInstance tree in data.treeInstances)
            {
                if (tree.prototypeIndex >= data.treePrototypes.Length)
                    continue;
                var _tree = data.treePrototypes[tree.prototypeIndex].prefab;
                Vector3 position = new Vector3(
                    tree.position.x * width,
                    tree.position.y * y,
                    tree.position.z * height) + terrain.transform.position;
                Vector3 scale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                GameObject go = GameObject.Instantiate(_tree, position, Quaternion.Euler(0f, Mathf.Rad2Deg * tree.rotation, 0f), parent.transform) as GameObject;
                go.transform.localScale = scale;
            }
        }

        public void Clear()
        {
            GameObject.DestroyImmediate(GameObject.Find("TREES_GENERATED"));
        }
    }