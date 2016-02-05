using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        Map.Buildings.Add(building);
        
        building = new Building();
        building.Positions.Add(new Vector2Int(6, 2));
        building.Positions.Add(new Vector2Int(7, 2));
        building.Positions.Add(new Vector2Int(8, 2));
        building.Positions.Add(new Vector2Int(9, 2));
        building.Positions.Add(new Vector2Int(10, 2));
        building.Positions.Add(new Vector2Int(5, 3));
        building.Positions.Add(new Vector2Int(6, 3));
        //building.Positions.Add(new Vector2Int(7, 3));
        building.Positions.Add(new Vector2Int(8, 3));
        building.Positions.Add(new Vector2Int(9, 3));
        building.Positions.Add(new Vector2Int(10, 3));
        building.Positions.Add(new Vector2Int(11, 3));
        building.Positions.Add(new Vector2Int(5, 4));
        building.Positions.Add(new Vector2Int(6, 4));
        building.Positions.Add(new Vector2Int(7, 4));
        building.Positions.Add(new Vector2Int(8, 4));
        building.Positions.Add(new Vector2Int(9, 4));
        building.Positions.Add(new Vector2Int(10, 4));
        building.Positions.Add(new Vector2Int(11, 4));
        building.Positions.Add(new Vector2Int(11, 6));
        building.Positions.Add(new Vector2Int(12, 6));
        building.Type = "house";
        building.RemoveTrees(Map.TerrainData);
        Map.Buildings.Add(building);
        Init(Map);
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
