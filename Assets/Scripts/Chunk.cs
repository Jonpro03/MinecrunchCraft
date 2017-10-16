using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;
using System.Linq;

namespace Assets.Scripts
{
    public class Chunk : MonoBehaviour
    {
        public bool Generated { get; set; }

        public Vector2 WorldPosition { get; set; }

        public Vector2 ChunkPosition { get; set; }

        public GameObject ChunkGameObject { get; set; }

        public List<IBlock> TopLevelBlocks { get; set; }

        public Mesh ChunkMesh { get; set; }

        public IBlock[,,] Blocks;

        List<int> Triangles;

        List<Vector3> Verticies;

        List<Vector2> UVs;


        public void InitializeChunk(Vector2 pos, GameObject chunkGameObject)
        {
            TopLevelBlocks = new List<IBlock>();
            Generated = false;
            ChunkPosition = pos;
            ChunkMesh = new Mesh();
            ChunkMesh.name = "TerrainMesh";
            Blocks = new IBlock[16, 256, 16];
            ChunkGameObject = chunkGameObject;
        }

        public void GenerateChunk(string seed)
        {
            int cx = (int)ChunkPosition.x;
            int cz = (int)ChunkPosition.y;

            // Create the blocks            
            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    int blockWorldPosX = bx + (cx * 16);
                    int blockWorldPosZ = bz + (cz * 16);
                    int by = PerlinNoise(blockWorldPosX, blockWorldPosZ, seed);
                    by = Mathf.Max(0, by);
                    by = Mathf.Min(255, by);
                    Vector3 positionInChunk = new Vector3(bx, by, bz);
                    Blocks[bx, by, bz] = new GrassBlock(new Vector3(bx, by, bz), false, ChunkPosition);
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
                    if (columnY == block.PositionInChunk.y)
                    {
                        Blocks[bx, columnY, bz] = new GrassBlock(new Vector3(bx, columnY, bz), false, ChunkPosition);
                    }
                    else if (columnY < 3)
                    {
                        Blocks[bx, columnY, bz] = new BedrockBlock(new Vector3(bx, columnY, bz), false, ChunkPosition);
                    }
                    else
                    {
                        Blocks[bx, columnY, bz] = new DirtBlock(new Vector3(bx, columnY, bz), false, ChunkPosition);
                    }
                }
            }

            // Cut out some caves
            for (int bx = 0; bx < 16; bx++)
            {
                for (int by = 3; by < 256; by++)
                {
                    for (int bz = 0; bz < 16; bz++)
                    {
                        IBlock block = Blocks[bx, by, bz];
                        if (null == block)
                        {
                            continue;
                        }
                        bool isCaveBlock = PerlinCave(block.PositionInWorld, seed);
                        if (isCaveBlock)
                        {
                            if (block.IsTopLevel)
                            {
                                TopLevelBlocks.Remove(TopLevelBlocks.FirstOrDefault(b => b.PositionInChunk == block.PositionInChunk));
                            }
                            Blocks[bx, by, bz] = null;
                        }
                    }
                }
            }

            // Now that we have our chunk filled in, we need to figure out which block faces are visible.
            foreach (IBlock block in TopLevelBlocks)
            {
                int bx = (int)block.PositionInChunk.x;
                int by = (int)block.PositionInChunk.y;
                int bz = (int)block.PositionInChunk.z;

                // Check front, back, left and right to see if there's another visible block there.
                // Move down and repeat

                for (int columnY = by; columnY > 0; columnY--)
                {
                    if (Blocks[bx, columnY, bz] == null)
                    {
                        continue;
                    }

                    // Top
                    if (null == Blocks[bx, Mathf.Min(columnY + 1, 255), bz])
                    {
                        Blocks[bx, columnY, bz].TopVisible = true;
                    }

                    // Bottom
                    if (null == Blocks[bx, Mathf.Max(columnY - 1, 0), bz])
                    {
                        Blocks[bx, columnY, bz].BottomVisible = true;
                    }

                    // Left
                    //if (null == Blocks[Mathf.Max(bx - 1, 0), columnY, bz] || bx == 0)
                    if (null == Blocks[Mathf.Max(bx - 1, 0), columnY, bz])
                    {
                        Blocks[bx, columnY, bz].LeftVisible = true;
                    }
                    // Right
                    //if (null == Blocks[Mathf.Min(bx + 1, 15), columnY, bz] || bx == 15)
                    if (null == Blocks[Mathf.Min(bx + 1, 15), columnY, bz])
                    {
                        Blocks[bx, columnY, bz].RightVisible = true;
                    }
                    // Front
                    // if (null == Blocks[bx, columnY, Mathf.Max(bz - 1, 0)] || bz == 0)
                    if (null == Blocks[bx, columnY, Mathf.Max(bz - 1, 0)])
                    {
                        Blocks[bx, columnY, bz].FrontVisible = true;
                    }
                    // Back
                    //if (null == Blocks[bx, columnY, Mathf.Min(bz + 1, 15)] || bz == 15)
                    if (null == Blocks[bx, columnY, Mathf.Min(bz + 1, 15)])
                    {
                        Blocks[bx, columnY, bz].BackVisible = true;
                    }
                }
            }

            // Create the vertices
            Verticies = new List<Vector3>();
            UVs = new List<Vector2>();

            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    for (int by = 255; by >= 0; by--)
                    {
                        var block = Blocks[bx, by, bz];
                        if (null != block)
                        {
                            Verticies.AddRange(block.Verticies);
                            UVs.AddRange(block.UVs);
                        }
                    }
                }
            }

            // Create triangles for the vertices
            Triangles = new List<int>();
            for (int a = 0; a < Verticies.Count; a++)
            {
                Triangles.Add(a);
            }

            ChunkMesh.SetVertices(Verticies);
            ChunkMesh.SetUVs(0, UVs);
            ChunkMesh.SetTriangles(Triangles, 0);

            var meshFilter = ChunkGameObject.AddComponent<MeshFilter>();
            var meshRenderer = ChunkGameObject.AddComponent<MeshRenderer>();
            
            meshFilter.mesh = ChunkMesh;
            meshRenderer.material = Resources.Load<Material>("Materials/GrassBlock");

            ChunkMesh.RecalculateNormals();
            var collider = ChunkGameObject.AddComponent<MeshCollider>();
            Generated = true;

        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public int PerlinNoise(float x, float z, string seed)
        {
            int hash = seed.GetHashCode();
            int Noisiness = 70;
            int TerrainHeight = 50;
            //Generate a value from the given position, position is divided to make the noise more frequent.
            float pass1 = Mathf.PerlinNoise(((x + hash) / Noisiness), ((z + hash) / Noisiness)) * 10;
            float pass2 = Mathf.PerlinNoise(((hash - x) / Noisiness), ((hash - z) / Noisiness)) * 10;

            //Return the noise value
            return (int)(pass2 - pass1 + TerrainHeight);
        }

        public bool PerlinCave(Vector3 loc, string seed)
        {
            const float CAVEFILLPERCENT = 0.40f;
            const float CAVEHEIGHTFACTOR = 0.55f;
            const float STRETCHFACTOR = 0.0675f;

            int hash = seed.GetHashCode();
            int digitsInHash = hash.ToString().Length;
            float seedDecimal = hash / Mathf.Pow(10, digitsInHash - 1);
            loc.y /= CAVEHEIGHTFACTOR;

            float x = loc.x * STRETCHFACTOR / seedDecimal;
            float y = loc.y * STRETCHFACTOR / seedDecimal;
            float z = loc.z * STRETCHFACTOR / seedDecimal;

            List<float> Perlins = new List<float>()
            {
                Mathf.PerlinNoise(x, z),
                Mathf.PerlinNoise(y, z),
                Mathf.PerlinNoise(z, y),
                Mathf.PerlinNoise(x, y),
                Mathf.PerlinNoise(y, x),
                Mathf.PerlinNoise(x, -x),
                Mathf.PerlinNoise(y, -y)
            };

            return Perlins.Average() < CAVEFILLPERCENT;
        }
    }
}
