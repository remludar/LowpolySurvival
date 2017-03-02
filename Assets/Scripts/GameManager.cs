using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
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

        //_CreateAlternatingSeparateVertexMesh();
        _CreateAlternatingSeparateVertexMeshWithJitter();
	}

    private void _CreateAlternatingSeparateVertexMesh()
    {
        int width = 33;
        int depth = 33;

        HeightMap.CreateHeightMap(width, depth);

        bool flip = false;
        var squares = new List<Square>();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                squares.Add(new Square(new Vector3(x, 0, z), flip));
                flip = !flip;
            }
        }

        CombineInstance[] combine = new CombineInstance[squares.Count];
        for (int i = 0; i < combine.Length; i++)
        {
            combine[i].mesh = squares[i].mesh;
            combine[i].transform = meshFilter.transform.localToWorldMatrix;
        }

        mesh = meshFilter.mesh;
        mesh.CombineMeshes(combine);
        mesh.RecalculateNormals();

    }

    private void _CreateAlternatingSeparateVertexMeshWithJitter()
    {
        int width = 65; 
        int depth = 65;


        HeightMap.CreateHeightMap(width, depth);
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


                Vector3[] vertexPositions = new Vector3[4];

                if (x == 0 && z == 0) // very first square
                {

                    var newX0 = x + jitterX0;
                    var newX1 = x + jitterX1;
                    var newX2 = x + jitterX2;
                    if (newX2 > x + 1) newX2 = x + 1;
                    var newX3 = x + jitterX3;
                    if (newX3 > x + 1) newX3 = x + 1;

                    vertexPositions[0] = new Vector3(newX0, Random.Range(0.0f, 0.4f), z);
                    vertexPositions[1] = new Vector3(newX1, Random.Range(0.0f, 0.4f), z + 1);
                    vertexPositions[2] = new Vector3(newX2, Random.Range(0.0f, 0.4f), z +1);
                    vertexPositions[3] = new Vector3(newX3, Random.Range(0.0f, 0.4f), z);
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
                    var newY1 = Random.Range(0.0f, 0.4f);
                    var newY2 = Random.Range(0.0f, 0.4f);
                    var newY3 = previousSquare.vertexLocations[2].y;

                    vertexPositions[0] = new Vector3(newX0, newY0, z);
                    vertexPositions[1] = new Vector3(newX1, newY1, z + 1);
                    vertexPositions[2] = new Vector3(newX2, newY2, z + 1);
                    vertexPositions[3] = new Vector3(newX3, newY3, z);
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
                    var newY2 = Random.Range(0.0f, 0.4f);
                    var newY3 = Random.Range(0.0f, 0.4f);

                    vertexPositions[0] = new Vector3(newX0, newY0, z);
                    vertexPositions[1] = new Vector3(newX1, newY1, z + 1);
                    vertexPositions[2] = new Vector3(newX2, newY2, z + 1);
                    vertexPositions[3] = new Vector3(newX3, newY3, z);
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
                    var newY2 = Random.Range(0.0f, 0.4f);
                    var newY3 = previousSquare0.vertexLocations[2].y;

                    vertexPositions[0] = new Vector3(newX0, newY0, z);
                    vertexPositions[1] = new Vector3(newX1, newY1, z + 1);
                    vertexPositions[2] = new Vector3(newX2, newY2, z + 1);
                    vertexPositions[3] = new Vector3(newX3, newY3, z);
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
}
