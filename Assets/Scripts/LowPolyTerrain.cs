using UnityEngine;
using System.Collections;

public class LowPolyTerrain : MonoBehaviour 
{
    public int Width = 50;
    public int Height = 50;
    public float Depth = 32.0f;

    public int NumSectionsX = 1;
    public int NumSectionsY = 1;
    
    public MaterialPair[] Materials;

	// Use this for initialization
	void Start () 
    {
        for (var y = 0; y < NumSectionsY; y++)
        {
            for (var x = 0; x < NumSectionsX; x++)
            {
                var sectionObj = new GameObject();
                var section = sectionObj.AddComponent<LowPolyTerrainSection>() as LowPolyTerrainSection;

                section.Width = Width;
                section.Height = Height;
                section.Depth = Depth;
                section.OffsetX = x * (Width - 1);
                section.OffsetY = y * (Height - 1);
                section.Materials = Materials;

                section.transform.parent = transform;
                section.name = "Section_" + x + "_" + y;
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
