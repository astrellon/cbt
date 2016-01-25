using UnityEngine;
using System.Collections;

public class LowPolyTreeRender : MonoBehaviour 
{
    public LowPolyTerrainData TerrainData;
    public GameObject TreePrefab;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Init(LowPolyTerrainData terrainData)
    {
        TerrainData = terrainData;
        CreateTrees();
    }

    void CreateTrees()
    {
        for (var y = 0; y < TerrainData.Height; y++)
        {
            for (var x = 0; x < TerrainData.Width; x++)
            {
                var tile = TerrainData.GetTile(x, y);

                if (tile.HasTree)
                {
                    AddTree(tile);
                }
            }
        }
    }
    
    private void AddTree(LowPolyTerrainTile tile)
    {
        if (TreePrefab == null)
        {
            return;
        }

        var center = (tile.Corner1 + tile.Corner2 + tile.Corner3) / 3.0f;

        var newTree = Instantiate(TreePrefab, center, Quaternion.identity) as GameObject;
        newTree.transform.localScale = Vector3.one * tile.TreeScale;
        newTree.transform.localEulerAngles = new Vector3(0, tile.TreeRotation, 0);
        newTree.transform.parent = transform;
    }
}
