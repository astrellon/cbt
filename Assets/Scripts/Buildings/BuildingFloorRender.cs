using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingFloorRender : MonoBehaviour 
{
    private List<Vector3> vertices;
    private List<int> triangles;
    public BuildingFloor Floor;

	// Use this for initialization
	void Start () 
    {
	
	}
	
    public void Render(BuildingsRender buildingsRender)
    {
        GameObject prefab;
        if (!buildingsRender.PrefabMap.TryGetValue("pillar", out prefab))
        {
            Debug.Log("Cannot find pillar prefab");
            return;
        }

        var floorTilesObj = RenderFloorTiles(buildingsRender);
        floorTilesObj.transform.parent = transform;
        
        var transPos = MatchHighest(transform.localPosition, Floor.BaseHeight);
        gameObject.transform.localPosition = transPos;
    }
    
    private void RenderFloorTile(LowPolyTerrainTile tile, float height, bool swapOrder)
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

    private GameObject RenderFloorTiles(BuildingsRender buildingsRender)
    {
        var floorTilesObj = new GameObject();
        floorTilesObj.name = "Tiles";

        var meshFilter = floorTilesObj.AddComponent<MeshFilter>();
        var meshRenderer = floorTilesObj.AddComponent<MeshRenderer>();
        var meshCollider = floorTilesObj.AddComponent<MeshCollider>();
        
        Material material;
        if (buildingsRender.MaterialMap.TryGetValue("house", out material))
        {
            meshRenderer.material = material;
        }

        vertices = new List<Vector3>();
        triangles = new List<int>();
        
        foreach (var position in Floor.Positions)
        {
            var tile = Floor.Map.TerrainData.GetTile(position.x, position.y);

            RenderFloorTile(tile, Floor.FloorHeight, false);
            RenderFloorTile(tile, 0.0f, true);
        }

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        vertices = null;
        triangles = null;

        return floorTilesObj;
    }
    private GameObject RenderWalls(BuildingsRender buildingsRender)
    {
        var wallObj = new GameObject();
        wallObj.name = "Tiles";

        var meshFilter = wallObj.AddComponent<MeshFilter>();
        var meshRenderer = wallObj.AddComponent<MeshRenderer>();
        var meshCollider = wallObj.AddComponent<MeshCollider>();
        
        Material material;
        if (buildingsRender.MaterialMap.TryGetValue("house", out material))
        {
            meshRenderer.material = material;
        }

        vertices = new List<Vector3>();
        triangles = new List<int>();
        
        /*
        foreach (var position in Floor.WallEdges)
        {
            var tile = Floor.Map.TerrainData.GetTile(position.x, position.y);

            RenderFloorTile(tile, Floor.FloorHeight, false);
            RenderFloorTile(tile, 0.0f, true);
        }
        */

        var mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
        
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        vertices = null;
        triangles = null;

        return wallObj;
    }
    
    public static Vector3 MatchHighest(Vector3 input, float highest)
    {
        return new Vector3(input.x, highest, input.z);
    }
}
