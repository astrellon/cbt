using UnityEngine;
using System.Collections;

public class BuildingHouseRender : MonoBehaviour, IBuildingRender 
{
    public GameMap Map;
    public Building Building;

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
        var tile = Map.TerrainData.GetTile(Building.PositionX, Building.PositionY);

        var newObj = Instantiate(prefab);
        newObj.transform.parent = transform;
        newObj.transform.localPosition = tile.Center;
    }
}
