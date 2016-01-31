using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building
{
    public List<Vector2Int> Positions = new List<Vector2Int>();

    public string Type = "road";

    public void RemoveTrees(LowPolyTerrainData terrainData)
    {
        foreach (var position in Positions)
        {
            terrainData.GetTile(position.x, position.y).HasTree = false;
        }
    }

    public bool HasPosition(Vector2Int position)
    {
        for (var i = 0; i < Positions.Count; i++)
        {
            if (Positions[i] == position)
            {
                return true;
            }
        }
        return false;
    }

    public float GetLowestPoint(LowPolyTerrainData terrainData)
    {
        var lowest = float.MaxValue;
        foreach (var position in Positions)
        {
            var tile = terrainData.GetTile(position.x, position.y);
            if (tile.Corner1.y < lowest)
            {
                lowest = tile.Corner1.y;
            }
            if (tile.Corner2.y < lowest)
            {
                lowest = tile.Corner2.y;
            }
            if (tile.Corner3.y < lowest)
            {
                lowest = tile.Corner3.y;
            }
        }
        return lowest;
    }
    public float GetHighestPoint(LowPolyTerrainData terrainData)
    {
        var highest = float.MinValue;
        foreach (var position in Positions)
        {
            var tile = terrainData.GetTile(position.x, position.y);
            if (tile.Corner1.y > highest)
            {
                highest = tile.Corner1.y;
            }
            if (tile.Corner2.y > highest)
            {
                highest = tile.Corner2.y;
            }
            if (tile.Corner3.y > highest)
            {
                highest = tile.Corner3.y;
            }
        }
        return highest;
    }
}
