using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using minecrunch.utilities;
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
            //chunk.sections.ToList().ForEach(s => ProcessSection(s));
            
            Parallel.ForEach(chunk.sections, ProcessSection);
            
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
                        if (block.Id is BlockIds.AIR) { continue; }

                        int textureKey = 0;

                        if (!(block.FacesVisible.back ||
                            block.FacesVisible.front ||
                            block.FacesVisible.left ||
                            block.FacesVisible.right ||
                            block.FacesVisible.top ||
                            block.FacesVisible.bottom))
                        {
                            continue;
                        }

                        string texture = bInfo.GetBlockTexture(block.Id);
                        if (section.Materials.Values.Contains(texture))
                        {
                            textureKey = section.Materials.FirstOrDefault(a => a.Value == texture).Key;
                        }
                        else
                        {
                            textureKey = section.Materials.Count;
                            section.Materials.Add(textureKey, texture);
                            section.Triangles.Add(textureKey, new List<int>());
                        }

                        Vector3 chunkPos = new Vector3(bx, by + sectionYOffset, bz);
                        int verticieCount = 0;

                        if (block.FacesVisible.left)
                        {
                            verticieCount += 6;
                            section.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 1, 1) + chunkPos, //Todo: can this be optimized?
                                new Vector3(0, 1, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 0, 1) + chunkPos,
                                new Vector3(0, 1, 1) + chunkPos
                            });

                            section.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0, 0.5f),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.25f, 0),
                                new Vector2(0.25f, 0),
                                new Vector2(0, 0),
                                new Vector2(0, 0.5f)
                            });
                        }

                        if (block.FacesVisible.right)
                        {
                            verticieCount += 6;
                            section.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 0, 0) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos
                            });

                            section.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.5f, 0.5f),
                                new Vector2(0.75f, 0.5f),
                                new Vector2(0.75f, 0),
                                new Vector2(0.75f, 0),
                                new Vector2(0.5f, 0),
                                new Vector2(0.5f, 0.5f)
                            });
                        }

                        if (block.FacesVisible.top)
                        {
                            verticieCount += 6;
                            section.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 1, 1) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(0, 1, 0) + chunkPos,
                                new Vector3(0, 1, 1) + chunkPos
                            });

                            section.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.5f, 1),
                                new Vector2(0.25f, 1),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.5f, 0.5f),
                                new Vector2(0.5f, 1),
                            });
                        }

                        if (block.FacesVisible.bottom)
                        {
                            verticieCount += 6;
                            section.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(1, 0, 0) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(0, 0, 1) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos
                            });

                            section.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.25f, 1),
                                new Vector2(0, 1),
                                new Vector2(0, 0.5f),
                                new Vector2(0, 0.5f),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.25f, 1)
                            });
                        }

                        if (block.FacesVisible.front)
                        {
                            verticieCount += 6;
                            section.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(1, 1, 0) + chunkPos,
                                new Vector3(1, 0, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 0, 0) + chunkPos,
                                new Vector3(0, 1, 0) + chunkPos,
                                new Vector3(1, 1, 0) + chunkPos
                            });

                            section.UVs.AddRange(new List<Vector2>
                            {
                                new Vector2(0.5f, 0.5f),
                                new Vector2(0.5f, 0),
                                new Vector2(0.25f, 0),
                                new Vector2(0.25f, 0),
                                new Vector2(0.25f, 0.5f),
                                new Vector2(0.5f, 0.5f)
                            });
                        }

                        if (block.FacesVisible.back)
                        {
                            verticieCount += 6;
                            section.Verticies.AddRange(new List<Vector3>
                            {
                                new Vector3(0, 0, 1) + chunkPos,
                                new Vector3(1, 0, 1) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(1, 1, 1) + chunkPos,
                                new Vector3(0, 1, 1) + chunkPos,
                                new Vector3(0, 0, 1) + chunkPos
                            });

                            section.UVs.AddRange(new List<Vector2>
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
                            section.Triangles[textureKey].Add(trianglesCount + a);
                        }
                        trianglesCount += verticieCount;
                    }
                }
            }
            
        }
    }
}
