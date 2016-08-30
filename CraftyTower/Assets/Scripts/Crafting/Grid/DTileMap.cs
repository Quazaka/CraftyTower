using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DTileMap
{
    int size_x;
    int size_y;

    int[,] map_data;

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

    public int GetTileAt(int x, int y)
    {
        return map_data[x, y];
    }
}
