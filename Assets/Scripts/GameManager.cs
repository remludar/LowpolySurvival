using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    Material material;

	void Start ()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = Resources.Load("Materials/Grass") as Material;

        _CreateAlternatingSeparateVertexMesh();
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
}
