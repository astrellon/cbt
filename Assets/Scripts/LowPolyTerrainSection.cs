using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LowPolyTerrainSection : MonoBehaviour 
{
    public LowPolyTerrainData TerrainData;

    public int StartX = 0;
    public int StartY = 0;
    public int Width = 50;
    public int Height = 50;
    //public float Depth = 32.0f;
    //public GameObject TreePrefab;

    public MaterialPair[] Materials;

    public Dictionary<string, Material> materialMap = new Dictionary<string, Material>();

	// Use this for initialization
	void Start () 
    {
        foreach (var pair in Materials)
        {
            materialMap[pair.Name] = pair.Material;
        }
        
        /*
        var maxX = Mathf.Min(StartX + Width, TerrainData.Width);
        var maxY = Mathf.Min(StartY + Height, TerrainData.Height);

        for (var y = StartY; y < maxY; y++)
        {
            for (var x = StartX; x < maxX; x++)
            {
                var tile = TerrainData.GetTile(x, y);

                var height1 = CalcHeight(tile.Corner1);
                var height2 = CalcHeight(tile.Corner2);
                var height3 = CalcHeight(tile.Corner3);

                tile.SetCorner(height1, 0);
                tile.SetCorner(height2, 1);
                tile.SetCorner(height3, 2);
                
                var anyBelow = height1 < 4 || height2 < 4 || height3 < 4;
                var allAbove = height1 > 14 && height2 > 14 && height3 > 14;

                if (allAbove)
                {
                    tile.HasTree = CalcHasTree(tile.Corner1);
                    tile.TreeScale = CalcTreeScale(tile.Corner2);
                    tile.TreeRotation = CalcTreeRotation(tile.Corner3);
                }
                TerrainData.SetTileType(x, y, anyBelow ? "sand" : "grass");
            } 
        }
        */

        /*
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
                
                var anyBelow = height1 < 4 || height2 < 4 || height3 < 4;
                var allAbove = height1 > 14 && height2 > 14 && height3 > 14;

                if (allAbove)
                {
                    tile.HasTree = CalcHasTree(tile.Corner1);
                    tile.TreeScale = CalcTreeScale(tile.Corner2);
                    tile.TreeRotation = CalcTreeRotation(tile.Corner3);
                }
                TerrainData.SetTileType(x, y, anyBelow ? "sand" : "grass");
            } 
        }
        */

        CreateMesh();	

        //transform.position = new Vector3(Offset.x * LowPolyTerrainTile.TriHalfWidth, 0, Offset.y * LowPolyTerrainTile.TriHeight);
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
            //tileRender.TreePrefab = TreePrefab;
            tileRender.CreateMesh(TerrainData, tileType, materialMap[tileType]);
        }
    }

    /*
    Vector2 CalcOffsetPos(Vector3 position)
    {
        var xpos = Offset.x * LowPolyTerrainTile.TriHalfWidth + position.x + SeedOffset.x * 100;
        var ypos = Offset.y * LowPolyTerrainTile.TriHeight + position.z + SeedOffset.y * 100;
        return new Vector2(xpos, ypos);
    }

    float CalcHeight(Vector3 position)
    {
        var pos = CalcOffsetPos(position);
        var subheight = Mathf.PerlinNoise(pos.x / 512.0f, pos.y / 512.0f) * Depth * 5.0f - Depth * 2.5f;
        var height = Mathf.PerlinNoise(pos.x / 128.0f, pos.y / 128.0f) * Depth; 
        return Mathf.Round(subheight + height * 0.5f) * 2.0f  - Depth * 0.4f;
    }
    bool CalcHasTree(Vector3 position)
    {
        var pos = CalcOffsetPos(position);
        return Mathf.PerlinNoise(pos.x, pos.y) > 0.5f;
    }
    float CalcTreeScale(Vector3 position)
    {
        var pos = CalcOffsetPos(position);
        return Mathf.PerlinNoise(pos.x, pos.y) * 5.0f + 5.0f;
    }
    float CalcTreeRotation(Vector3 position)
    {
        var pos = CalcOffsetPos(position);
        return Mathf.PerlinNoise(pos.x, pos.y) * 360.0f;
    }
    */
}
