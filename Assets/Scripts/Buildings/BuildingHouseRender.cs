using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
  
public class BuildingHouseRender : MonoBehaviour, IBuildingRender 
{
    public GameMap Map;
    public Building Building;
    
    public static Vector3 Offset = new Vector3(0.0f, 0.2f, 0.0f);

    private List<Vector3> vertices;
    private List<int> triangles;
    private float highest = float.MinValue;
    private float lowest = float.MinValue;

    public void Init(GameMap map, Building building)
    {
        Map = map;
        Building = building;
    }

    public void Render(BuildingsRender buildingsRender)
    {
        GameObject prefab;
        if (!buildingsRender.PrefabMap.TryGetValue("pillar", out prefab))
        {
            Debug.Log("Cannot find pillar prefab");
            return;
        }
        
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        var meshCollider = gameObject.AddComponent<MeshCollider>();

        Material material;
        if (buildingsRender.MaterialMap.TryGetValue(Building.Type, out material))
        {
            meshRenderer.material = material;
        }
        
        vertices = new List<Vector3>();
        triangles = new List<int>();

        highest = Building.GetHighestPoint(Map.TerrainData) + 0.2f;
        lowest = Building.GetLowestPoint(Map.TerrainData) - 0.2f;

        var buildingWall = new BuildingWalls();

        foreach (var position in Building.Positions)
        {
            var tile = Map.TerrainData.GetTile(position.x, position.y);

            RenderFloor(tile, highest, false);

            var surrounded = true;
            for (var i = 0; i < 3; i++)
            {
                var edgePosition = tile.GetEdgePosition(i);
                if (!Building.HasPosition(edgePosition))
                {
                    var edgePoints = tile.GetEdgeCorners(i);
                    buildingWall.Edges.AddPair(edgePoints);

                    surrounded = false;
                    RenderDownwall(tile, highest, highest - 1.0f, i);
                }
            }
            
            RenderFloor(tile, highest - 1.0f, true);

            if (!surrounded)
            {
                RenderPillar(prefab, tile, highest, false);
                RenderPillar(prefab, tile, highest - 1.0f, true, highest - lowest);
            }
        }

        buildingWall.OffsetWalls(-2.0f, highest);
        RenderPoints(buildingWall.Points, false);
        //buildingWall.OffsetWalls(-2.0f, highest);
        RenderPoints(buildingWall.Points, true);

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        vertices = null;
        triangles = null;
    }

    public static Vector3 MatchHighest(Vector3 input, float highest)
    {
        return new Vector3(input.x, highest, input.z);
    }

    private void RenderFloor(LowPolyTerrainTile tile, float height, bool swapOrder)
    {
        vertices.Add(MatchHighest(tile.Corner1, height));

        var corner2 = MatchHighest(tile.Corner2, height);
        var corner3 = MatchHighest(tile.Corner3, height);
        vertices.Add(swapOrder ? corner3 : corner2);
        vertices.Add(swapOrder ? corner2 : corner3);

        triangles.Add(triangles.Count);
        triangles.Add(triangles.Count);
        triangles.Add(triangles.Count);
    }

    private void RenderPillar(GameObject prefab, LowPolyTerrainTile tile, float height, bool rotateDownwards, float heightScale = 1.0f)
    {
        var newObj = Instantiate(prefab);
        newObj.transform.parent = transform;
        newObj.transform.localPosition = MatchHighest(tile.Center, height);
        if (rotateDownwards)
        {
            newObj.transform.Rotate(180, 0, 0);
        }
        newObj.transform.localScale = new Vector3(1.0f, heightScale, 1.0f);
    }

    private void RenderDownwall(LowPolyTerrainTile tile, float upper, float lower, int edge)
    {
        var edgePair = tile.GetEdgeCorners(edge);

        vertices.Add(MatchHighest(edgePair.V1, upper));
        vertices.Add(MatchHighest(edgePair.V1, lower));
        vertices.Add(MatchHighest(edgePair.V2, upper));
        
        vertices.Add(MatchHighest(edgePair.V2, upper));
        vertices.Add(MatchHighest(edgePair.V1, lower));
        vertices.Add(MatchHighest(edgePair.V2, lower));

        for (var i = 0; i < 6; i++)
        {
            triangles.Add(triangles.Count);
        }
    }
    
    private void RenderUpwardWall(LowPolyTerrainTile tile, float offset, float upper, float lower, int edge, bool swapOrder)
    {
        var edgePair = tile.GetEdgeCorners(edge, offset);

        vertices.Add(MatchHighest(swapOrder ? edgePair.V2 : edgePair.V1, upper));
        vertices.Add(MatchHighest(swapOrder ? edgePair.V2 : edgePair.V1, lower));
        vertices.Add(MatchHighest(swapOrder ? edgePair.V1 : edgePair.V2, upper));
        
        vertices.Add(MatchHighest(swapOrder ? edgePair.V1 : edgePair.V2, upper));
        vertices.Add(MatchHighest(swapOrder ? edgePair.V2 : edgePair.V1, lower));
        vertices.Add(MatchHighest(swapOrder ? edgePair.V1 : edgePair.V2, lower));

        for (var i = 0; i < 6; i++)
        {
            triangles.Add(triangles.Count);
        }
    }

    private void RenderPoints(List<List<Vector3>> points, bool backwards)
    {
        foreach (var path in points)
        {
            if (backwards)
            {
                for (var i = path.Count - 1; i > 0; i--)
                {
                    RenderPointPair(path[i], path[i - 1], Vector3.up * 4);
                }

                RenderPointPair(path.First(), path.Last(), Vector3.up * 4);
            }
            else
            {
                for (var i = 0; i < path.Count - 1; i++)
                {
                    RenderPointPair(path[i], path[i + 1], Vector3.up * 4);
                }

                RenderPointPair(path.Last(), path.First(), Vector3.up * 4);
            }
        }
    }

    private void RenderPointPair(Vector3 first, Vector3 second, Vector3 offset)
    {
        vertices.Add(first);
        vertices.Add(first + offset);
        vertices.Add(second);

        vertices.Add(second);
        vertices.Add(first + offset);
        vertices.Add(second + offset);

        for (var t = 0; t < 6; t ++)
        {
            triangles.Add(triangles.Count);
        }
    }

    private static ClipperLib.IntPoint CreateIntPoint(Vector3 position)
    {
        var scaled = position * 1000.0f;
        return new ClipperLib.IntPoint(scaled.x, scaled.z);
    }

    public class PolygonSoup
    {
        public List<Vector3Pair> Pairs = new List<Vector3Pair>();

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
            var result = new List<List<Vector3>>();

            while (Pairs.Count > 0)
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
            var result = Pairs[Pairs.Count - 1];
            Pairs.RemoveAt(Pairs.Count - 1);
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
            for (var i = 0; i < Pairs.Count; i++)
            {
                var pair = Pairs[i];
                if (pair.V1 == point)
                {
                    result = pair.V2;
                    Pairs.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }

    public class BuildingWalls
    {
        public PolygonSoup Edges = new PolygonSoup();

        public List<List<Vector3>> Points;

        public void OffsetWalls(float amount, float height)
        {
            var orderedPoints = Edges.GetAllOrderedIntPoints();
            var co = new ClipperLib.ClipperOffset();
            co.AddPaths(orderedPoints, ClipperLib.JoinType.jtSquare, ClipperLib.EndType.etClosedPolygon);

            var results = new List<List<ClipperLib.IntPoint>>();
            co.Execute(ref results, (int)(amount * 1000.0f));

            Points = results.Select(
                    xs => xs.Select(
                        x => new Vector3((float)x.X / 1000.0f, height, (float)x.Y / 1000.0f)).ToList()).ToList();

        }
    }
}
