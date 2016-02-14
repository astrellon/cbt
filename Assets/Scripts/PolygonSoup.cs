using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PolygonSoup
{
    public List<Vector3Pair> Pairs = new List<Vector3Pair>();
    private List<Vector3Pair> pairList;

    public void AddPair(Vector3Pair pair)
    {
        Pairs.Add(pair);
    }
    public void AddPairs(List<Vector3Pair> pairs)
    {
        Pairs.AddRange(pairs);
    }

    public List<List<Vector3>> GetAllOrderedPoints()
    {
        pairList = new List<Vector3Pair>(Pairs);

        var result = new List<List<Vector3>>();

        while (pairList.Count > 0)
        {
            result.Add(GetOrderedPairs());
        }

        return result;
    }
    public List<List<ClipperLib.IntPoint>> GetAllOrderedIntPoints()
    {
        var orderedPoints = GetAllOrderedPoints();

        var result = new List<List<ClipperLib.IntPoint>>();

        foreach (var points in orderedPoints)
        {
            var resultPoints = new List<ClipperLib.IntPoint>();
            result.Add(resultPoints);
            foreach (var point in points)
            {
                resultPoints.Add(new ClipperLib.IntPoint(point.x * 1000.0f, point.z * 1000.0f));
            }
        }

        return result;
    }

    private Vector3Pair PopPair()
    {
        var result = pairList[pairList.Count - 1];
        pairList.RemoveAt(pairList.Count - 1);
        return result;
    }

    private List<Vector3> GetOrderedPairs()
    {
        var result = new List<Vector3>();
        var start = PopPair();
        result.Add(start.V1);
        result.Add(start.V2);

        var currentPoint = start.V2;
        var foundConnectingPoint = true;
        while (foundConnectingPoint)
        {
            foundConnectingPoint = FindConnectingPoint(currentPoint, ref currentPoint);
            if (foundConnectingPoint)
            {
                result.Add(currentPoint);
            }
        }

        return result;
    }

    private bool FindConnectingPoint(Vector3 point, ref Vector3 result)
    {
        for (var i = 0; i < pairList.Count; i++)
        {
            var pair = pairList[i];
            if (pair.V1 == point)
            {
                result = pair.V2;
                pairList.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public List<List<Vector3>> Offset(float amount, float height)
    {
        var orderedPoints = GetAllOrderedIntPoints();
        var co = new ClipperLib.ClipperOffset();
		co.AddPaths(orderedPoints, ClipperLib.JoinType.jtSquare, ClipperLib.EndType.etOpenSquare);

        var results = new List<List<ClipperLib.IntPoint>>();
        co.Execute(ref results, (int)(amount * 1000.0f));

        return results.Select(
                xs => xs.Select(
                    x => new Vector3((float)x.X / 1000.0f, height, (float)x.Y / 1000.0f)).ToList()).ToList();

    }
}
