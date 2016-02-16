using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PolygonSoup
{
    public class Path
    {
        public readonly List<Vector3> Points = new List<Vector3>();
        public bool IsClosedLoop {get; private set;}

        public void CheckIfClosedLoop()
        {
            if (Points.First() == Points.Last())
            {
                Points.RemoveAt(Points.Count - 1);
                IsClosedLoop = true;
            }
        }
    }
    public class PathInt
    {
        public readonly List<ClipperLib.IntPoint> Points = new List<ClipperLib.IntPoint>();
        public readonly bool IsClosedLoop;

        public PathInt (Path path)
        {
            foreach (var point in path.Points)
            {
                Points.Add(new ClipperLib.IntPoint(point.x * 1000.0f, point.z * 1000.0f));
            }
            IsClosedLoop = path.IsClosedLoop;
        }
    }
    public class Line
    {
        public readonly List<Vector3Pair> Segments = new List<Vector3Pair>();
        public bool IsLoop { get; private set; }

        public void AddPair(Vector3Pair pair)
        {
            Segments.Add(pair);
        }

        public void ProcessSegments()
        {
            if (Segments.Count() == 0)
            {
                return;
            }

            var visited = new HashSet<Vector3Pair>();
            var result = new List<Vector3Pair>();
            var afterStart = new List<Vector3Pair>();
            var beforeStart = new List<Vector3Pair>();

            var currentSegment = Segments[0];
            afterStart.Add(currentSegment);
            visited.Add(currentSegment);

            for (var i = 0; i < Segments.Count; i++)
            {
                Vector3Pair connected = Vector3Pair.Zero;
                var foundConnected = FindConnectingPoint(currentSegment, false, ref connected);
                if (!foundConnected)
                {
                    continue;
                }

                if (visited.Contains(connected))
                {
                    IsLoop = true;
                    continue;
                }

                afterStart.Add(connected);
                currentSegment = connected;
            }
            
            currentSegment = Segments[0];
            for (var i = Segments.Count - 1; i >= 0; i--)
            {
                Vector3Pair connected = Vector3Pair.Zero;
                var foundConnected = FindConnectingPoint(currentSegment, true, ref connected);
                if (!foundConnected)
                {
                    continue;
                }

                if (visited.Contains(connected))
                {
                    IsLoop = true;
                    continue;
                }
            }
        }

        private IEnumerable<Vector3Pair> GetSegments(bool backwards)
        {
            if (backwards)
            {
                for (var i = Segments.Count() - 1; i >= 0; i--)
                {
                    yield return Segments[i];
                }
            }
            else
            {
                for (var i = 0; i < Segments.Count(); i++)
                {
                    yield return Segments[i];
                }
            }
        }

        private bool FindConnectingPoint(Vector3Pair pair, bool reverseCheck, ref Vector3Pair result)
        {
            /*
            foreach (var segment in GetSegments(reverseCheck))
            {
                if (segment == pair)
                {
                    continue;
                }

                if (pair.V1 == segment.V1 || pair.V1 == segment.V2 ||
                    pair.V2 == segment.V1 || pair.V2 == semgnet.V2)
                {
                    result = segment;
                    return true;
                }
            }
            */

            return false;
        }
    }

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

    public List<Path> GetAllOrderedPoints()
    {
        pairList = new List<Vector3Pair>(Pairs);

        var result = new List<Path>();

        while (pairList.Count > 0)
        {
            result.Add(GetOrderedPairs());
        }

        return result;
    }
    public List<PathInt> GetAllOrderedIntPoints()
    {
        var orderedPoints = GetAllOrderedPoints();

        var result = new List<PathInt>();

        foreach (var points in orderedPoints)
        {
            result.Add(new PathInt(points));
        }

        return result;
    }

    private Vector3Pair PopPair()
    {
        var result = pairList[pairList.Count - 1];
        pairList.RemoveAt(pairList.Count - 1);
        return result;
    }

	private Path GetOrderedPairs()
    {
        var result = new Path();
        var start = PopPair();
        result.Points.Add(start.V1);
        result.Points.Add(start.V2);

        var currentPoint = start.V2;
        var foundConnectingPoint = true;
        while (foundConnectingPoint)
        {
            foundConnectingPoint = FindConnectingPoint(currentPoint, ref currentPoint);
            if (foundConnectingPoint)
            {
                result.Points.Add(currentPoint);
            }
        }

        result.CheckIfClosedLoop();

        return result;
    }

    private bool FindConnectingPoint(Vector3 point, ref Vector3 result)
    {
        for (var i = 0; i < pairList.Count; i++)
        {
            var pair = pairList[i];
            var connectResult = IsConnectingFirst(pair, point);
            if (connectResult == 0)
            {
                break;
            }

            if (connectResult == 1)
            {
                result = pair.V2;
            }
            else if (connectResult == 2)
            {
                result = pair.V1;
            }

            pairList.RemoveAt(i);
            return true;
        }
        return false;
    }

    private int IsConnectingFirst(Vector3Pair pair, Vector3 point)
    {
        if (pair.V1 == point)
        {
            return 1;
        }
        if (pair.V2 == point)
        {
            return 2;
        }
        return 0;
    }

    public List<List<Vector3>> Offset(float amount, float height)
    {
        var orderedPoints = GetAllOrderedIntPoints();

        var result = new List<List<Vector3>>();

        foreach (var path in orderedPoints)
        {
            Debug.Log("New path");
            foreach (var intpoint in path.Points)
            {
                Debug.Log("Ordered point: " + intpoint.X + ", " + intpoint.Y);
            }
            var co = new ClipperLib.ClipperOffset();
            var endType = path.IsClosedLoop ? ClipperLib.EndType.etClosedLine : ClipperLib.EndType.etOpenSquare;
            co.AddPath(path.Points, ClipperLib.JoinType.jtSquare, endType);

            var results = new List<List<ClipperLib.IntPoint>>();
            co.Execute(ref results, (int)(amount * 1000.0f));

            result.AddRange(results.Select(
                    xs => xs.Select(
                        x => new Vector3((float)x.X / 1000.0f, height, (float)x.Y / 1000.0f)).ToList()));
        }

        return result;
    }
}
