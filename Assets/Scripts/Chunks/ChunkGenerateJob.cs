using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Chunks
{
    public class ChunkGenerateJob : ThreadedJob
    {
        public Chunk Chunk { get; private set; }

        public ChunkGenerateJob(Chunk chunk)
        {
            Chunk = chunk;
        }

        protected override void ThreadFunction()
        {
            int cx = (int)Chunk.ChunkPosition.x;
            int cz = (int)Chunk.ChunkPosition.y;

            // Create the blocks            
            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    int blockWorldPosX = bx + (cx * 16);
                    int blockWorldPosZ = bz + (cz * 16);
                    int by = Utility.PerlinNoise.Terrain(blockWorldPosX, blockWorldPosZ, World.World.SeedHash, (int)Chunk.Biome);
                    by = Mathf.Max(0, by);
                    by = Mathf.Min(255, by);
                    Chunk.Blocks[bx, by, bz] = new GrassBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);

                    // fill in below the terrain
                    for (int columnY = by; columnY >= 0; columnY--)
                    {
                        IBlock block = null;
                        if (columnY == by)
                        {
                            if ((int)Chunk.Biome == 5)
                                block = new SandBlock(new Vector3(bx, columnY, bz), Chunk.ChunkPosition);
                            else if ((int)Chunk.Biome == 4)
                                block = new GrassBlock(new Vector3(bx, columnY, bz), Chunk.ChunkPosition);
                            else if ((int)Chunk.Biome == 3)
                                block = new StoneBlock(new Vector3(bx, columnY, bz), Chunk.ChunkPosition);
                            else
                                block = new CobblestoneBlock(new Vector3(bx, columnY, bz), Chunk.ChunkPosition);
                        }
                        else if (columnY < 6)
                        {
                            block = new BedrockBlock(new Vector3(bx, columnY, bz), Chunk.ChunkPosition);
                        }
                        else if (columnY < 55)
                        {
                            block = new StoneBlock(new Vector3(bx, columnY, bz), Chunk.ChunkPosition);
                        }
                        else
                        {
                            block = new DirtBlock(new Vector3(bx, columnY, bz), Chunk.ChunkPosition);
                        }
                        Chunk.Blocks[bx, columnY, bz] = block;
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
                        IBlock block = Chunk.Blocks[bx, by, bz];
                        if (null == block)
                        {
                            continue;
                        }
                        bool isCaveBlock = Utility.PerlinNoise.Cave(block.PositionInWorld, World.World.SeedHash);
                        if (isCaveBlock)
                        {
                            Chunk.Blocks[bx, by, bz] = null;
                        }
                    }
                }
            }
            Chunk.Generated = true;
            Chunk.HasUpdate = true;
        }
    }
}
