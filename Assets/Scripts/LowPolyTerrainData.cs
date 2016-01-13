using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrainData
{
    public readonly uint Width;
    public readonly uint Height;
    public readonly LowPolyTerrainTile[] Data;
    public readonly HashSet<string> TileTypes = new HashSet<string>();

    public LowPolyTerrainData(uint width, uint height)
    {
        Width = width;
        Height = height;

        var total = width * height;
        Data = new LowPolyTerrainTile[total];

        for (var i = 0; i < total; i++)
        {
            Data[i] = new LowPolyTerrainTile();
        }

        TileTypes.Add(Data[0].Type);
    }

    public LowPolyTerrainTile GetTile(int x, int y)
    {
        return Data[y * Width + x];
    }

    public void SetTileType(int x, int y, string tileType)
    {
        GetTile(x, y).Type = tileType;
        TileTypes.Add(tileType);
    }
}
