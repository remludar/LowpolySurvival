using UnityEngine;
using System.Collections;

public static class HeightMap
{
    public static float[,] heightMap;

    public static void CreateHeightMap(int width, int depth)
    {
        heightMap = new float[width + 1, depth + 1];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                heightMap[x, z] = Random.Range(0.0f, 0.2f);
            }
        }


    }
}