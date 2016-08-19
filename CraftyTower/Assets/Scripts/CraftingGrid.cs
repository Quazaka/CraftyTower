using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class CraftingGrid : MonoBehaviour {

    private Vector3[] vertices;
    private Mesh mesh;

    public int xSize, ySize;

    private void Awake()
    {
        GenerateGrid();
    }
    
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void GenerateGrid()
    {     
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Crafting Grid";

        // initaliaze number of vertices
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        // Create all vertices using a double for-loop
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);                
            }
        }
        mesh.vertices = vertices;

        StartCoroutine(GenerateTriangles());
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

    private IEnumerator GenerateTriangles()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        int[] triangles = new int[xSize * ySize * 6];

        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                // waiting for visulization purpose
                mesh.triangles = triangles;
                yield return wait;
            }            
        }

        //triangles[0] = 0;
        //triangles[1] = xSize + 1;
        //triangles[2] = 1;
        //triangles[3] = 1;
        //triangles[4] = xSize + 1;
        //triangles[5] = xSize + 2;

        
    }
}
