﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrainRender : MonoBehaviour 
{
    public LowPolyTerrainData TerrainData;

    public LowPolyTreeRender Trees;
    public LowPolyTerrainSection Terrain;

	// Use this for initialization
	void Start () 
    {
        TerrainData = LowPolyTerrainData.GetRandomMap();

        if (Trees != null)
        {
            Trees.Init(TerrainData);
        }
        if (Terrain != null)
        {
            Terrain.Init(TerrainData);
        }

        var buildingObj = new GameObject();
        var buildingRender = buildingObj.AddComponent<BuildingRender>();
        buildingRender.Init(TerrainData);
        buildingObj.transform.parent = transform;
    }
	
	// Update is called once per frame
	void Update () 
    {
        
	}
}