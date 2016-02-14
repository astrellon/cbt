using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingFloor
{
    public GameMap Map;
    public List<Vector2Int> Positions = new List<Vector2Int>();
    public List<WallEdge> WallEdges = new List<WallEdge>();
    public float FloorHeight;
    public float Height;
    public float BaseHeight;
}
