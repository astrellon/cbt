using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowPolyTerrainTileRender : MonoBehaviour 
{
    public const float Size = 8.0f;
    public static float HalfSize = Size * 0.5f;
    public static float TriSize = Mathf.Sqrt(3.0f) * Size;
    public static float TriHalfSize = TriSize * 0.5f;
    public static float TriHeight = 1.5f * Size;

    private List<Vector3> verticies;
    private List<Vector2> uvs;
    private List<int> triangles;
    private LowPolyTerrainData terrainData;

	// Use this for initialization
	void Start () 
    {
	}

    public void CreateMesh(LowPolyTerrainData terrainData, string tileType, Material material)
    {
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.material = material;

        verticies = new List<Vector3>();
        uvs = new List<Vector2>();
        triangles = new List<int>();
        this.terrainData = terrainData;

        var width = terrainData.Width - 2;
        var height = terrainData.Height - 2;

        for (var y = 1; y < height; y++)
        {
            for (var x = 1; x < width; x++)
            {
                var tile = terrainData.GetTile(x, y);
                if (tile.Type == tileType)
                {
                    RenderTileTriangle3(x, y);
                }
            }
        }

        var mesh = new Mesh();
        meshFilter.mesh = mesh;

        mesh.vertices = verticies.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        meshCollider.sharedMesh = mesh;
    }

    private void RenderTile(int x, int y)
    {
        var blTile = terrainData.GetTile(x, y);
        var brTile = terrainData.GetTile(x + 1, y);
        var tlTile = terrainData.GetTile(x, y + 1);
        var trTile = terrainData.GetTile(x + 1, y + 1);

        //var middleHeight = (tlTile.Height + trTile.Height + blTile.Height + brTile.Height) * 0.25f;
        var middleHeight = VotedHeight(tlTile, trTile, blTile, brTile);

        var vi = verticies.Count;

        var xpos = x * Size;
        var ypos = y * Size;

        verticies.Add(new Vector3(xpos, tlTile.Height, ypos + Size));
        verticies.Add(new Vector3(xpos + Size, trTile.Height, ypos + Size));
        verticies.Add(new Vector3(xpos + HalfSize, middleHeight, ypos + HalfSize));

        verticies.Add(new Vector3(xpos + Size, trTile.Height, ypos + Size));
        verticies.Add(new Vector3(xpos + Size, brTile.Height, ypos));
        verticies.Add(new Vector3(xpos + HalfSize, middleHeight, ypos + HalfSize));

        verticies.Add(new Vector3(xpos + Size, brTile.Height, ypos));
        verticies.Add(new Vector3(xpos, blTile.Height, ypos));
        verticies.Add(new Vector3(xpos + HalfSize, middleHeight, ypos + HalfSize));

        verticies.Add(new Vector3(xpos, blTile.Height, ypos));
        verticies.Add(new Vector3(xpos, tlTile.Height, ypos + Size));
        verticies.Add(new Vector3(xpos + HalfSize, middleHeight, ypos + HalfSize));

        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(0.5f, 0.5f));

        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));

        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0.5f, 0.5f));

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(0.5f, 0.5f));

        for (var i = vi; i < vi + 12; i++)
        {
            triangles.Add(i);
        }
    }

    private void RenderTileTriangle3(int x, int y)
    {
        var tile = terrainData.GetTile(x, y);

        var vi = verticies.Count;

        verticies.Add(tile.Corner1); 
        verticies.Add(tile.Corner2); 
        verticies.Add(tile.Corner3); 
        
        for (var i = vi; i < vi + 3; i++)
        {
            triangles.Add(i);
        }
    }

    private void RenderTileTriangle2(int x, int y)
    {
        var isEven = ((x + y) % 2) == 0;

        var vi = verticies.Count;

        var xpos = x * TriSize * 0.5f;
        var ypos = y * TriHeight;

        if (isEven)
        {
            var tlHeight = HexHeight(x, y, x - 1, y);
            var trHeight = HexHeight(x, y, x + 1, y);
            var bHeight = HexHeight(x, y, x, y - 1);

            verticies.Add(new Vector3(xpos, tlHeight, ypos + TriHeight));
            verticies.Add(new Vector3(xpos + TriSize, trHeight, ypos + TriHeight));
            verticies.Add(new Vector3(xpos + TriHalfSize, bHeight, ypos));
        }
        else
        {
            var blHeight = HexHeight(x, y, x - 1, y - 1);
            var brHeight = HexHeight(x, y, x + 1, y - 1);
            var tHeight = HexHeight(x, y, x, y);
        
            verticies.Add(new Vector3(xpos, blHeight, ypos));
            verticies.Add(new Vector3(xpos + TriHalfSize, tHeight, ypos + TriHeight));
            verticies.Add(new Vector3(xpos + TriSize, brHeight, ypos));
        }
        
        for (var i = vi; i < vi + 3; i++)
        {
            triangles.Add(i);
        }
    }

    private void RenderTileTriangle(int x, int y)
    {
        var isEven = ((x + y) % 2) == 0;

        var vi = verticies.Count;

        var xpos = x * Size;
        var ypos = y * Size;

        if (isEven)
        {
            var tlHeight = HexHeight(x, y, x, y);
            var trHeight = HexHeight(x, y, x, y);
            var bHeight = HexHeight(x, y, x, y);

            verticies.Add(new Vector3(xpos - Size, tlHeight, ypos + HalfSize));
            verticies.Add(new Vector3(xpos + Size, trHeight, ypos + HalfSize));
            verticies.Add(new Vector3(xpos, bHeight, ypos - HalfSize));
        }
        else
        {
            var blHeight = HexHeight(x, y, x, y);
            var brHeight = HexHeight(x, y, x, y);
            var tHeight = HexHeight(x, y, x, y);
        
            verticies.Add(new Vector3(xpos - Size, blHeight, ypos - HalfSize));
            verticies.Add(new Vector3(xpos, tHeight, ypos + HalfSize));
            verticies.Add(new Vector3(xpos + Size, brHeight, ypos - HalfSize));
        }
        
        for (var i = vi; i < vi + 3; i++)
        {
            triangles.Add(i);
        }
    }

    private Vector3 AngledVector(Vector2 offset, float radian, float angleOffset, float height)
    {
        var angle = radian * Mathf.PI + angleOffset * Mathf.PI;
        return new Vector3(offset.x + Mathf.Cos(angle) * Size, height, offset.y + Mathf.Sin(angle) * Size);
    }

    private void RenderTriangle(Vector2 pos, float radian, float height1, float height2, float height3)
    {
        var vi = verticies.Count;
        
        verticies.Add(AngledVector(pos, 0, radian, height1));
        verticies.Add(AngledVector(pos, 4.0f / 3.0f, radian, height2));
        verticies.Add(AngledVector(pos, 2.0f / 3.0f, radian, height3));
        
        for (var i = vi; i < vi + 3; i++)
        {
            triangles.Add(i);
        }
    }

    float HexHeight(int x, int y, int offsetX, int offsetY)
    {
        var defaultHeight = terrainData.GetTile(x, y).HeightCm;
        
        var height1 = GetHeightOrDefault(offsetX - 1, offsetY - 1, defaultHeight); 
        var height2 = GetHeightOrDefault(offsetX, offsetY - 1, defaultHeight); 
        var height3 = GetHeightOrDefault(offsetX + 1, offsetY - 1, defaultHeight);
        var height4 = GetHeightOrDefault(offsetX - 1, offsetY, defaultHeight); 
        var height5 = GetHeightOrDefault(offsetX, offsetY, defaultHeight);
        var height6 = GetHeightOrDefault(offsetX + 1, offsetY, defaultHeight); 
        //return VotedHeightHex(height1, height2, height3, height4, height5, height6);
        return (height1 + height2 + height3 + height4 + height5 + height6) / 6.0f;
        //return defaultHeight;
    }
    
    float VotedHeightHex(params float[] tiles)
    {
        var counts = new Dictionary<float, int>();
        for (var i = 0; i < tiles.Length; i++)
        {
            counts[tiles[i]] = 0;
        }
        
        for (var i = 0; i < tiles.Length; i++)
        {
            counts[tiles[i]]++;
        }

        var result = 0.0f;
        var maxCount = 0;
        foreach (var kvp in counts)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                result = kvp.Key;
            }
        }

        if (maxCount <= 1)
        {
            result = 0.0f;
            for (var i = 0; i < tiles.Length; i++)
            {
                result += tiles[i];
            }
            result /= (float)tiles.Length;
            //result = (tl.Height + tr.Height + bl.Height + br.Height) * 0.25f;
        }

        return result;
    }
    float GetHeightOrDefault(int x, int y, float defaultHeight)
    {
        if (x < 0 || x >= terrainData.Width ||
            y < 0 || y >= terrainData.Height)
        {
            return defaultHeight;
        }
        return terrainData.GetTile(x, y).HeightCm;
    }
    
    float VotedHeight(LowPolyTerrainTile tl, LowPolyTerrainTile tr, LowPolyTerrainTile bl, LowPolyTerrainTile br)
    {
        var counts = new Dictionary<float, int>();
        counts[tl.HeightCm] = 0;
        counts[tr.HeightCm] = 0;
        counts[bl.HeightCm] = 0;
        counts[br.HeightCm] = 0;
        
        counts[tl.HeightCm]++;
        counts[tr.HeightCm]++;
        counts[bl.HeightCm]++;
        counts[br.HeightCm]++;

        var result = 0.0f;
        var maxCount = 0;
        foreach (var kvp in counts)
        {
            if (kvp.Value > maxCount)
            {
                maxCount = kvp.Value;
                result = kvp.Key;
            }
        }

        if (maxCount <= 1)
        {
            result = (tl.Height + tr.Height + bl.Height + br.Height) * 0.25f;
        }

        return result;
    }
}
