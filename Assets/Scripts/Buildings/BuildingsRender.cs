using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingsRender : MonoBehaviour 
{
    public GameMap Map;

    public MaterialPair[] Materials;
    public Dictionary<string, Material> MaterialMap = new Dictionary<string, Material>();

    public GameObjectPair[] Prefabs;
    public Dictionary<string, GameObject> PrefabMap = new Dictionary<string, GameObject>();

    public void Init(GameMap map)
    {
        Map = map;

        foreach (var pair in Materials)
        {
            MaterialMap[pair.Name] = pair.Material;
        }
        foreach (var pair in Prefabs)
        {
            PrefabMap[pair.Name] = pair.Object;
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

        var script = BuildingRenderFactory.GetBuildingRender(buildingObj, Map, building);
        script.Render(this);
    }
}
