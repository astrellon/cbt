using UnityEngine;
using System.Collections;

public class BuildingRenderFactory
{
    public static IBuildingRender GetBuildingRender(GameObject obj, GameMap map, Building building)
    {
        IBuildingRender result = null;

        if (building.Type == "road")
        {
            result = obj.AddComponent<BuildingRoadRender>();
        }
        else if (building.Type == "house")
        {
            result = obj.AddComponent<BuildingHouseRender>();
        }
        else if (building.Type == "generic")
        {
            result = obj.AddComponent<BuildingGeneric>();
        }

        if (result != null)
        {
            result.Init(map, building);
        }

        return result;
    }
}
