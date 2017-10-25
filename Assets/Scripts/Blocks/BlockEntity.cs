using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Blocks;
using Assets.Scripts.Interfaces;
using UnityEngine;

public class BlockEntity : MonoBehaviour, IEntity
{

    public Block Block { get; set; }

    // Use this for initialization
    void Start()
    {
        if (null != Block)
        {
            Draw();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Draw()
    {
        GameObject go = transform.gameObject;
        //transform.position = Block.PositionInWorld;
        Material[] mats = new Material[1];
        mats[0] = Resources.Load<Material>(Block.Texture);

        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.materials = mats;

        Mesh mesh = new Mesh();
        mesh.name = "Mesh for " + Block.PositionInWorld.ToString();
        mesh.SetVertices(Block.Verticies);
        mesh.SetUVs(0, Block.UVs);
        List<int> triangles = Enumerable.Range(0, Block.Verticies.Count).ToList();
        mesh.SetTriangles(triangles, 0);

        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        mesh.RecalculateNormals();
        go.AddComponent<MeshCollider>();
    }
}
