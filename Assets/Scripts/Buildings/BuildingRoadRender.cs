using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingRoadRender : MonoBehaviour, IBuildingRender 
{
    public GameMap Map;
    public Building Building;

    public static Vector3 Offset = new Vector3(0.0f, 0.2f, 0.0f);

    public void Init(GameMap map, Building building)
    {
        Map = map;
        Building = building;
    }

    public void Render(BuildingsRender buildingsRender)
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();

        Material material;
        if (buildingsRender.MaterialMap.TryGetValue(Building.Type, out material))
        {
            meshRenderer.material = material;
        }
        
        var verticies = new List<Vector3>();
        var triangles = new List<int>();

        foreach (var position in Building.Positions)
        {
            var tile = Map.TerrainData.GetTile(position.x, position.y);

            verticies.Add(tile.Corner1 + Offset);
            verticies.Add(tile.Corner2 + Offset);
            verticies.Add(tile.Corner3 + Offset);

            triangles.Add(triangles.Count);
            triangles.Add(triangles.Count);
            triangles.Add(triangles.Count);
        }
        var mesh = new Mesh();
        meshFilter.mesh = mesh;
        mesh.vertices = verticies.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
