﻿using UnityEngine;
using System.Collections;

public class LowPolyTerrainTile
{
    public const float Size = 9.0f;
    public static readonly float TriWidth = Mathf.Sqrt(3.0f) * Size;
    public static readonly float TriHalfWidth = TriWidth * 0.5f;
    public static readonly float TriHeight = 1.5f * Size;

    public static readonly LowPolyTerrainTile NullTile = new LowPolyTerrainTile(-1, -1);

    public float Height = 0f;

    public Vector3 Corner1;
    public Vector3 Corner2;
    public Vector3 Corner3;

    public readonly Vector2Int Position;
    public readonly bool IsEven;

    public bool HasTree = false;
    public float TreeScale = 1.0f;
    public float TreeRotation = 0.0f;

    public string Type = "grass";

    public LowPolyTerrainTile(int x, int y)
    {
        Position = new Vector2Int(x, y);
        IsEven = ((x + y) % 2) == 0;

        var xpos = x * TriHalfWidth;
        var ypos = y * TriHeight;

        if (IsEven)
        {
            Corner1 = new Vector3(xpos, 0.0f, ypos + TriHeight);
            Corner2 = new Vector3(xpos + TriWidth, 0.0f, ypos + TriHeight);
            Corner3 = new Vector3(xpos + TriHalfWidth, 0.0f, ypos);
        }
        else
        {
            Corner1 = new Vector3(xpos, 0.0f, ypos);
            Corner2 = new Vector3(xpos + TriHalfWidth, 0.0f, ypos + TriHeight);
            Corner3 = new Vector3(xpos + TriWidth, 0.0f, ypos);
        }
    }

    public bool IsNull
    {
        get { return this == NullTile; }
    }

    public float HeightCm
    {
        get { return (float)System.Math.Round((decimal)Height, 2, System.MidpointRounding.AwayFromZero); }
    }

    public void SetCorner(float height, int corner)
    {
        if (corner == 0) { Corner1.y = height; }
        if (corner == 1) { Corner2.y = height; }
        if (corner == 2) { Corner3.y = height; }
    }
    public Vector3 GetCorner(int corner)
    {
        if (corner == 0) { return Corner1; }
        if (corner == 1) { return Corner2; }
        if (corner == 2) { return Corner3; }

        return Vector3.zero;
    }
    public Vector3 GetCornerLerp(int corner1, int corner2, float amount)
    {
        return Vector3.Lerp(GetCorner(corner1), GetCorner(corner2), amount);
    }
    public Vector3 Center
    {
        get { return (Corner1 + Corner2 + Corner3) * 0.333333333f; }
    }

    public Vector2Int GetEdgePosition(int edge)
    {
        if (IsEven)
        {
            if (edge == 0) return new Vector2Int(Position.x, Position.y + 1);
            if (edge == 1) return new Vector2Int(Position.x + 1, Position.y);
            if (edge == 2) return new Vector2Int(Position.x - 1, Position.y);
        }
        else
        {
            if (edge == 0) return new Vector2Int(Position.x - 1, Position.y);
            if (edge == 1) return new Vector2Int(Position.x + 1, Position.y);
            if (edge == 2) return new Vector2Int(Position.x, Position.y - 1);
        }

        return Vector2Int.Zero;
    }

    public Vector3Pair GetEdgeCorners(int edge, float offset = 0.0f)
    {
        if (offset == 0.0f)
        {
            if (edge == 0) return new Vector3Pair(Corner1, Corner2);
            if (edge == 1) return new Vector3Pair(Corner2, Corner3);
            if (edge == 2) return new Vector3Pair(Corner3, Corner1);
        }
        else
        {
            if (edge == 0) return new Vector3Pair(GetCornerLerp(0, 2, offset), GetCornerLerp(1, 2, offset));
            if (edge == 1) return new Vector3Pair(GetCornerLerp(1, 0, offset), GetCornerLerp(2, 0, offset));
            if (edge == 2) return new Vector3Pair(GetCornerLerp(2, 1, offset), GetCornerLerp(0, 1, offset));
        }

        return new Vector3Pair(Vector3.zero, Vector3.zero);
    }
}
