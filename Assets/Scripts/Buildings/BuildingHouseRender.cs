using UnityEngine;
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

        foreach (var position in Building.Positions)
        {
            var tile = Map.TerrainData.GetTile(position.x, position.y);

            RenderFloor(tile);

            var surrounded = true;
            for (var i = 0; i < 3; i++)
            {
                var edgePosition = tile.GetEdgePosition(i);
                if (!Building.HasPosition(edgePosition))
                {
                    surrounded = false;
                    RenderDownwall(tile, i);
                }
            }

            if (!surrounded)
            {
                RenderPillar(prefab, tile);
            }
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
    }

    public static Vector3 MatchHighest(Vector3 input, float highest)
    {
        return new Vector3(input.x, highest, input.z);
    }

    private void RenderFloor(LowPolyTerrainTile tile)
    {
        vertices.Add(MatchHighest(tile.Corner1, highest));
        vertices.Add(MatchHighest(tile.Corner2, highest));
        vertices.Add(MatchHighest(tile.Corner3, highest));

        triangles.Add(triangles.Count);
        triangles.Add(triangles.Count);
        triangles.Add(triangles.Count);
    }

    private void RenderPillar(GameObject prefab, LowPolyTerrainTile tile)
    {
        var newObj = Instantiate(prefab);
        newObj.transform.parent = transform;
        newObj.transform.localPosition = MatchHighest(tile.Center, highest);
    }

    private void RenderDownwall(LowPolyTerrainTile tile, int edge)
    {
        var edgePair = tile.GetEdgeCorners(edge);

        vertices.Add(MatchHighest(edgePair.V1, highest));
        vertices.Add(MatchHighest(edgePair.V1, lowest));
        vertices.Add(MatchHighest(edgePair.V2, highest));
        
        vertices.Add(MatchHighest(edgePair.V2, highest));
        vertices.Add(MatchHighest(edgePair.V1, lowest));
        vertices.Add(MatchHighest(edgePair.V2, lowest));

        for (var i = 0; i < 6; i++)
        {
            triangles.Add(triangles.Count);
        }
    }
}
