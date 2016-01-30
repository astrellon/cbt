using UnityEngine;
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

    public Vector2Int(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }
}