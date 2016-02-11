using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingGeneric : MonoBehaviour, IBuildingRender 
{
    public GameMap Map;
    public Building Building;

    private List<Vector3> vertices;
    private List<int> triangles;

    public void Init(GameMap map, Building building)
    {
        Map = map;
        Building = building;
    }

    public void Render(BuildingsRender buildingsRender)
    {
        var floorCount = 1;
        foreach (var floor in Building.Floors)
        {
            var floorObj = new GameObject();
            floorObj.transform.parent = transform;
            floorObj.name = "Floor_" + floorCount++;

            var floorScript = floorObj.AddComponent<BuildingFloorRender>();
            floorScript.Floor = floor;
            floorScript.Render(buildingsRender);
        }
    }
}
