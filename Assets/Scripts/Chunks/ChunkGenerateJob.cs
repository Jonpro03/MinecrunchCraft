using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using Assets.Scripts;
using UnityEngine;
using Assets.Scripts.Interfaces;
using Assets.Scripts.World;

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

            try
            {
                // Create the blocks            
                for (int bx = 0; bx < 16; bx++)
                {
                    for (int bz = 0; bz < 16; bz++)
                    {
                        int blockWorldPosX = bx + (cx * 16);
                        int blockWorldPosZ = bz + (cz * 16);
                        int terrainY = Utility.PerlinNoise.Terrain(blockWorldPosX, blockWorldPosZ, World.World.SeedHash, (int)Chunk.Biome);
                        terrainY = Mathf.Max(0, terrainY);
                        terrainY = Mathf.Min(255, terrainY);

                        for (int by = 0; by < 256; by++)
                        {
                            Block block = Chunk.Blocks[bx, by, bz];
                            block = new AirBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                            if (terrainY == by)
                            {
                                if ((int)Chunk.Biome == 5)
                                    block = new SandBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                                else if ((int)Chunk.Biome == 4)
                                    block = new GrassBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                                else if ((int)Chunk.Biome == 3)
                                    block = new StoneBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                                else
                                    block = new CobblestoneBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                            }
                            else if (by < 6)
                            {
                                block = new BedrockBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                            }
                            else if (by < 52)
                            {
                                block = new StoneBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                            }
                            else if (by < terrainY)
                            {
                                block = new DirtBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                            }
                            Chunk.Blocks[bx, by, bz] = block;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            try
            {
                // Cut out some caves
                for (int bx = 0; bx < 16; bx++)
                {
                    for (int by = 5; by < 256; by++)
                    {
                        for (int bz = 0; bz < 16; bz++)
                        {
                            IBlock block = Chunk.Blocks[bx, by, bz];
                            if (block is AirBlock)
                            {
                                continue;
                            }
                            bool isCaveBlock = Utility.PerlinNoise.Cave(block.PositionInWorld, World.World.SeedHash);
                            if (isCaveBlock)
                            {
                                block = new AirBlock(new Vector3(bx, by, bz), Chunk.ChunkPosition);
                            }
                        }
                    }
                }
                Chunk.Generated = true;
                Chunk.HasUpdate = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
