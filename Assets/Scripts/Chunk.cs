using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;
using System.Linq;

namespace Assets.Scripts
{
    public class Chunk : MonoBehaviour
    {
        public bool Generated { get; private set; }

        public bool HasUpdate { get; set; }

        public Vector2 WorldPosition { get; private set; }

        public Vector2 ChunkPosition { get; private set; }

        public GameObject ChunkGameObject { get; set; }

        public IBlock[,,] Blocks;

        public List<int> Triangles;

        public List<Vector3> Verticies;

        public List<Vector2> UVs;


        public void InitializeChunk(Vector2 pos, GameObject chunkGameObject)
        {
            Generated = false;
            HasUpdate = false;
            ChunkPosition = pos;
            
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
                    Blocks[bx, by, bz] = new GrassBlock(new Vector3(bx, by, bz), ChunkPosition);

                    // fill in below the terrain
                    for (int columnY = by; columnY >= 0; columnY--)
                    {
                        if (columnY == by)
                        {
                            Blocks[bx, columnY, bz] = new GrassBlock(new Vector3(bx, columnY, bz), ChunkPosition);
                        }
                        else if (columnY < 3)
                        {
                            Blocks[bx, columnY, bz] = new BedrockBlock(new Vector3(bx, columnY, bz), ChunkPosition);
                        }
                        else
                        {
                            Blocks[bx, columnY, bz] = new DirtBlock(new Vector3(bx, columnY, bz), ChunkPosition);
                        }
                    }

                }
            }

            // Cut out some caves
            for (int bx = 0; bx < 16; bx++)
            {
                for (int by = 5; by < 256; by++)
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
                            Blocks[bx, by, bz] = null;
                        }
                    }
                }
            }

            // Now that we have our chunk filled in, we need to figure out which block faces are visible.
            for (int bx = 0; bx < 16; bx++)
            {
                for (int by = 3; by < 256; by++)
                {
                    for (int bz = 0; bz < 16; bz++)
                    {

                        // Check front, back, left and right to see if there's another visible block there.
                        // Move down and repeat
                        if (Blocks[bx, by, bz] == null)
                        {
                            continue;
                        }

                        // Top
                        if (null == Blocks[bx, Mathf.Min(by + 1, 255), bz])
                        {
                            Blocks[bx, by, bz].TopVisible = true;
                        }

                        // Bottom
                        if (null == Blocks[bx, Mathf.Max(by - 1, 0), bz])
                        {
                            Blocks[bx, by, bz].BottomVisible = true;
                        }

                        // Left
                        //if (null == Blocks[Mathf.Max(bx - 1, 0), by, bz] || bx == 0)
                        if (null == Blocks[Mathf.Max(bx - 1, 0), by, bz] || bx == 0)
                        {
                            Blocks[bx, by, bz].LeftVisible = true;
                        }
                        // Right
                        //if (null == Blocks[Mathf.Min(bx + 1, 15), by, bz] || bx == 15)
                        if (null == Blocks[Mathf.Min(bx + 1, 15), by, bz] || bx == 15)
                        {
                            Blocks[bx, by, bz].RightVisible = true;
                        }
                        // Front
                        // if (null == Blocks[bx, by, Mathf.Max(bz - 1, 0)] || bz == 0)
                        if (null == Blocks[bx, by, Mathf.Max(bz - 1, 0)] || bz == 0)
                        {
                            Blocks[bx, by, bz].FrontVisible = true;
                        }
                        // Back
                        //if (null == Blocks[bx, by, Mathf.Min(bz + 1, 15)] || bz == 15)
                        if (null == Blocks[bx, by, Mathf.Min(bz + 1, 15)] || bz == 15)
                        {
                            Blocks[bx, by, bz].BackVisible = true;
                        }
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

            
            Generated = true;
            HasUpdate = true;
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
            const float CAVEFILLPERCENT = 0.35f;
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
                Mathf.PerlinNoise(z, x),
                Mathf.PerlinNoise(x, x),
                Mathf.PerlinNoise(y, y)
            };

            return Perlins.Average() < CAVEFILLPERCENT;
        }
    }
}
