using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace minecrunch.tasks
{
    public sealed class ChunkCalcVerticiesTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private BlockInfo bInfo;

        public ChunkCalcVerticiesTask(Chunk c)
        {
            chunk = c;
            bInfo = BlockInfo.Instance;
        }

        protected override void ThreadFunction()
        {
            var watch = Stopwatch.StartNew();

            foreach (ChunkSection sec in chunk.sections)
            {
                ProcessSection(sec);
            }

            //chunk.sections.ToList().ForEach(s => ProcessSection(s));
            //Parallel.ForEach(chunk.sections, ProcessSection);
            watch.Stop();
            chunk.processTimeMs = watch.ElapsedMilliseconds;

        }

        private void ProcessSection(ChunkSection section)
        {
            int sectionYOffset = 16 * section.number;
            int trianglesCount = 0;
            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    for (int by = 0; by < 16; by++)
                    {
                        Block block = section.blocks[bx, by, bz];
                        //if (block.Id is BlockIds.AIR) { continue; }
                        if (block is null) { continue; }

                        if (block.faceByte == 0b00000000) { continue; }

                        int textureKey = 0;
                        string texture = bInfo.GetBlockTexture(block.Id);
                        if (section.Mesh.Materials.Values.Contains(texture))
                        {
                            textureKey = section.Mesh.Materials.FirstOrDefault(a => a.Value == texture).Key;
                        }
                        else
                        {
                            textureKey = section.Mesh.Materials.Count;
                            section.Mesh.Materials.Add(textureKey, texture);
                            section.Mesh.Triangles.Add(textureKey, new List<int>());
                        }

                        Vector3 chunkPos = new Vector3(bx, by + sectionYOffset, bz);
                        int verticieCount = 0;

                        if (block.GetFaceVisible(Sides.Left))
                        {
                            verticieCount += 6;
                            section.Mesh.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 1, 1) + chunkPos, //Todo: can this be optimized?
                                new Vector3(0, 1, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 0, 1) + chunkPos,
                                new Vector3(0, 1, 1) + chunkPos
                            });

                            section.Mesh.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0, 0.5f),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.25f, 0),
                                new Vector2(0.25f, 0),
                                new Vector2(0, 0),
                                new Vector2(0, 0.5f)
                            });
                        }

                        if (block.GetFaceVisible(Sides.Right))
                        {
                            verticieCount += 6;
                            section.Mesh.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 0, 0) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos
                            });

                            section.Mesh.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.5f, 0.5f),
                                new Vector2(0.75f, 0.5f),
                                new Vector2(0.75f, 0),
                                new Vector2(0.75f, 0),
                                new Vector2(0.5f, 0),
                                new Vector2(0.5f, 0.5f)
                            });
                        }

                        if (block.GetFaceVisible(Sides.Top))
                        {
                            verticieCount += 6;
                            section.Mesh.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 1, 1) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(0, 1, 0) + chunkPos,
                                new Vector3(0, 1, 1) + chunkPos
                            });

                            section.Mesh.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.5f, 1),
                                new Vector2(0.25f, 1),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.5f, 0.5f),
                                new Vector2(0.5f, 1),
                            });
                        }

                        if (block.GetFaceVisible(Sides.Bottom))
                        {
                            verticieCount += 6;
                            section.Mesh.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(1, 0, 0) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(0, 0, 1) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos
                            });

                            section.Mesh.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.25f, 1),
                                new Vector2(0, 1),
                                new Vector2(0, 0.5f),
                                new Vector2(0, 0.5f),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.25f, 1)
                            });
                        }

                        if (block.GetFaceVisible(Sides.Front))
                        {
                            verticieCount += 6;
                            section.Mesh.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(1, 0, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 1, 0) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos
                            });

                            section.Mesh.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.5f, 0.5f),
                                new Vector2(0.5f, 0),
                                new Vector2(0.25f, 0),
                                new Vector2(0.25f, 0),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.5f, 0.5f)
                            });
                        }

                        if (block.GetFaceVisible(Sides.Back))
                        {
                            verticieCount += 6;
                            section.Mesh.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 0, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(0, 1, 1) + chunkPos,
                                new Vector3(0, 0, 1) + chunkPos
                            });

                            section.Mesh.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.5f, 1),
                                new Vector2(0.75f, 1),
                                new Vector2(0.75f, 0.5f),
                                new Vector2(0.75f, 0.5f),
                                new Vector2(0.5f, 0.5f),
                                new Vector2(0.5f, 1)
                            });
                        }

                        for (var a = 0; a < verticieCount; a++)
                        {
                            section.Mesh.Triangles[textureKey].Add(trianglesCount + a);
                        }
                        trianglesCount += verticieCount;
                    }
                }
            }
            
        }
    }
}
