using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingRoadRender : MonoBehaviour, IBuildingRender 
{
    public GameMap Map;
    public Building Building;

    public static int[] Triangles = new int[]{ 0, 1, 2  };

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

        var tile = Map.TerrainData.GetTile(Building.PositionX, Building.PositionY);

        var offset = new Vector3(0.0f, 0.2f, 0.0f);

        var verticies = new List<Vector3>();
        verticies.Add(tile.Corner1 + offset);
        verticies.Add(tile.Corner2 + offset);
        verticies.Add(tile.Corner3 + offset);

        var mesh = new Mesh();
        meshFilter.mesh = mesh;

        mesh.vertices = verticies.ToArray();
        mesh.triangles = Triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }
}
