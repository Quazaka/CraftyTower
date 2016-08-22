using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CraftingGrid : MonoBehaviour {

    public LayerMask layerMask;

    private Vector3[] vertices;
    private Mesh mesh;
    private MeshCollider meshCol;
    private Renderer render;

    public int xSize, ySize;
    public Color highlightColor;
    private Color normalColor;

    private void Awake()
    {
        GenerateGrid();
    }
    
    // Use this for initialization
    void Start ()
    {
        render = GetComponent<Renderer>();
        normalColor = render.material.GetColor("_TintColor");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            RaycastHit selectedGridTile;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (meshCol.Raycast(ray, out selectedGridTile, layerMask))
            {
                ChangeTileColor(selectedGridTile.triangleIndex, highlightColor);
            }
            else
            {
                ChangeTileColor(selectedGridTile.triangleIndex, normalColor);
            }
        }
    }

    private void GenerateGrid()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.name = "Crafting Grid";

        GenerateVertices();
        GenerateTriangles();
        mesh.RecalculateNormals();

        meshCol = GetComponent<MeshCollider>();
        meshCol.sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }      
    }

    // Generates the vertices (and UVs) for our grid
    private void GenerateVertices()
    {
        // initaliaze number of vertices
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        // UV-coordinates - one for each vertices
        Vector2[] uv = new Vector2[vertices.Length];

        // Create all vertices and UV-coordinates using a double for-loop
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
    }

    private void GenerateTriangles()
    {
        // each triangle is 3 points and each quad is 6 points
        // therefore we need an array 6 times the size of our grid
        int[] triangles = new int[xSize * ySize * 6];

        // ti = triangle index, vi = vertice index
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            // we increment the ti by 6 because we create two triangles( 6 points) for each loop trough
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;

                // triangles ti + 2/3 and ti + 1/4 are the shared points for triangles and therefore the same value
            }            
        }
        mesh.triangles = triangles;
    }

    // fucking fix this bullshit
    // just finds four next vertices - to the right, doesn't find the four that connects the selected quad/triangle
    private void ChangeTileColor(int triIndex, Color toColor)
    {        
        Color[] colors = new Color[vertices.Length];        
        int vertIndex = mesh.triangles[triIndex * 3];
        Debug.Log("triIndex " + triIndex + " vertIndex: " + vertIndex); // this is correct

        // this shit isn't
        for (int i = vertIndex; i < vertIndex + 4; i++)
        {
            //Debug.Log(" i " + i + " " + mesh.triangles[i]);
            colors[mesh.triangles[i]] = toColor;
        }
        mesh.colors = colors;
    }

    // These work too, but i don't know what code to write here.
    //void OnMouseEnter()
    //{
    //    render.material.SetColor("_TintColor", highlightColor);
    //    Debug.Log("Enter");
    //}
    //void OnMouseExit()
    //{
    //    render.material.SetColor("_TintColor", normalColor);
    //    Debug.Log("Exit");
    //}
}
