using UnityEngine;
using System.Collections;

public class LowPolyTerrainData
{
    public readonly uint Width;
    public readonly uint Height;
    public readonly Tile[] Data;

    public LowPolyTerrainData(uint width, uint height)
    {
        Width = width;
        Height = height;

        var total = width * height;
        Data = new Tile[total];

        for (var i = 0; i < total; i++)
        {
            Data[i] = new Tile();
        }
    }

    public Tile GetTile(int x, int y)
    {
        return Data[y * Width + x];
    }
}
