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

            RenderFloor(tile, highest, false);

            var surrounded = true;
            for (var i = 0; i < 3; i++)
            {
                var edgePosition = tile.GetEdgePosition(i);
                if (!Building.HasPosition(edgePosition))
                {
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
}
