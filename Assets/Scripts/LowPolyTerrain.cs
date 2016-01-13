using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrain : MonoBehaviour 
{
    public LowPolyTerrainData TerrainData;
    public const float Size = 8.0f;

    public uint Width = 50;
    public uint Height = 50;
    public float Depth = 32.0f;
    public float OffsetX = 0;
    public float OffsetY = 0;

    [System.Serializable]
    public struct MaterialPair
    {
        public string Name;
        public Material Material;
    }

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
                var height = Mathf.PerlinNoise(Size * (OffsetX + x) / 64.0f, Size * (OffsetY + y) / 64.0f) * Depth; 
                height = Mathf.Round(height * 0.25f) * 4.0f;

                var tile = TerrainData.GetTile(x, y);
                tile.Height = height;
                TerrainData.SetTileType(x, y, height < 10 ? "sand" : "grass");
            } 
        }

        for (var y = 0; y < TerrainData.Height - 1; y++)
        {
            for (var x = 0; x < TerrainData.Width - 1; x++)
            {
                var blTile = TerrainData.GetTile(x, y);
                var brTile = TerrainData.GetTile(x + 1, y);
                var tlTile = TerrainData.GetTile(x, y + 1);
                var trTile = TerrainData.GetTile(x + 1, y + 1);

                var anyBelow = blTile.Height < 10 || brTile.Height < 10 || tlTile.Height < 10 || trTile.Height < 10;
                TerrainData.SetTileType(x, y, anyBelow ? "sand" : "grass");
            }
        }

        CreateMesh();	

        transform.position = new Vector3(OffsetX * Size, 0, OffsetY * Size);
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
            var tileRender = tileMesh.AddComponent<LowPolyTerrainTileRender>() as LowPolyTerrainTileRender;
            tileRender.CreateMesh(TerrainData, tileType, materialMap[tileType]);
        }
    }
}
