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
        building.PositionX = 2;
        building.PositionY = 2;
        Map.Buildings.Add(building);
        
        building = new Building();
        building.PositionX = 3;
        building.PositionY = 2;
        Map.Buildings.Add(building);
        
        building = new Building();
        building.PositionX = 4;
        building.PositionY = 2;
        Map.Buildings.Add(building);
        
        building = new Building();
        building.PositionX = 5;
        building.PositionY = 2;
        building.Type = "house";
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
