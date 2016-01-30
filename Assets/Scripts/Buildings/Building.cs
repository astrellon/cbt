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
}
