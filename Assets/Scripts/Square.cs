using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square
{
    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();

    public Mesh mesh = new Mesh();

    public Square(Vector3 position, bool flip)
    {
        var heightMap = HeightMap.heightMap;
        var pos1 = position + new Vector3(0, 0, 0) + new Vector3(0, heightMap[(int)position.x, (int)position.z], 0);
        var pos2 = position + new Vector3(0, 0, 1) + new Vector3(0, heightMap[(int)position.x, (int)position.z + 1], 0);
        var pos3 = position + new Vector3(1, 0, 1) + new Vector3(0, heightMap[(int)position.x + 1, (int)position.z + 1], 0);
        var pos4 = position + new Vector3(1, 0, 0) + new Vector3(0, heightMap[(int)position.x + 1, (int)position.z], 0);
        var pos5 = Vector3.zero;
        var pos6 = Vector3.zero;
        if (flip)
        {
            pos5 = position + new Vector3(0, 0, 1) + new Vector3(0, heightMap[(int)position.x, (int)position.z + 1], 0);
            pos6 = position + new Vector3(1, 0, 0) + new Vector3(0, heightMap[(int)position.x + 1, (int)position.z], 0);
        }
        else
        {
            pos5 = position + new Vector3(0, 0, 0) + new Vector3(0, heightMap[(int)position.x, (int)position.z], 0);
            pos6 = position + new Vector3(1, 0, 1) + new Vector3(0, heightMap[(int)position.x + 1, (int)position.z + 1], 0);
        }

        verts.Add(pos1);
        verts.Add(pos2);
        verts.Add(pos3);
        verts.Add(pos4);
        verts.Add(pos5);
        verts.Add(pos6);

        if (flip)
        {
            tris.Add(0);
            tris.Add(1);
            tris.Add(3);
            tris.Add(5);
            tris.Add(4);
            tris.Add(2);
        }
        else
        {
            tris.Add(0);
            tris.Add(1);
            tris.Add(2);
            tris.Add(5);
            tris.Add(3);
            tris.Add(4);
        }
        

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
    }
}
