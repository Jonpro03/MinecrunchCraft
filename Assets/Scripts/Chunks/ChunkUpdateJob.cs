using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Blocks;
using Assets.Scripts.World;
using System;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Chunks
{
    public class ChunkUpdateJob : ThreadedJob
    {
        public Chunk Chunk { get; private set; }

        public ChunkUpdateJob(Chunk chunk)
        {
            Chunk = chunk;
        }

        protected override void ThreadFunction()
        {
            try
            {
                // Now that we have our chunk filled in, we need to figure out which block faces are visible.
                for (int bx = 0; bx < 16; bx++)
                {
                    for (int by = 3; by < 256; by++)
                    {
                        for (int bz = 0; bz < 16; bz++)
                        {

                            // Check front, back, left and right to see if there's another visible block there.

                            // Move down and repeat
                            if (Chunk.Blocks[bx, by, bz] is AirBlock)
                            {
                                continue;
                            }

                            // Top
                            if (Chunk.Blocks[bx, Mathf.Min(by + 1, 255), bz] is AirBlock)
                            {
                                Chunk.Blocks[bx, by, bz].TopVisible = true;
                            }

                            // Bottom
                            if (Chunk.Blocks[bx, Mathf.Max(by - 1, 0), bz] is AirBlock)
                            {
                                Chunk.Blocks[bx, by, bz].BottomVisible = true;
                            }

                            // Left
                            if (Chunk.Blocks[Mathf.Max(bx - 1, 0), by, bz] is AirBlock)
                            {
                                Chunk.Blocks[bx, by, bz].LeftVisible = true;
                            }
                            // Right
                            if (Chunk.Blocks[Mathf.Min(bx + 1, 15), by, bz] is AirBlock)
                            {
                                Chunk.Blocks[bx, by, bz].RightVisible = true;
                            }
                            // Front
                            if (Chunk.Blocks[bx, by, Mathf.Max(bz - 1, 0)] is AirBlock)
                            {
                                Chunk.Blocks[bx, by, bz].FrontVisible = true;
                            }
                            // Back
                            if (Chunk.Blocks[bx, by, Mathf.Min(bz + 1, 15)] is AirBlock)
                            {
                                Chunk.Blocks[bx, by, bz].BackVisible = true;
                            }

                            var block = Chunk.Blocks[bx, by, bz];
                            
                            // Chunk Boundaries
                            /*
                            if (bx == 0)
                            {
                                block.LeftVisible = WorldTerrain.GetBlockRef(new Vector3(bx,by,bz) + Vector3.left) is AirBlock;
                            }

                            if (bx == 15)
                            {
                                block.RightVisible = WorldTerrain.GetBlockRef(new Vector3(bx, by, bz) + Vector3.right) is AirBlock;
                            }

                            if (bz == 0)
                            {
                                block.FrontVisible = WorldTerrain.GetBlockRef(new Vector3(bx, by, bz) + Vector3.back) is AirBlock;
                            }

                            if (bz == 15)
                            {
                                block.BackVisible = WorldTerrain.GetBlockRef(new Vector3(bx, by, bz) + Vector3.forward) is AirBlock;
                            }
                            */
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
                // Create the blocks
                int trianglesCount = 0;
                for (int bx = 0; bx < 16; bx++)
                {
                    for (int bz = 0; bz < 16; bz++)
                    {
                        for (int by = 255; by >= 0; by--)
                        {
                            Block block = Chunk.Blocks[bx, by, bz];
                            if (block is AirBlock)
                            {
                                continue;
                            }

                            int textureKey = 0;

                            if (block.IsVisible())
                            {
                                // Add this block's texture to the chunk
                                if (Chunk.Materials.Values.Contains(block.Texture))
                                {
                                    textureKey = Chunk.Materials.FirstOrDefault(a => a.Value == block.Texture).Key;
                                }
                                else
                                {
                                    textureKey = Chunk.Materials.Count;
                                    Chunk.Materials.Add(textureKey, block.Texture);
                                    Chunk.Triangles.Add(textureKey, new List<int>());
                                }
                            }

                            for (var a = 0; a < block.Verticies.Count; a++)
                            {
                                Chunk.Triangles[textureKey].Add(trianglesCount + a);
                            }
                            trianglesCount += block.Verticies.Count;
                            Chunk.Verticies.AddRange(block.Verticies);
                            Chunk.UVs.AddRange(block.UVs);
                        }
                    }
                }
                Chunk.HasUpdate = false;
                Chunk.IsDrawn = false;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            // Notify Neighbor chunks
            /*
            WorldTerrain.ScheduleChunkUpdate(Chunk.ChunkPosition + Vector2.up);
            WorldTerrain.ScheduleChunkUpdate(Chunk.ChunkPosition + Vector2.down);
            WorldTerrain.ScheduleChunkUpdate(Chunk.ChunkPosition + Vector2.left);
            WorldTerrain.ScheduleChunkUpdate(Chunk.ChunkPosition + Vector2.right);
            */
        }
    }
}
