﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public struct MaterialPair
{
    public string Name;
    public Material Material;
}

[System.Serializable]
public struct GameObjectPair
{
    public string Name;
    public GameObject Object;
}

[System.Serializable]
public struct Vector2Int
{
    public int x;
    public int y;

    public static readonly Vector2Int Zero = new Vector2Int();

    public Vector2Int(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }

    public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
    {
        return lhs.x != rhs.x && lhs.y != rhs.y;
    }
    public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public override bool Equals(object other)
    {
        if (!(other is Vector2Int))
        {
            return false;
        }

        return this == (Vector2Int)other;
    }
    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            hash = hash * 23 + x.GetHashCode();
            hash = hash * 23 + y.GetHashCode();
            return hash;
        }
    }
}

[System.Serializable]
public struct Vector3Pair
{
    public static readonly Vector3Pair Zero = new Vector3Pair(Vector3.zero, Vector3.zero);

    public readonly Vector3 V1;
    public readonly Vector3 V2;

    public Vector3Pair(Vector3 v1, Vector3 v2)
    {
        V1 = v1;
        V2 = v2;
    }
}

[System.Serializable]
public struct WallEdge
{
    public readonly Vector2Int TilePosition;
    public readonly int EdgeNumber;
    public readonly int WallLength;

    public WallEdge(Vector2Int tilePosition, int edgeNumber, int wallLength = 1)
    {
        TilePosition = tilePosition;
        EdgeNumber = edgeNumber;
        WallLength = wallLength;
    }

    public Vector3Pair GetPoints(GameMap map)
    {
        var tile = map.TerrainData.GetTile(TilePosition);
        var edgePair = tile.GetEdgeCorners(EdgeNumber);

        if (WallLength == 1)
        {
            return edgePair;
        }

        var endPoint = (edgePair.V2 - edgePair.V1) * WallLength + edgePair.V1;
        return new Vector3Pair(edgePair.V1, endPoint);
    }
}
