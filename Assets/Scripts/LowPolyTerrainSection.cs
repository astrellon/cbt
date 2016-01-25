using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrainSection : MonoBehaviour 
{
    public LowPolyTerrainData TerrainData;

    public int StartX = 0;
    public int StartY = 0;
    public int Width = 50;
    public int Height = 50;

    public MaterialPair[] Materials;

    public Dictionary<string, Material> materialMap = new Dictionary<string, Material>();

	// Use this for initialization
	void Start () 
    {
	}

    public void Init(LowPolyTerrainData terrainData)
    {
        TerrainData = terrainData;

        foreach (var pair in Materials)
        {
            materialMap[pair.Name] = pair.Material;
        }
        
        CreateMesh();	
    }

    void CreateMesh()
    {
        if (TerrainData.Width == 0 || TerrainData.Height == 0)
        {
            Debug.Log("Cannot generate terrain data, too small");
            return;
        }

        foreach (var tileType in TerrainData.TileTypes)
        {
            var tileMesh = new GameObject();
            tileMesh.name = "mesh_" + tileType;
            tileMesh.transform.parent = transform;
            var tileRender = tileMesh.AddComponent<LowPolyTerrainTileRender>() as LowPolyTerrainTileRender;
            tileRender.CreateMesh(TerrainData, tileType, materialMap[tileType]);
        }
    }
}
