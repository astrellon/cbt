using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LowPolyTerrainTileRender : MonoBehaviour 
{
    private List<Vector3> verticies;
    private List<Vector2> uvs;
    private List<int> triangles;
    private LowPolyTerrainData terrainData;
    public GameObject TreePrefab;
    private GameObject TreeParent;

	// Use this for initialization
	void Start () 
    {
	}

    public void CreateMesh(LowPolyTerrainData terrainData, string tileType, Material material)
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();

        TreeParent = new GameObject();
        TreeParent.name = "TreeParent";
        TreeParent.transform.parent = transform;

        meshRenderer.material = material;

        verticies = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();
        this.terrainData = terrainData;

        var width = terrainData.Width - 1;
        var height = terrainData.Height - 1;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var tile = terrainData.GetTile(x, y);
                if (tile.Type == tileType)
                {
                    RenderTileTriangle(x, y);
                }
            }
        }

        var mesh = new Mesh();
        meshFilter.mesh = mesh;

        mesh.vertices = verticies.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
    }

    private void RenderTileTriangle(int x, int y)
    {
        var tile = terrainData.GetTile(x, y);

        var vi = verticies.Count;

        verticies.Add(tile.Corner1); 
        verticies.Add(tile.Corner2); 
        verticies.Add(tile.Corner3); 
        
        for (var i = vi; i < vi + 3; i++)
        {
            triangles.Add(i);
        }

        if (tile.HasTree)
        {
            AddTree(tile);
        }
    }

    private void AddTree(LowPolyTerrainTile tile)
    {
        if (TreePrefab == null)
        {
            return;
        }

        var center = (tile.Corner1 + tile.Corner2 + tile.Corner3) / 3.0f;

        var newTree = Instantiate(TreePrefab, center, Quaternion.identity) as GameObject;
        newTree.transform.localScale = Vector3.one * tile.TreeScale;
        newTree.transform.localEulerAngles = new Vector3(0, tile.TreeRotation, 0);
        newTree.transform.parent = TreeParent.transform;
    }
}
