using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingFloor
{
    public GameMap Map;
    public List<Vector2Int> Positions = new List<Vector2Int>();
    public List<Vector3Pair> WallEdges = new List<Vector3Pair>();
    public float FloorHeight;
    public float Height;
    public float BaseHeight;
}
