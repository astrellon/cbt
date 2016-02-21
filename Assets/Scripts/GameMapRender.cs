using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;

public class GameMapRender : MonoBehaviour 
{
    public GameMap Map;

    public LowPolyTreeRender Trees;
    public LowPolyTerrainSection Terrain;
    public BuildingsRender Buildings;

	// Use this for initialization
	void Start () 
    {
        Map = new GameMap();
        Map.TerrainData = LowPolyTerrainData.GetRandomMap();

        var building = new Building();
        building.Positions.Add(new Vector2Int(6, 1));
        building.Positions.Add(new Vector2Int(7, 1));
        building.Positions.Add(new Vector2Int(8, 1));
        building.Positions.Add(new Vector2Int(9, 1));
        building.Positions.Add(new Vector2Int(10, 1));
        building.Positions.Add(new Vector2Int(11, 1));
        building.Positions.Add(new Vector2Int(12, 1));
        building.Positions.Add(new Vector2Int(13, 1));
        building.Positions.Add(new Vector2Int(14, 1));
        building.Positions.Add(new Vector2Int(11, 2));
        building.Positions.Add(new Vector2Int(12, 2));
        building.Positions.Add(new Vector2Int(12, 3));
        building.Positions.Add(new Vector2Int(13, 3));
        building.Positions.Add(new Vector2Int(13, 4));
        building.Positions.Add(new Vector2Int(14, 4));
        building.Positions.Add(new Vector2Int(14, 5));
        building.Positions.Add(new Vector2Int(15, 5));
        building.RemoveTrees(Map.TerrainData);
        //Map.Buildings.Add(building);
        
        building = new Building();
        //building.Positions.Add(new Vector2Int(6, 2));
        building.Positions.Add(new Vector2Int(7, 2));
        building.Positions.Add(new Vector2Int(8, 2));
        building.Positions.Add(new Vector2Int(9, 2));
        building.Positions.Add(new Vector2Int(10, 2));
        building.Positions.Add(new Vector2Int(5, 3));
        building.Positions.Add(new Vector2Int(6, 3));
        building.Positions.Add(new Vector2Int(7, 3));
        building.Positions.Add(new Vector2Int(8, 3));
        building.Positions.Add(new Vector2Int(9, 3));
        building.Positions.Add(new Vector2Int(10, 3));
        building.Positions.Add(new Vector2Int(11, 3));
        //building.Positions.Add(new Vector2Int(5, 4));
        building.Positions.Add(new Vector2Int(6, 4));
        building.Positions.Add(new Vector2Int(7, 4));
        building.Positions.Add(new Vector2Int(8, 4));
        building.Positions.Add(new Vector2Int(9, 4));
        building.Positions.Add(new Vector2Int(10, 4));
        building.Positions.Add(new Vector2Int(11, 4));
        building.Positions.Add(new Vector2Int(10, 6));
        building.Positions.Add(new Vector2Int(11, 6));
        building.Positions.Add(new Vector2Int(12, 6));
        building.Type = "house";
        building.RemoveTrees(Map.TerrainData);
        //Map.Buildings.Add(building);
        
        building = new Building();
        
        var floor = new BuildingFloor();
        floor.Positions.Add(new Vector2Int(13, 2));
        floor.Positions.Add(new Vector2Int(14, 2));
        floor.Positions.Add(new Vector2Int(15, 2));
        floor.Positions.Add(new Vector2Int(16, 2));
        floor.Positions.Add(new Vector2Int(14, 3));
        floor.Positions.Add(new Vector2Int(15, 3));
        floor.Positions.Add(new Vector2Int(16, 3));

		floor.WallEdges.Add(new WallEdge(new Vector2Int(13, 2), 2));
		floor.WallEdges.Add(new WallEdge(new Vector2Int(13, 2), 0));

        floor.BaseHeight = 20.0f;
        floor.FloorHeight = 0.2f;
        floor.Height = 10.0f;
        floor.Map = Map;

        building.Floors.Add(floor);
        
        floor = new BuildingFloor();
        floor.Positions.Add(new Vector2Int(14, 2));
        floor.Positions.Add(new Vector2Int(15, 2));
        floor.Positions.Add(new Vector2Int(16, 2));
        floor.Positions.Add(new Vector2Int(14, 3));
        floor.Positions.Add(new Vector2Int(15, 3));
        floor.Positions.Add(new Vector2Int(16, 3));

		floor.WallEdges.Add(new WallEdge(new Vector2Int(14, 2), 2));
		floor.WallEdges.Add(new WallEdge(new Vector2Int(15, 2), 2));

        floor.BaseHeight = 30.0f;
        floor.FloorHeight = 0.2f;
        floor.Height = 10.0f;
        floor.Map = Map;
        building.Floors.Add(floor);

        floor = new BuildingFloor();
        floor.Positions.Add(new Vector2Int(15, 4));
        floor.Positions.Add(new Vector2Int(16, 4));
        floor.Positions.Add(new Vector2Int(17, 4));
        floor.Positions.Add(new Vector2Int(17, 3));
        floor.Positions.Add(new Vector2Int(18, 3));

		floor.WallEdges.Add(new WallEdge(new Vector2Int(15, 4), 0));
		floor.WallEdges.Add(new WallEdge(new Vector2Int(16, 4), 0));
		floor.WallEdges.Add(new WallEdge(new Vector2Int(17, 4), 1, 2));

        floor.BaseHeight = 22.0f;
        floor.FloorHeight = 0.2f;
        floor.Height = 10.0f;
        floor.Map = Map;
        building.Floors.Add(floor);

        building.Type = "generic";

        building.RemoveTrees(Map.TerrainData);
        Map.Buildings.Add(building);

        Init(Map);

        var geometry = new InputGeometry();

        //it is necessary to put a border around all the points in order to get triangulation to work correctly when holes are used
        var border = new List<Point>();
        border.Add(new Point(10, 6));
        border.Add(new Point(10, -6));
        border.Add(new Point(-10, -6));
        border.Add(new Point(-10, 6));
        geometry.AddRing(border);

        border = new List<Point>();
        border.Add(new Point(7, 4));
        border.Add(new Point(7, 2));
        border.Add(new Point(5, 2));
        border.Add(new Point(5, 4));
        geometry.AddRingAsHole(border);
        
        border = new List<Point>();
        border.Add(new Point(3, 2));
        border.Add(new Point(3, -6));
        border.Add(new Point(-3, -6));
        border.Add(new Point(-3, 2));
        geometry.AddRingAsHole(border);

        var meshRepresentation = new TriangleNet.Mesh();
        meshRepresentation.Triangulate(geometry);

        var triangleIndex = 0;
        var vertices = new List<Vector3>(meshRepresentation.triangles.Count * 3);
        var triangleIndices = new List<int>(meshRepresentation.triangles.Count * 3);

        foreach(var pair in meshRepresentation.triangles)
        {
            var triangle = pair.Value;

            var vertex0 = triangle.GetVertex(0);
            var vertex1 = triangle.GetVertex(1);
            var vertex2 = triangle.GetVertex(2);

			var p0 = new Vector3( vertex0.x, vertex0.y, 0 );
			var p1 = new Vector3( vertex1.x, vertex1.y, 0 );
			var p2 = new Vector3( vertex2.x, vertex2.y, 0 );

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);

            triangleIndices.Add(triangleIndex + 2);
            triangleIndices.Add(triangleIndex + 1);
            triangleIndices.Add(triangleIndex);

            triangleIndex += 3;
        }
        
        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleIndices.ToArray();
        mesh.RecalculateNormals();

        var newObj = new GameObject();
        newObj.name = "Test";
        newObj.transform.parent = transform;
        newObj.AddComponent<MeshFilter>();
        newObj.AddComponent<MeshRenderer>();
        newObj.GetComponent<MeshFilter>().mesh = mesh;

        /*
        var path = new List<ClipperLib.IntPoint>();
        path.Add(new ClipperLib.IntPoint(-3000, 0));
        path.Add(new ClipperLib.IntPoint(1000, 0));
        path.Add(new ClipperLib.IntPoint(6000, 3000));
        path.Add(new ClipperLib.IntPoint(6000, -3000));
        path.Add(new ClipperLib.IntPoint(1000, 0));
        //path.Add(new ClipperLib.IntPoint(5000, 0));
        //path.Add(new ClipperLib.IntPoint(1000, -3500));
        //path.Add(new ClipperLib.IntPoint(-3000, 0));
        
        var co = new ClipperLib.ClipperOffset();
        co.AddPath(path, ClipperLib.JoinType.jtSquare, ClipperLib.EndType.etClosedLine);

        var result = new List<List<ClipperLib.IntPoint>>();
        co.Execute(ref result, 500);
        foreach (var intpoint in path)
        {
            var point = new Vector3((float)intpoint.X / 1000.0f, 0, (float)intpoint.Y / 1000.0f);
            Debug.DrawLine(point, point + Vector3.up * 3, Color.cyan, 100);
        }

        foreach (var intpath in result)
        {
            foreach (var intpoint in intpath)
            {
                var point = new Vector3((float)intpoint.X / 1000.0f, 0, (float)intpoint.Y / 1000.0f);
                Debug.DrawLine(point, point + Vector3.up * 3, Color.red, 100);
            }
        }
        */
    }
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    public void Init(GameMap map)
    {
        Map = map;

        if (Trees != null)
        {
            Trees.Init(map.TerrainData);
        }
        if (Terrain != null)
        {
            Terrain.Init(map.TerrainData);
        }
        if (Buildings != null)
        {
            Buildings.Init(map);
        }
    }
}
