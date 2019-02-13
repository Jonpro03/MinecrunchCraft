using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockTexture : MonoBehaviour
{

    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        List<Vector2> UVs = new List<Vector2>();
        List<Vector3> Verticies = new List<Vector3>();
        Dictionary<int, List<int>> Triangles = new Dictionary<int, List<int>>();


        // Left
        Verticies.AddRange(new List<Vector3>
        {
            new Vector3(0, 1, 1),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
        });

        UVs.AddRange(new List<Vector2>
        {
            new Vector2(0, 0.5f),
            new Vector2(0.25f, 0.5f),
            new Vector2(0.25f, 0),
            new Vector2(0.25f, 0),
            new Vector2(0, 0),
            new Vector2(0, 0.5f),
        });


        // Right
        Verticies.AddRange(new List<Vector3>
        {
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
        });

        UVs.AddRange(new List<Vector2>
        {
            new Vector2(0.5f, 0.5f),
            new Vector2(0.75f, 0.5f),
            new Vector2(0.75f, 0),
            new Vector2(0.75f, 0),
            new Vector2(0.5f, 0),
            new Vector2(0.5f, 0.5f),
        });


        // Top
        Verticies.AddRange(new List<Vector3>
        {
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1)
        });

        UVs.AddRange(new List<Vector2>
        {
            new Vector2(0.25f + 0.25f, 0.5f + 0.5f),
            new Vector2(0 + 0.25f, 0.5f + 0.5f),
            new Vector2(0 + 0.25f, 0 + 0.5f),
            new Vector2(0 + 0.25f, 0 + 0.5f),
            new Vector2(0.25f + 0.25f, 0 + 0.5f),
            new Vector2(0.25f + 0.25f, 0.5f + 0.5f),
        });

        // Bottom
        Verticies.AddRange(new List<Vector3>
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 0)
        });

        UVs.AddRange(new List<Vector2>
        {
            new Vector2(0.25f, 0.5f + 0.5f),
            new Vector2(0, 0.5f + 0.5f),
            new Vector2(0, 0 + 0.5f),
            new Vector2(0, 0 + 0.5f),
            new Vector2(0.25f, 0 + 0.5f),
            new Vector2(0.25f, 0.5f + 0.5f)
        });


        // Front
        Verticies.AddRange(new List<Vector3>
        {
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        });

        UVs.AddRange(new List<Vector2>
        {
            new Vector2(0.25f + 0.25f, 0.5f),
            new Vector2(0.25f + 0.25f, 0),
            new Vector2(0 + 0.25f, 0),
            new Vector2(0 + 0.25f, 0),
            new Vector2(0 + 0.25f, 0.5f),
            new Vector2(0.25f + 0.25f, 0.5f),
        });


        // Back
        Verticies.AddRange(new List<Vector3>
        {
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 0, 1)
        });



        UVs.AddRange(new List<Vector2>
        {
            new Vector2(0.5f, 1),
            new Vector2(0.75f, 1),
            new Vector2(0.75f, 0.5f),
            new Vector2(0.75f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 1),
        });


        

        

        

        

        

        mesh.SetVertices(Verticies);
        mesh.SetUVs(0, UVs);        

        //mesh.SetTriangles(Enumerable.Range(0, 11).ToArray(), 0);

        MeshFilter mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;
        mesh.RecalculateNormals();
    }
}

