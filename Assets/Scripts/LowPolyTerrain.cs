using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrain : MonoBehaviour 
{
    public LowPolyTerrainData TerrainData;
    public const float Size = 8.0f;

    public uint Width = 50;
    public uint Height = 50;
    public float Depth = 32.0f;
    public float OffsetX = 0;
    public float OffsetY = 0;

	// Use this for initialization
	void Start () 
    {
        TerrainData = new LowPolyTerrainData(Width, Height);

        for (var y = 0; y < TerrainData.Height; y++)
        {
            for (var x = 0; x < TerrainData.Width; x++)
            {
                var height = Mathf.PerlinNoise(Size * (OffsetX + x) / 64.0f, Size * (OffsetY + y) / 64.0f) * Depth; 
                //height = Mathf.Round(height * 2.0f) * 0.5f;
                height = Mathf.Round(height * 0.25f) * 4.0f;
                TerrainData.GetTile(x, y).Height = height;
            }
        }

        /*
        TerrainData.GetTile(0, 0).Height = 16.0f;
        TerrainData.GetTile(1, 0).Height = 16.0f;
        TerrainData.GetTile(0, 1).Height = 16.0f;
        TerrainData.GetTile(1, 1).Height = 16.0f;
        
        TerrainData.GetTile(2, 0).Height = 4.0f;
        TerrainData.GetTile(2, 1).Height = 4.0f;
        TerrainData.GetTile(2, 2).Height = 4.0f;
        TerrainData.GetTile(1, 2).Height = 4.0f;
        TerrainData.GetTile(0, 2).Height = 4.0f;
        
        TerrainData.GetTile(3, 0).Height = 2.0f;
        TerrainData.GetTile(3, 1).Height = 2.0f;
        TerrainData.GetTile(3, 2).Height = 2.0f;
        TerrainData.GetTile(3, 3).Height = 2.0f;
        TerrainData.GetTile(2, 3).Height = 2.0f;
        TerrainData.GetTile(1, 3).Height = 2.0f;
        TerrainData.GetTile(0, 3).Height = 2.0f;
        */

        CreateMesh();	

        transform.position = new Vector3(OffsetX * Size, 0, OffsetY * Size);
	}

    void CreateMesh()
    {
        if (TerrainData.Width == 0 || TerrainData.Height == 0)
        {
            Debug.Log("Cannot generate terrain data, too small");
            return;
        }

        var mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        var width = TerrainData.Width - 1;
        var height = TerrainData.Height - 1;
        var total = width * height;
        var newVerticies = new Vector3[total * 12];
        var newUvs = new Vector2[total * 12];
        var vi = 0;
        for (var y = 0; y < height; y++)
        {
            var ypos = y * Size;
            for (var x = 0; x < width; x++)
            {
                var blTile = TerrainData.GetTile(x, y);
                var brTile = TerrainData.GetTile(x + 1, y);
                var tlTile = TerrainData.GetTile(x, y + 1);
                var trTile = TerrainData.GetTile(x + 1, y + 1);

                var middleHeight = (tlTile.Height + trTile.Height + blTile.Height + brTile.Height) * 0.25f;

                var xpos = x * Size;
                newVerticies[vi    ] = new Vector3(xpos, tlTile.Height, ypos + Size);
                newVerticies[vi + 1] = new Vector3(xpos + Size, trTile.Height, ypos + Size);
                newVerticies[vi + 2] = new Vector3(xpos + Size * 0.5f, middleHeight, ypos + Size * 0.5f);

                newVerticies[vi + 3] = new Vector3(xpos + Size, trTile.Height, ypos + Size);
                newVerticies[vi + 4] = new Vector3(xpos + Size, brTile.Height, ypos);
                newVerticies[vi + 5] = new Vector3(xpos + Size * 0.5f, middleHeight, ypos + Size * 0.5f);

                newVerticies[vi + 6] = new Vector3(xpos + Size, brTile.Height, ypos);
                newVerticies[vi + 7] = new Vector3(xpos, blTile.Height, ypos);
                newVerticies[vi + 8] = new Vector3(xpos + Size * 0.5f, middleHeight, ypos + Size * 0.5f);
                
                newVerticies[vi + 9] = new Vector3(xpos, blTile.Height, ypos);
                newVerticies[vi +10] = new Vector3(xpos, tlTile.Height, ypos + Size);
                newVerticies[vi +11] = new Vector3(xpos + Size * 0.5f, middleHeight, ypos + Size * 0.5f);

                newUvs[vi    ] = new Vector2(0, 1);
                newUvs[vi + 1] = new Vector2(1, 1);
                newUvs[vi + 2] = new Vector2(0.5f, 0.5f);

                newUvs[vi + 3] = new Vector2(1, 1);
                newUvs[vi + 4] = new Vector2(1, 0);
                newUvs[vi + 5] = new Vector2(0.5f, 0.5f);
                
                newUvs[vi + 6] = new Vector2(1, 0);
                newUvs[vi + 7] = new Vector2(0, 0);
                newUvs[vi + 8] = new Vector2(0.5f, 0.5f);
                
                newUvs[vi + 9] = new Vector2(0, 0);
                newUvs[vi +10] = new Vector2(0, 1);
                newUvs[vi +11] = new Vector2(0.5f, 0.5f);

                vi += 12;
            }
        }

        var newTriangles = new int[total * 12];
        for (var ti = 0; ti < newTriangles.Length; ti++)
        {
            newTriangles[ti] = ti;
        }

        mesh.Clear();
        mesh.vertices = newVerticies;
        mesh.uv = newUvs;
        mesh.triangles = newTriangles;
        mesh.Optimize();
        mesh.RecalculateNormals();

        var collider = GetComponent<MeshCollider>();
        collider.sharedMesh = mesh;
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    float VotedHeight(Tile tl, Tile tr, Tile bl, Tile br)
    {
        var counts = new Dictionary<float, int>();
        counts[tl.HeightCm] = 0;
        counts[tr.HeightCm] = 0;
        counts[bl.HeightCm] = 0;
        counts[br.HeightCm] = 0;
        
        counts[tl.HeightCm]++;
        counts[tr.HeightCm]++;
        counts[bl.HeightCm]++;
        counts[br.HeightCm]++;

        var result = 0.0f;
        var maxCount = 0;
        foreach (var kvp in counts)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                result = kvp.Key;
            }
        }

        if (maxCount <= 1)
        {
            result = (tl.Height + tr.Height + bl.Height + br.Height) * 0.25f;
        }

        return result;
    }
}

/*
void Start () {
   
  mesh = GetComponent<MeshFilter> ().mesh;
   
  float x = transform.position.x;
  float y = transform.position.y;
  float z = transform.position.z;
   
  newVertices.Add( new Vector3 (x  , y  , z ));
  newVertices.Add( new Vector3 (x + 1 , y  , z ));
  newVertices.Add( new Vector3 (x + 1 , y-1 , z ));
  newVertices.Add( new Vector3 (x  , y-1 , z ));
   
  newTriangles.Add(0);
  newTriangles.Add(1);
  newTriangles.Add(3);
  newTriangles.Add(1);
  newTriangles.Add(2);
  newTriangles.Add(3);
   
  newUV.Add(new Vector2 (tUnit * tStone.x, tUnit * tStone.y + tUnit));
  newUV.Add(new Vector2 (tUnit * tStone.x + tUnit, tUnit * tStone.y + tUnit));
  newUV.Add(new Vector2 (tUnit * tStone.x + tUnit, tUnit * tStone.y));
  newUV.Add(new Vector2 (tUnit * tStone.x, tUnit * tStone.y));
   
  mesh.Clear ();
  mesh.vertices = newVertices.ToArray();
  mesh.triangles = newTriangles.ToArray();
  mesh.uv = newUV.ToArray(); // add this line to the code here
  mesh.Optimize ();
  mesh.RecalculateNormals ();
}
*/
