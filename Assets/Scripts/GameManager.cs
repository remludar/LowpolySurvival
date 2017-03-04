﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float scale;
    public float sampleX;
    public float sampleZ;
    public float xLoc;
    public float zLoc;

    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Square> squares = new List<Square>();

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    Material material;

	void Start ()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = Resources.Load("Materials/Grass") as Material;
        mesh = meshFilter.mesh;

        scale = 3.5f;
        sampleX = 5.9f;
        sampleZ = 4.1f;
        xLoc = 0;
        zLoc = 0;

        _CreateAlternatingSeparateVertexMeshNoJitterWithPerlin();
	}

    void Update()
    {
        mesh.Clear();
        verts.Clear();
        tris.Clear();

        _CreateAlternatingSeparateVertexMeshNoJitterWithPerlin();

        xLoc += 0.05f;
        zLoc += 0.05f;
    }


    private void _CreateAlternatingSeparateVertexMeshWithJitter()
    {
        int width = 32; 
        int depth = 32;

        //HeightMap.CreateHeightMap(width, depth);
        bool flip = false;
        int triCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float jitterX0 = Random.Range(0.0f, 0.5f);
                float jitterX1 = Random.Range(0.0f, 0.5f);
                float jitterX2 = Random.Range(0.5f, 1.0f);
                float jitterX3 = Random.Range(0.5f, 1.0f);

                float jitterZ0 = Random.Range(0.0f, 0.5f);
                float jitterZ1 = Random.Range(0.5f, 1.0f);
                float jitterZ2 = Random.Range(0.5f, 1.0f);
                float jitterZ3 = Random.Range(0.0f, 0.5f);

                float perlinNoise = Mathf.PerlinNoise(((float)(x + xLoc) / (float)width) * sampleX, ((float)(z + zLoc) / (float)depth) * sampleZ) * scale;

                Vector3[] vertexPositions = new Vector3[4];

                if (x == 0 && z == 0) // very first square
                {

                    var newX0 = x + jitterX0;
                    var newX1 = x + jitterX1;
                    var newX2 = x + jitterX2;
                    if (newX2 > x + 1) newX2 = x + 1;
                    var newX3 = x + jitterX3;
                    if (newX3 > x + 1) newX3 = x + 1;

                    var newZ0 = z + jitterZ0;
                    var newZ1 = z + jitterZ1;
                    if (newZ1 > z + 1) newZ1 = z + 1;
                    var newZ2 = z + jitterZ2;
                    if (newZ2 > z + 1) newZ2 = z + 1;
                    var newZ3 = z + jitterZ3;

                    vertexPositions[0] = new Vector3(newX0, perlinNoise, newZ0);
                    vertexPositions[1] = new Vector3(newX1, perlinNoise, newZ1);
                    vertexPositions[2] = new Vector3(newX2, perlinNoise, newZ2);
                    vertexPositions[3] = new Vector3(newX3, perlinNoise, newZ3);
                }
                else if(x == 0) // squares on first column
                {
                    var previousSquare = squares[squares.Count - 1];
                    var newX0 = previousSquare.vertexLocations[1].x;
                    var newX1 = (Random.Range(0.0f, 1.0f) < 0.5) ? newX0 + jitterX1 : newX0 - jitterX1;
                    if (newX1 > x + 0.5f) newX1 = x + 0.5f;
                    if (newX1 < x) newX1 = x;

                    var newX2 = x + jitterX2;
                    if (newX2 > x + 1) newX2 = x + 1;
                    var newX3 = previousSquare.vertexLocations[2].x;

                    var newY0 = previousSquare.vertexLocations[1].y;
                    var newY1 = perlinNoise;
                    var newY2 = perlinNoise;
                    var newY3 = previousSquare.vertexLocations[2].y;

                    var newZ0 = previousSquare.vertexLocations[1].z;
                    var newZ1 = (Random.Range(0.0f, 1.0f) < 0.5) ? newZ0 + jitterZ1 : newZ0 - jitterZ1;
                    if (newZ1 > z + 0.5f) newZ1 = z + 0.5f;
                    if (newZ1 < z) newZ1 = z;
                    var newZ2 = z + jitterZ2;
                    if (newZ2 > z + 1) newZ2 = z + 1;
                    var newZ3 = previousSquare.vertexLocations[2].z;
                    

                    vertexPositions[0] = new Vector3(newX0, newY0, newZ0);
                    vertexPositions[1] = new Vector3(newX1, newY1, newZ1);
                    vertexPositions[2] = new Vector3(newX2, newY2, newZ2);
                    vertexPositions[3] = new Vector3(newX3, newY3, newZ3);
                }
                else if(z == 0) //squares on first row
                {
                    var previousSquare = squares.Find(Square.ByLocation(x - 1, z));
                    var newX0 = previousSquare.vertexLocations[3].x;
                    var newX1 = previousSquare.vertexLocations[2].x;
                    var newX2 = x + jitterX2;
                    var newX3 = x + jitterX3;

                    var newY0 = previousSquare.vertexLocations[3].y;
                    var newY1 = previousSquare.vertexLocations[2].y;
                    var newY2 = perlinNoise;
                    var newY3 = perlinNoise;

                    var newZ0 = previousSquare.vertexLocations[3].z;
                    var newZ1 = previousSquare.vertexLocations[2].z;
                    var newZ2 = z + jitterZ2;
                    var newZ3 = z + jitterZ3;

                    vertexPositions[0] = new Vector3(newX0, newY0, newZ0);
                    vertexPositions[1] = new Vector3(newX1, newY1, newZ1);
                    vertexPositions[2] = new Vector3(newX2, newY2, newZ2);
                    vertexPositions[3] = new Vector3(newX3, newY3, newZ3);
                }
                else // all other squares
                {
                    var previousSquare0 = squares[squares.Count - 1];
                    var previousSquare1 = squares.Find(Square.ByLocation(x-1, z));
                    var newX0 = previousSquare0.vertexLocations[1].x;
                    var newX1 = previousSquare1.vertexLocations[2].x;
                    var newX2 = x + jitterX2;
                    var newX3 = previousSquare0.vertexLocations[2].x;

                    var newY0 = previousSquare0.vertexLocations[1].y;
                    var newY1 = previousSquare1.vertexLocations[2].y;
                    var newY2 = perlinNoise;
                    var newY3 = previousSquare0.vertexLocations[2].y;

                    var newZ0 = previousSquare0.vertexLocations[1].z;
                    var newZ1 = previousSquare1.vertexLocations[2].z;
                    var newZ2 = z + jitterZ2;
                    var newZ3 = previousSquare0.vertexLocations[2].z;

                    vertexPositions[0] = new Vector3(newX0, newY0, newZ0);
                    vertexPositions[1] = new Vector3(newX1, newY1, newZ1);
                    vertexPositions[2] = new Vector3(newX2, newY2, newZ2);
                    vertexPositions[3] = new Vector3(newX3, newY3, newZ3);
                }

                var square = new Square(vertexPositions, x, z);
                squares.Add(square);

                verts.Add(vertexPositions[0]);
                verts.Add(vertexPositions[1]);
                verts.Add(vertexPositions[2]);
                verts.Add(vertexPositions[3]);

                if (flip)
                {
                    verts.Add(vertexPositions[1]);
                    verts.Add(vertexPositions[3]);
                }
                else
                {
                    verts.Add(vertexPositions[0]);
                    verts.Add(vertexPositions[2]);
                }

                if (flip)
                {
                    tris.Add(triCount * 6 + 0);
                    tris.Add(triCount * 6 + 1);
                    tris.Add(triCount * 6 + 3);
                    tris.Add(triCount * 6 + 5);
                    tris.Add(triCount * 6 + 4);
                    tris.Add(triCount * 6 + 2);
                }
                else
                {
                    tris.Add(triCount * 6 + 0);
                    tris.Add(triCount * 6 + 1);
                    tris.Add(triCount * 6 + 2);
                    tris.Add(triCount * 6 + 5);
                    tris.Add(triCount * 6 + 3);
                    tris.Add(triCount * 6 + 4);
                }
                triCount++;
                flip = !flip;
            }
        }

        mesh = meshFilter.mesh;
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
    }

    private void _CreateAlternatingSeparateVertexMeshNoJitterWithPerlin()
    {
        int width = 32;
        int depth = 32;

        bool flip = false;
        int triCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float perlinNoise =  Mathf.PerlinNoise(((float)(x + xLoc) / (float)width) * sampleX, ((float)(z + zLoc) / (float)depth) * sampleZ) * scale;

                Vector3[] vertexPositions = new Vector3[4];

                var newX0 = x;
                var newX1 = x;
                var newX2 = x + 1;
                var newX3 = x + 1;

                var newZ0 = z;
                var newZ1 = z + 1;
                var newZ2 = z + 1;
                var newZ3 = z;

                if (x == 0 && z == 0) // very first square
                {
                    vertexPositions[0] = new Vector3(newX0, perlinNoise, newZ0);
                    vertexPositions[1] = new Vector3(newX1, perlinNoise, newZ1);
                    vertexPositions[2] = new Vector3(newX2, perlinNoise, newZ2);
                    vertexPositions[3] = new Vector3(newX3, perlinNoise, newZ3);
                }
                else if (x == 0) // squares on first column
                {
                    var previousSquare = squares[squares.Count - 1];

                    var newY0 = previousSquare.vertexLocations[1].y;
                    var newY1 = perlinNoise;
                    var newY2 = perlinNoise;
                    var newY3 = previousSquare.vertexLocations[2].y;

                    vertexPositions[0] = new Vector3(newX0, newY0, newZ0);
                    vertexPositions[1] = new Vector3(newX1, newY1, newZ1);
                    vertexPositions[2] = new Vector3(newX2, newY2, newZ2);
                    vertexPositions[3] = new Vector3(newX3, newY3, newZ3);
                }
                else if (z == 0) //squares on first row
                {
                    var previousSquare = squares.Find(Square.ByLocation(x - 1, z));

                    var newY0 = previousSquare.vertexLocations[3].y;
                    var newY1 = previousSquare.vertexLocations[2].y;
                    var newY2 = perlinNoise;
                    var newY3 = perlinNoise;

                    vertexPositions[0] = new Vector3(newX0, newY0, newZ0);
                    vertexPositions[1] = new Vector3(newX1, newY1, newZ1);
                    vertexPositions[2] = new Vector3(newX2, newY2, newZ2);
                    vertexPositions[3] = new Vector3(newX3, newY3, newZ3);
                }
                else // all other squares
                {
                    var previousSquare0 = squares[squares.Count - 1];
                    var previousSquare1 = squares.Find(Square.ByLocation(x - 1, z));

                    var newY0 = previousSquare0.vertexLocations[1].y;
                    var newY1 = previousSquare1.vertexLocations[2].y;
                    var newY2 = perlinNoise;
                    var newY3 = previousSquare0.vertexLocations[2].y;

                    vertexPositions[0] = new Vector3(newX0, newY0, newZ0);
                    vertexPositions[1] = new Vector3(newX1, newY1, newZ1);
                    vertexPositions[2] = new Vector3(newX2, newY2, newZ2);
                    vertexPositions[3] = new Vector3(newX3, newY3, newZ3);
                }
                var square = new Square(vertexPositions, x, z);
                squares.Add(square);

                verts.Add(vertexPositions[0]);
                verts.Add(vertexPositions[1]);
                verts.Add(vertexPositions[2]);
                verts.Add(vertexPositions[3]);

                if (flip)
                {
                    verts.Add(vertexPositions[1]);
                    verts.Add(vertexPositions[3]);
                }
                else
                {
                    verts.Add(vertexPositions[0]);
                    verts.Add(vertexPositions[2]);
                }

                if (flip)
                {
                    tris.Add(triCount * 6 + 0);
                    tris.Add(triCount * 6 + 1);
                    tris.Add(triCount * 6 + 3);
                    tris.Add(triCount * 6 + 5);
                    tris.Add(triCount * 6 + 4);
                    tris.Add(triCount * 6 + 2);
                }
                else
                {
                    tris.Add(triCount * 6 + 0);
                    tris.Add(triCount * 6 + 1);
                    tris.Add(triCount * 6 + 2);
                    tris.Add(triCount * 6 + 5);
                    tris.Add(triCount * 6 + 3);
                    tris.Add(triCount * 6 + 4);
                }
                triCount++;
                flip = !flip;
            }
        }

        squares.Clear();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
    }

}
