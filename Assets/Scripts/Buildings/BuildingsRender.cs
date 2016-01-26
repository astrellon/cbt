using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsRender : MonoBehaviour {

    public GameMap Map;

    public MaterialPair[] Materials;
    public Dictionary<string, Material> MaterialMap = new Dictionary<string, Material>();

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void Init(GameMap map)
    {
        Map = map;

        foreach (var pair in Materials)
        {
            MaterialMap[pair.Name] = pair.Material;
        }

        foreach (var building in Map.Buildings)
        {
            CreateBuilding(building);
        }
    }

    void CreateBuilding(Building building)
    {
        var buildingObj = new GameObject();
        buildingObj.transform.parent = transform;

        var buildingScript = buildingObj.AddComponent<BuildingRender>();
        buildingScript.MaterialMap = MaterialMap;
        buildingScript.Init(Map, building);
    }
}
