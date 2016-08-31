using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


//Create a 2D array which hold data for each tile in the grid.
public class DTileMap
{
    public delegate void TileMap();
    public static event TileMap RebuildMesh;

    TileMap tileMap;

    int size_x;
    int size_y;

    int[,] map_data;

    public int length
    { get{return size_x;} }

    public int width
    { get { return size_y; } }

    //Create array with tile data
    public DTileMap(int size_x, int size_y)
    {
        this.size_x = size_x;
        this.size_y = size_y;

        map_data = new int[size_x, size_y];

        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                map_data[x, y] = 3;
            }
        }

        ColorMiddle();
    }

    //Make the middle four square avalible at start.
    private void ColorMiddle()
    {
        for (int x = (size_x / 2) - 1; x < (size_x / 2) + 1; x++)
        {
            for (int y = (size_y / 2) - 1; y < (size_y / 2) + 1; y++)
            {
                map_data[x, y] = 1;
            }
        }
    }

    //Get datat from tile
    public int GetTileAt(int x, int y)
    {
        return map_data[x, y];
    }

    public void SetTileAt(int x, int y, int value)
    {
        if (value < 4)
        {
            map_data[x, y] = value;

            if (RebuildMesh != null)
            {
                RebuildMesh();
            }

            return;
        }
        throw new ArgumentException("value is to large", "DTileMap.SetTileAt");
    }
}
