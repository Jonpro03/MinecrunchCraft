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
    public class ChunkUpdateJob : ThreadedJob
    {
        public Chunk Chunk { get; private set; }

        public ChunkUpdateJob(Chunk chunk)
        {
            Chunk = chunk;
        }

        protected override void ThreadFunction()
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
                        if (Chunk.Blocks[bx, by, bz] == null)
                        {
                            continue;
                        }

                        IBlock block = Chunk.Blocks[bx, by, bz];

                        // Top
                        if (null == Chunk.Blocks[bx, Mathf.Min(by + 1, 255), bz])
                        {
                            block.TopVisible = true;
                        }

                        // Bottom
                        if (null == Chunk.Blocks[bx, Mathf.Max(by - 1, 0), bz])
                        {
                            block.BottomVisible = true;
                        }

                        // Left
                        //if (null == Blocks[Mathf.Max(bx - 1, 0), by, bz] || bx == 0)
                        if (null == Chunk.Blocks[Mathf.Max(bx - 1, 0), by, bz] || bx == 0)
                        {
                            block.LeftVisible = true;
                        }
                        // Right
                        //if (null == Blocks[Mathf.Min(bx + 1, 15), by, bz] || bx == 15)
                        if (null == Chunk.Blocks[Mathf.Min(bx + 1, 15), by, bz] || bx == 15)
                        {
                            block.RightVisible = true;
                        }
                        // Front
                        // if (null == Blocks[bx, by, Mathf.Max(bz - 1, 0)] || bz == 0)
                        if (null == Chunk.Blocks[bx, by, Mathf.Max(bz - 1, 0)] || bz == 0)
                        {
                            block.FrontVisible = true;
                        }
                        // Back
                        //if (null == Blocks[bx, by, Mathf.Min(bz + 1, 15)] || bz == 15)
                        if (null == Chunk.Blocks[bx, by, Mathf.Min(bz + 1, 15)] || bz == 15)
                        {
                            block.BackVisible = true;
                        }
                    }
                }
            }

            // Create the blocks
            int trianglesCount = 0;
            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    for (int by = 255; by >= 0; by--)
                    {
                        var block = Chunk.Blocks[bx, by, bz];
                        if (null != block)
                        {
                            int textureKey = 0;

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
            }
            Chunk.HasUpdate = false;
            Chunk.IsDrawn = false;
        }
    }
}
