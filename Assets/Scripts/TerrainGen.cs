﻿using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;

public class TerrainGen : MonoBehaviour
{

    //Public variable for the size of the terrain, width and heigth
    public int RenderSize = 5;

    //Height multiplies the final noise output
    public float HeightOfTerrain = 10.0f;

    public Material BaseMaterial;

    public float Seed = 13.4f;

    //This divides the noise frequency
    //Higher is Flatter
    public float NoiseSize = 10.0f;

    public IBlock[,,] Blocks;

    private List<IBlock> TopLevelBlocks { get; set; }


    //Function that inputs the position and spits out a float value based on the perlin noise
    public int PerlinNoise(float x, float z)
    {
        //Generate a value from the given position, position is divided to make the noise more frequent.
        float noise = Mathf.PerlinNoise(((x + Mathf.Sqrt(Seed)) / NoiseSize), ((z + Mathf.Sqrt(Seed)) / NoiseSize));

        //Return the noise value
        return (int)(noise * HeightOfTerrain);
    }

    //Generates the terrain
    void Start()
    {
        //BaseMaterial = Resources.Load<Material>("Materials/DirtBlock");

        
        // Create the chunks
        for (int cx = (-1 * RenderSize); cx < RenderSize; cx++)
        {
            for (int cz = (-1 * RenderSize); cz < RenderSize; cz++)
            {
                TopLevelBlocks = new List<IBlock>();
                GameObject chunkGameObject = new GameObject(string.Format("chunk{0},{1}", cx, cz));
                chunkGameObject.transform.position = new Vector3(cx * 16, 0, cz * 16);

                Chunk chunk = chunkGameObject.AddComponent(typeof(Chunk)) as Chunk;
                chunk.ChunkPosition = new Vector2(cx, cz);
                chunk.Generated = false;

                chunk.mesh = new Mesh();
                chunk.mesh.name = "TerrainMesh";

                // Create the blocks
                Blocks = new IBlock[16, 256, 16];
                for (int bx = 0; bx < 16; bx++)
                {
                    for (int bz = 0; bz < 16; bz++)
                    {
                        int blockWorldPosX = bx + (cx * 16);
                        int blockWorldPosZ = bz + (cz * 16);
                        int by = PerlinNoise(blockWorldPosX, blockWorldPosZ);
                        by = Mathf.Max(0, by);
                        by = Mathf.Min(255, by);
                        Vector3 positionInChunk = new Vector3(bx, by, bz);
                        Blocks[bx, by, bz] = new DirtBlock(positionInChunk, true);
                        TopLevelBlocks.Add(Blocks[bx, by, bz]);
                    }
                }


                // Fill in blocks below the terrain gen blocks.
                // Set them all not visible so we don't generate verticies for them.
                foreach (IBlock block in TopLevelBlocks)
                {
                    int bx = (int)block.PositionInChunk.x;
                    int bz = (int)block.PositionInChunk.z;
                    for (int columnY = (int)block.PositionInChunk.y; columnY >= 0; columnY--)
                    {
                        Blocks[bx, columnY, bz] = new DirtBlock(new Vector3(bx, columnY, bz), false);

                    }

                }

                // Now that we have our chunk filled in, we need to figure out which block faces are visible.
                foreach (IBlock block in TopLevelBlocks)
                {
                    int bx = (int)block.PositionInChunk.x;
                    int by = (int)block.PositionInChunk.y;
                    int bz = (int)block.PositionInChunk.z;

                    // All top level blocks have the top visible
                    Blocks[bx, by, bz].TopVisible = true;
                    // Check front, back, left and right to see if there's another visible block there.
                    // Move down and repeat

                    for (int columnY = by; columnY > 0; columnY--)
                    {
                        // Left
                        if (null == Blocks[Mathf.Max(bx - 1, 0), columnY, bz])
                        {
                            Blocks[bx, columnY, bz].LeftVisible = true;
                        }
                        // Right
                        if (null == Blocks[Mathf.Min(bx + 1, 15), columnY, bz])
                        {
                            Blocks[bx, columnY, bz].RightVisible = true;
                        }
                        // Front
                        if (null == Blocks[bx, columnY, Mathf.Max(bz - 1, 0)])
                        {
                            Blocks[bx, columnY, bz].FrontVisible = true;
                        }
                        // Back
                        if (null == Blocks[bx, columnY, Mathf.Min(bz + 1, 15)])
                        {
                            Blocks[bx, columnY, bz].BackVisible = true;
                        }
                    }
                }

                // Create the vertices
                var vertices = new List<Vector3>();
                var uv = new List<Vector2>();

                for (int bx = 0; bx < 16; bx++)
                {
                    for (int bz = 0; bz < 16; bz++)
                    {
                        for (int by = 255; by >= 0; by--)
                        {
                            var block = Blocks[bx, by, bz];
                            if (null != block)
                            {
                                vertices.AddRange(block.Verticies);
                                uv.AddRange(block.UVs);
                            }                           
                        }
                    }
                }

                chunk.mesh.SetVertices(vertices);

                chunk.mesh.SetUVs(0, uv);

                List<int> triangles = new List<int>();
                for (int a = 0; a < vertices.Count; a++)
                {
                    triangles.Add(a);
                }


                chunk.mesh.SetTriangles(triangles, 0);

                

                var meshFilter = chunkGameObject.AddComponent<MeshFilter>();
                meshFilter.mesh = chunk.mesh;
                var meshRenderer = chunkGameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = BaseMaterial;

                chunk.mesh.RecalculateNormals();
                var collider = chunkGameObject.AddComponent<MeshCollider>();
            }
        }
    }
}