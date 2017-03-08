using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Square
{

    public int xLoc, zLoc;
    public Vector3[] vertexLocations = new Vector3[6];
    public bool isFlipped;

    public Square(Vector3[] pos, int x, int z, bool flipped)
    {
        vertexLocations = pos;
        xLoc = x;
        zLoc = z;
        isFlipped = flipped;
    }


    public static Predicate<Square> ByLocation(int x, int z)
    {
        return delegate(Square square)
        {
            return square.xLoc == x && square.zLoc == z;
        };
    }
}
