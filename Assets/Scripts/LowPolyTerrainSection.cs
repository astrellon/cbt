using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrainSection : MonoBehaviour 
{
    public LowPolyTerrainData TerrainData;

    public int Width = 50;
    public int Height = 50;
    public float Depth = 32.0f;
    public float OffsetX = 0;
    public float OffsetY = 0;

    public MaterialPair[] Materials;

    public Dictionary<string, Material> materialMap = new Dictionary<string, Material>();

	// Use this for initialization
	void Start () 
    {
        foreach (var pair in Materials)
        {
            materialMap[pair.Name] = pair.Material;
        }

        TerrainData = new LowPolyTerrainData(Width, Height);

        for (var y = 0; y < TerrainData.Height; y++)
        {
            for (var x = 0; x < TerrainData.Width; x++)
            {
                var tile = TerrainData.GetTile(x, y);

                var height1 = CalcHeight(tile.Corner1);
                var height2 = CalcHeight(tile.Corner2);
                var height3 = CalcHeight(tile.Corner3);

                tile.SetCorner(height1, 0);
                tile.SetCorner(height2, 1);
                tile.SetCorner(height3, 2);
                
                var anyBelow = height1 < 0 || height2 < 0 || height3 < 0;

                TerrainData.SetTileType(x, y, anyBelow ? "sand" : "grass");
            } 
        }

        CreateMesh();	

        transform.position = new Vector3(OffsetX * LowPolyTerrainTile.TriHalfWidth, 0, OffsetY * LowPolyTerrainTile.TriHeight);
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

    float CalcHeight(Vector3 position)
    {
        var xpos = OffsetX * LowPolyTerrainTile.TriHalfWidth + position.x;
        var ypos = OffsetY * LowPolyTerrainTile.TriHeight + position.z;
        var subheight = Mathf.PerlinNoise(xpos / 512.0f, ypos / 512.0f) * Depth * 5.0f - Depth * 2.5f;
        var height = Mathf.PerlinNoise(xpos / 128.0f, ypos / 128.0f) * Depth; 
        return Mathf.Round(subheight + height * 0.5f) * 2.0f  - Depth * 0.4f;
    }
}
