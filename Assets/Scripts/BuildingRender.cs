using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingRender : MonoBehaviour {

    public Building Building;
    private static int[] triangles = new int[]{ 0, 1, 2  };

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateMesh(LowPolyTerrainData terrainData)
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();

        var tile = terrainData.GetTile(Building.PositionX, Building.PositionY);

        var offset = new Vector3(0.0f, 0.2f, 0.0f);

        var verticies = new List<Vector3>();
        verticies.Add(tile.Corner1 + offset);
        verticies.Add(tile.Corner2 + offset);
        verticies.Add(tile.Corner3 + offset);

        var mesh = new Mesh();
        meshFilter.mesh = mesh;

        mesh.vertices = verticies.ToArray();
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();


    }
}
