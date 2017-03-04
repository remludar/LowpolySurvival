using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Square
{

    public int xLoc, zLoc;

    public Vector3[] vertexLocations = new Vector3[4];

    public Square(Vector3[] pos, int x, int z)
    {
        vertexLocations[0] = pos[0];
        vertexLocations[1] = pos[1];
        vertexLocations[2] = pos[2];
        vertexLocations[3] = pos[3];

        xLoc = x;
        zLoc = z;
    }


    public static Predicate<Square> ByLocation(int x, int z)
    {
        return delegate(Square square)
        {
            return square.xLoc == x && square.zLoc == z;
        };
    }
}
