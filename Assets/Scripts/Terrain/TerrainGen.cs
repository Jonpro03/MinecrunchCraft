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

    //Height multiplies the final noise output
    public float HeightOfTerrain = 10.0f;

    public Material BaseMaterial;

    public string Seed = "";

    //This divides the noise frequency
    //Higher is Flatter
    public float NoiseSize = 10.0f;

    private List<Chunk> Chunks { get; set; }


    //Function that inputs the position and spits out a float value based on the perlin noise
    public int PerlinNoise(float x, float z)
    {
        //Generate a value from the given position, position is divided to make the noise more frequent.
        float pass1 = Mathf.PerlinNoise(((x + Seed.GetHashCode()) / NoiseSize), ((z + Seed.GetHashCode()) / NoiseSize)) * HeightOfTerrain;
        float pass2 = Mathf.Pow(Mathf.PerlinNoise(((x + Seed.GetHashCode()) / 1337), ((z + Seed.GetHashCode()) / 1337)) * HeightOfTerrain, 2.1337f);

        //Return the noise value
        return (int)(pass2 - pass1);
    }

    //Generates the terrain
    void Start()
    {
        // If the seed is empty, generate a random seed.
        Seed = DateTime.Now.ToString("h:ss");

        //BaseMaterial = Resources.Load<Material>("Materials/DirtBlock");
        Chunks = new List<Chunk>();

        // Create the chunks
        for (int cx = (-1 * RenderSize); cx < RenderSize; cx++)
        {
            for (int cz = (-1 * RenderSize); cz < RenderSize; cz++)
            {
                Chunks.Add(GenerateChunk(new Vector2(cx, cz)));
            } 
        }
        InvokeRepeating("UpdateChunks", 4, 2);
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
            var chunk = GenerateChunk(chunkCoord);
            if (null != chunk)
            {
                Chunks.Add(chunk);
            }            
        }
    }

    private Chunk GenerateChunk(Vector2 chunkPos)
    {
        if (ChunkExists(chunkPos) && GetChunk(chunkPos).Generated)
        {
            return null;
        }
        GameObject chunkGameObject = new GameObject(string.Format("chunk{0},{1}", chunkPos.x, chunkPos.y));
        Chunk chunk = chunkGameObject.AddComponent(typeof(Chunk)) as Chunk;
        chunk.InitializeChunk(chunkPos, chunkGameObject);
        chunkGameObject.transform.position = new Vector3(chunkPos.x * 16, 0, chunkPos.y * 16);
        chunk.GenerateChunk(Seed);

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

        return chunk;
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