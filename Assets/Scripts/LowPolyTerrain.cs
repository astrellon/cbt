using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LowPolyTerrain : MonoBehaviour 
{
    public int Width = 50;
    public int Height = 50;
    public float Depth = 32.0f;

    public int NumSectionsX = 1;
    public int NumSectionsY = 1;
    
    public MaterialPair[] Materials;

    public GameObject TreePrefab;

    private int currentWidth = -1;
    private int currentHeight = -1;
    private float currentDepth = -1;
    private int currentSectionsX = -1;
    private int currentSectionsY = -1;
    private GameObject currentTreePrefab = null;

	// Use this for initialization
	void Start () 
    {
        if (Application.isPlaying)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        CreateMesh();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!Application.isPlaying)
        {
            if (!HasChanged())
            {
                return;
            }
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            CreateMesh();
            UpdateCurrents();
        }
	}

    bool HasChanged()
    {
        return currentWidth != Width ||
            currentHeight != Height ||
            currentDepth != Depth ||
            currentSectionsX != NumSectionsX ||
            currentSectionsY != NumSectionsY ||
            currentTreePrefab != TreePrefab;
    }
    void UpdateCurrents()
    {
        currentWidth = Width;
        currentHeight = Height;
        currentDepth = Depth;
        currentSectionsX = NumSectionsX;
        currentSectionsY = NumSectionsY;
        currentTreePrefab = TreePrefab;
    }

    void CreateMesh()
    {
        for (var y = 0; y < NumSectionsY; y++)
        {
            for (var x = 0; x < NumSectionsX; x++)
            {
                var sectionObj = new GameObject();
                var section = sectionObj.AddComponent<LowPolyTerrainSection>();

                section.Width = Width;
                section.Height = Height;
                section.Depth = Depth;
                section.OffsetX = x * (Width - 1);
                section.OffsetY = y * (Height - 1);
                section.Materials = Materials;
                section.TreePrefab = TreePrefab;

                section.transform.parent = transform;
                section.name = "Section_" + x + "_" + y;
            }
        }
    }
}
