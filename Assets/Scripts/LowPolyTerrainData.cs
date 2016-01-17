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
}
