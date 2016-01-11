using UnityEngine;
using System.Collections;

public class Tile
{
    public float Height = 0f;

    public float HeightCm
    {
        get { return (float)System.Math.Round((decimal)Height, 2, System.MidpointRounding.AwayFromZero); }
    }
}
