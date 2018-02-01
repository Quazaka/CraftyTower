using System;
using UnityEngine;
using System.Collections;
using CraftyTower.Crafting;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class CraftingGrid : MonoBehaviour {

    private static CraftingGrid craftingGrid;

    private Mesh mesh;

    public Texture2D tileTextures;
    private Texture2D gridTexture;
    private Color[][] tiles;
    private int tileResolution = 16; // 16 x 16 pixels

    private int[,] grid;
    const int _length = 10; // x
    const int _width = 10; // z

    private bool firstGenerate;

    void Awake()
    {
        if (craftingGrid == null)
        {
            craftingGrid = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
        craftingGrid.enabled = true;
    }

    //Generate Mesh and its texture
    void Start () {
                
        firstGenerate = true;
        GenerateMesh();     
        GenerateTexture();
    }

    #region Mesh Generating
    private void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Crafting Grid";

        CalculateVertsAndUVs();
        CalculateTriangles();
        mesh.RecalculateNormals();

        // Assign our mesh to our filter/collider      
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void CalculateVertsAndUVs()
    {
        Vector3[] vertices = new Vector3[(_length + 1) * (_width + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= _width; z++)
        {
            for (int x = 0; x <= _length; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, z);
                uv[i] = new Vector2((float)x / _length, (float)z / _width);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
    }

    private void CalculateTriangles()
    {
        // Each quad consist of two triangles with three points - therefore we multiply by 6
        int[] triangles = new int[_length * _width * 6];

        for (int triIndex = 0, vertIndex = 0, z = 0; z < _width; z++, vertIndex++)
        {
            for (int x = 0; x < _length; x++, triIndex += 6, vertIndex++)
            {
                triangles[triIndex] = vertIndex;
                triangles[triIndex + 3] = triangles[triIndex + 2] = vertIndex + 1;
                triangles[triIndex + 4] = triangles[triIndex + 1] = vertIndex + _length + 1;
                triangles[triIndex + 5] = vertIndex + _length + 2;
            }
        }
        mesh.triangles = triangles;
    }
    #endregion

    #region Grid Texturing
    private void GenerateTexture()
    {
        if (firstGenerate)
        {
            // Setting up the texture for the grid on the first generation
            gridTexture = (Texture2D)GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
            gridTexture.filterMode = FilterMode.Point;
            gridTexture.wrapMode = TextureWrapMode.Clamp;

            ChopUpTileTextures();
            SetInitialGridTileTpes();
            firstGenerate = false;
        }

        for (int y = 0; y < _width; y++)
        {
            for (int x = 0; x < _length; x++)
            {
                Color[] p = tiles[grid[x, y]];
                gridTexture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
            }
        }
        gridTexture.Apply();
    }

    //Chop up the textures for the tiles so they fit on the grid
    private void ChopUpTileTextures()
    {
        int numTilesPerRow = (tileTextures.width / tileResolution);
        int numRows = (tileTextures.height / tileResolution);

        tiles = new Color[numTilesPerRow * numRows][];

        for (int i = 0, y = 0; y < numRows; y++)
        {
            for (int x = 0; x < numTilesPerRow; x++, i++)
            {
                tiles[i] = tileTextures.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
            }
        }
    }

    // First setup for our grid
    private void SetInitialGridTileTpes()
    {
        grid = new int[_length, _width];

        for (int x = 0; x < _length; x++)
        {
            for (int y = 0; y < _width; y++)
            {
                grid[x, y] = (int)TileType.Locked;
            }
        }

        // Set 4 tiles in the middle of the grid to be free
        grid[4, 4] = (int)TileType.Free;
        grid[4, 5] = (int)TileType.Free;
        grid[5, 4] = (int)TileType.Free;
        grid[5, 5] = (int)TileType.Free;
    }     
    #endregion

    #region Helper Methods
    public int GetTileTypeAt(Vector3 selectorPos)
    {
        return grid[Mathf.FloorToInt(selectorPos.x), Mathf.FloorToInt(selectorPos.z)];
    }

    public void SetTileTypeAt(Vector3 selectorPos, TileType tileType)
    {
        if (tileType != TileType.Error)
        {
            grid[Mathf.FloorToInt(selectorPos.x), Mathf.FloorToInt(selectorPos.z)] = (int)tileType;

            GenerateTexture();
            return;
        }
        throw new ArgumentException("Tile type does not exist");
    }
    #endregion
}
