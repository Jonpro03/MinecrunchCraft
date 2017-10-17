using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;
using System.Linq;
using System;

public class TerrainGen : MonoBehaviour
{
    public GameObject OakTree;

    //Public variable for the size of the terrain, width and heigth
    public int RenderSize = 5;

    public string Seed = "";

    private List<Chunk> Chunks { get; set; }

    private List<Chunk> ChunkGenerateJobs { get; set; }

    private List<Chunk> ChunkRenderJobs { get; set; }

    //Generates the terrain
    void Start()
    {
        // If the seed is empty, generate a random seed.
        Seed = DateTime.Now.ToString("h:ss");

        Chunks = new List<Chunk>();
        ChunkGenerateJobs = new List<Chunk>();
        ChunkRenderJobs = new List<Chunk>();

        // Create the chunks
        for (int cx = (-1 * RenderSize) + (int) ChunkCoordPlayerIsIn().x; cx < RenderSize; cx++)
        {
            for (int cz = (-1 * RenderSize) + (int)ChunkCoordPlayerIsIn().y; cz < RenderSize; cz++)
            {
                GenerateChunk(new Vector2(cx, cz));
            } 
        }
        InvokeRepeating("UpdateChunks", 1, 2);
    }

    private void Update()
    {
        foreach (Chunk chunkJob in ChunkRenderJobs)
        {
            DrawChunk(chunkJob);
        }
        ChunkRenderJobs = new List<Chunk>();

        foreach (Chunk chunkJob in ChunkGenerateJobs)
        {
            if (chunkJob.IsDone)
            {
                ChunkRenderJobs.Add(chunkJob);
            }
        }
        ChunkGenerateJobs.RemoveAll(c => c.IsDone);
    }

    public void UpdateChunks()
    {
        const int UpdateDistance = 2;
        var playerPos = ChunkCoordPlayerIsIn();
        List<Vector2> neededChunks = new List<Vector2>();

        for (var a = 0; a <= UpdateDistance; a++)
        {
            for (var b = 0; b <= UpdateDistance; b++)
            {
                neededChunks.Add(playerPos + (Vector2.up * a) + (Vector2.right * b));
                neededChunks.Add(playerPos + (Vector2.up * a) + (Vector2.left * b));
                neededChunks.Add(playerPos + (Vector2.down * a) + (Vector2.right * b));
                neededChunks.Add(playerPos + (Vector2.down * a) + (Vector2.left * b));
            }
        }

        foreach (var chunkCoord in neededChunks)
        {
            GenerateChunk(chunkCoord);     
        }
    }

    private void GenerateChunk(Vector2 chunkPos)
    {
        if (ChunkExists(chunkPos) && GetChunk(chunkPos).Generated)
        {
            return;
        }
        
        Chunk chunk = new Chunk(chunkPos, Seed);
        
        chunk.Start();
        ChunkGenerateJobs.Add(chunk);
    }

    private void DrawChunk(Chunk chunk)
    {
        GameObject chunkGameObject = new GameObject(string.Format("chunk{0}", chunk.ChunkPosition.ToString()));
        chunkGameObject.transform.position = new Vector3(chunk.ChunkPosition.x * 16, 0, chunk.ChunkPosition.y * 16);

        Material[] mats = new Material[chunk.Materials.Count];

        foreach (int key in chunk.Materials.Keys)
        {
            mats[key] = Resources.Load<Material>(chunk.Materials[key]);
        }

        var meshRenderer = chunkGameObject.AddComponent<MeshRenderer>();
        meshRenderer.materials = mats;

        var chunkMesh = new Mesh();
        chunkMesh.Clear();
        chunkMesh.name = "TerrainMesh";
        chunkMesh.SetVertices(chunk.Verticies.ToList());
        chunkMesh.subMeshCount = chunk.Materials.Count;

        chunkMesh.SetUVs(0, chunk.UVs.ToList());

        foreach (int key in chunk.Triangles.Keys)
        {
            chunkMesh.SetTriangles(chunk.Triangles[key].ToList(), key);
        }

        var meshFilter = chunkGameObject.AddComponent<MeshFilter>();


        meshFilter.mesh = chunkMesh;
        chunkMesh.RecalculateNormals();
        var collider = chunkGameObject.AddComponent<MeshCollider>();
        Chunks.Add(chunk);
    }

    private void UpdateBlockFaces(List<IBlock> blocks)
    {
        // Now that we have our chunk filled in, we need to figure out which block faces are visible.
        foreach (IBlock block in blocks)
        {
            int bx = (int)block.PositionInChunk.x;
            int by = (int)block.PositionInChunk.y;
            int bz = (int)block.PositionInChunk.z;

            var chunk = GetChunk(Vector2.zero);

            // Check top, bottom, front, back, left and right to see if there's another visible block there.

            for (int columnY = by; columnY > 0; columnY--)
            {
                // Left
                if (null == chunk.Blocks[Mathf.Max(bx - 1, 0), columnY, bz])
                {
                    chunk.Blocks[bx, columnY, bz].LeftVisible = true;
                }
                // Right
                if (null == chunk.Blocks[Mathf.Min(bx + 1, 15), columnY, bz])
                {
                    chunk.Blocks[bx, columnY, bz].RightVisible = true;
                }
                // Front
                if (null == chunk.Blocks[bx, columnY, Mathf.Max(bz - 1, 0)])
                {
                    chunk.Blocks[bx, columnY, bz].FrontVisible = true;
                }
                // Back
                if (null == chunk.Blocks[bx, columnY, Mathf.Min(bz + 1, 15)])
                {
                    chunk.Blocks[bx, columnY, bz].BackVisible = true;
                }
            }
        }
    }


    private Chunk GetChunk(Vector2 chunkLoc)
    {
        return Chunks.FirstOrDefault(x => x.ChunkPosition == chunkLoc);
    }

    private bool ChunkExists(Vector2 chunkLoc)
    {
        return Chunks.FirstOrDefault(x => x.ChunkPosition == chunkLoc) != null;
    }

    private Vector2 ChunkCoordPlayerIsIn()
    {
        return new Vector2((int)(transform.position.x / 16.0f), (int)(transform.position.z / 16.0f));
    }
}