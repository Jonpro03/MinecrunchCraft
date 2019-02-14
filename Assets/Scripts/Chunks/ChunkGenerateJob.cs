using System;
using Assets.Scripts.Blocks;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Chunks
{
    public class ChunkGenerateJob : ThreadedJob
    {
        public readonly Chunk chunk;

        public ChunkGenerateJob(Chunk chunk)
        {
            this.chunk = chunk;
        }

        protected override void ThreadFunction()
        {
            int cx = (int)chunk.ChunkPosition.x;
            int cz = (int)chunk.ChunkPosition.y;

            try
            {
                // Create the blocks            
                for (int bx = 0; bx < 16; bx++)
                {
                    for (int bz = 0; bz < 16; bz++)
                    {
                        int blockWorldPosX = bx + (cx * 16);
                        int blockWorldPosZ = bz + (cz * 16);
                        int terrainY = Utility.PerlinNoise.Terrain(blockWorldPosX, blockWorldPosZ, World.World.SeedHash, (int)chunk.Biome);
                        chunk.SurfaceMap.Add(new Vector2Int(bx, bz), terrainY);
                        terrainY = Mathf.Max(0, terrainY);
                        terrainY = Mathf.Min(255, terrainY);

                        for (int by = 0; by < 256; by++)
                        {
                            Block block = chunk.Blocks[bx, by, bz];
                            block = new AirBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                            if (terrainY == by)
                            {
                                if ((int)chunk.Biome == 5)
                                    block = new SandBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                                else if ((int)chunk.Biome == 4)
                                {
                                    block = new GrassBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                                    // Generate tree
                                }

                                else if ((int)chunk.Biome == 3)
                                    block = new StoneBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                                else
                                    block = new CobblestoneBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                            }
                            else if (by < 6)
                            {
                                block = new BedrockBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                            }
                            else if (by < 52)
                            {
                                block = new StoneBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                            }
                            else if (by < terrainY)
                            {
                                block = new DirtBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                            }
                            chunk.Blocks[bx, by, bz] = block;
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
                            IBlock block = chunk.Blocks[bx, by, bz];
                            if (block is AirBlock)
                            {
                                continue;
                            }
                            bool isCaveBlock = Utility.PerlinNoise.Cave(block.PositionInWorld, World.World.SeedHash);
                            if (isCaveBlock)
                            {
                                block = new AirBlock(new Vector3(bx, by, bz), chunk.ChunkPosition);
                            }
                        }
                    }
                }
                chunk.Generated = true;
                chunk.HasUpdate = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
