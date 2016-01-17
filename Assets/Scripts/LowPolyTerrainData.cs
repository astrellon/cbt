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

    public LowPolyTerrainTile GetTriCorner(int x, int y, int corner)
    {
        var isEven = ((x + y) % 2) == 0;

        if (isEven)
        {
            if (corner == 0) { return GetTile(x - 1, y); }
            if (corner == 1) { return GetTile(x + 1, y); }
            if (corner == 2) { return GetTile(x, y + 1); }
        }
        else
        {
            if (corner == 0) { return GetTile(x - 1, y); }
            if (corner == 1) { return GetTile(x + 1, y); }
            if (corner == 2) { return GetTile(x, y - 1); }
        }
        return LowPolyTerrainTile.NullTile;
    }
    
    float HexHeight(int x, int y, int offsetX, int offsetY)
    {
        var defaultHeight = GetTile(x, y).HeightCm;
        
        var height1 = GetHeightOrDefault(offsetX - 1, offsetY - 1, defaultHeight); 
        var height2 = GetHeightOrDefault(offsetX, offsetY - 1, defaultHeight); 
        var height3 = GetHeightOrDefault(offsetX + 1, offsetY - 1, defaultHeight);
        var height4 = GetHeightOrDefault(offsetX - 1, offsetY, defaultHeight); 
        var height5 = GetHeightOrDefault(offsetX, offsetY, defaultHeight);
        var height6 = GetHeightOrDefault(offsetX + 1, offsetY, defaultHeight); 
        //return VotedHeightHex(height1, height2, height3, height4, height5, height6);
        return (height1 + height2 + height3 + height4 + height5 + height6) / 6.0f;
        //return defaultHeight;
    }
    
    float GetHeightOrDefault(int x, int y, float defaultHeight)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return defaultHeight;
        }
        return GetTile(x, y).HeightCm;
    }

    public void CalcCornerHeights()
    {
        for (var y = 0; y < Height - 1; y++)
        {
            for (var x = 0; x < Width - 1; x++)
            {
                var isEven = ((x + y * Width) % 2) == 0;
            }
        }
    }

    public void SetTileType(int x, int y, string tileType)
    {
        GetTile(x, y).Type = tileType;
        TileTypes.Add(tileType);
    }
}
