using UnityEngine;
using System.Collections;

public interface IBuildingRender
{
    void Init(GameMap map, Building building);
    void Render(BuildingsRender buildingsRender);
}
