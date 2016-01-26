using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrainData
{
    public readonly int Width;
    public readonly int Height;
    public readonly LowPolyTerrainTile[] Data;
    public readonly HashSet<string> TileTypes = new HashSet<string>();

    public LowPolyTerrainData(int width, int height)
    {
        Width = width;
        Height = height;

        var total = width * height;
        Data = new LowPolyTerrainTile[total];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Data[x + y * Width] = new LowPolyTerrainTile(x, y);
            }
        }

        TileTypes.Add(Data[0].Type);
    }

    public LowPolyTerrainTile GetTile(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return LowPolyTerrainTile.NullTile;
        }
        return Data[y * Width + x];
    }

    public void SetTileType(int x, int y, string tileType)
    {
        GetTile(x, y).Type = tileType;
        TileTypes.Add(tileType);
    }

    public static LowPolyTerrainData GetRandomMap()
    {
        var result = new LowPolyTerrainData(111, 111);

        for (var y = 0; y < result.Height; y++)
        {
            for (var x = 0; x < result.Width; x++)
            {
                var tile = result.GetTile(x, y);

                var height1 = CalcHeight(result, tile.Corner1, 50);
                var height2 = CalcHeight(result, tile.Corner2, 50);
                var height3 = CalcHeight(result, tile.Corner3, 50);

                tile.SetCorner(height1, 0);
                tile.SetCorner(height2, 1);
                tile.SetCorner(height3, 2);
                
                var anyBelow = height1 < 4 || height2 < 4 || height3 < 4;
                var allAbove = height1 > 14 && height2 > 14 && height3 > 14;

                if (allAbove)
                {
                    tile.HasTree = CalcHasTree(result, tile.Corner1);
                    tile.TreeScale = CalcTreeScale(result, tile.Corner2);
                    tile.TreeRotation = CalcTreeRotation(result, tile.Corner3);
                }
                result.SetTileType(x, y, anyBelow ? "sand" : "grass");
            } 
        }

        return result;
    }
    
    static Vector2 CalcOffsetPos(LowPolyTerrainData terrainData, Vector3 position)
    {
        var xpos = LowPolyTerrainTile.TriHalfWidth + position.x;
        var ypos = LowPolyTerrainTile.TriHeight + position.z;
        return new Vector2(xpos, ypos);
    }

    static float CalcHeight(LowPolyTerrainData terrainData, Vector3 position, float depth)
    {
        var pos = CalcOffsetPos(terrainData, position);
        var subheight = Mathf.PerlinNoise(pos.x / 512.0f, pos.y / 512.0f) * depth * 5.0f - depth * 2.5f;
        var height = Mathf.PerlinNoise(pos.x / 128.0f, pos.y / 128.0f) * depth; 
        return Mathf.Round(subheight + height * 0.5f) * 2.0f  - depth * 0.4f;
    }
    static bool CalcHasTree(LowPolyTerrainData terrainData, Vector3 position)
    {
        var pos = CalcOffsetPos(terrainData, position);
        return Mathf.PerlinNoise(pos.x, pos.y) > 0.5f;
    }
    static float CalcTreeScale(LowPolyTerrainData terrainData, Vector3 position)
    {
        var pos = CalcOffsetPos(terrainData, position);
        return Mathf.PerlinNoise(pos.x, pos.y) * 5.0f + 5.0f;
    }
    static float CalcTreeRotation(LowPolyTerrainData terrainData, Vector3 position)
    {
        var pos = CalcOffsetPos(terrainData, position);
        return Mathf.PerlinNoise(pos.x, pos.y) * 360.0f;
    }
}
