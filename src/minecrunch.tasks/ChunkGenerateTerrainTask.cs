using System.Collections.Generic;
using System.Threading.Tasks;
using minecrunch.models.Biomes;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using minecrunch.utilities;

namespace minecrunch.tasks
{
    public sealed class ChunkGenerateTerrainTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private readonly List<ChunkSection> sections;
        private PerlinNoise pNoise;
        private BlockInfo bInfo;

        public ChunkGenerateTerrainTask(Chunk newChunk)
        {
            chunk = newChunk;
            //chunk.sections = new ChunkSection[16];
            for (int x = 0; x < chunk.sections.Length; x++)
            {
                chunk.sections[x] = new ChunkSection
                {
                    number = x,
                    name = $"subchunk{x}"
                };
            }
            sections = new List<ChunkSection>(chunk.sections);

            bInfo = BlockInfo.Instance;
            pNoise = PerlinNoise.Instance;
        }

        protected override void ThreadFunction()
        {
            chunk.biome = (Biome)pNoise.Biome(chunk.x, chunk.y);

            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    string key = $"{bx},{bz}";
                    chunk.SurfaceMap[key] = pNoise.Terrain(bx + (chunk.x * 16), bz + (chunk.y * 16), (int)chunk.biome);
                }
            }

            Parallel.ForEach(sections, ProcessSection);
        }

        private void ProcessSection(ChunkSection section)
        {
            int sectionYOffset = 16 * section.number;

            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    int terrainY = chunk.SurfaceMap[$"{bx},{bz}"];

                    for (int by = sectionYOffset; by < sectionYOffset + 16; by++)
                    {
                        Block block = new Block();

                        // If above the terrain, just mark it air and move on.
                        if (by > terrainY)
                        {
                            block.Id = BlockIds.AIR;
                            section.blocks[bx, by - sectionYOffset, bz] = block;
                            continue;
                        }

                        switch (chunk.biome)
                        {
                            case Biome.Desert:
                                {
                                    if (by > terrainY - 5 && by <= terrainY) // 5 layers of sand
                                    {
                                        block.Id = BlockIds.SAND;
                                    }
                                    break;
                                }

                            case Biome.Mountain:
                                {
                                    if (by == terrainY)
                                    {
                                        block.Id = BlockIds.DIRT;
                                    }
                                    else if (by > terrainY - 5)
                                    {
                                        block.Id = BlockIds.STONE;
                                    }
                                    break;
                                }

                            case Biome.Plains:
                            case Biome.Forest:
                            case Biome.Grassland:
                            default:
                                {
                                    if (by == terrainY)
                                    {
                                        block.Id = BlockIds.GRASS;
                                    }
                                    else if (by > terrainY - 5)
                                    {
                                        block.Id = BlockIds.DIRT;
                                    }
                                    break;
                                }
                        }

                        if (by < 3)
                        {
                            block.Id = BlockIds.BEDROCK;
                        }
                        else if (by < terrainY && string.IsNullOrEmpty(block.Id))
                        {
                            block.Id = BlockIds.STONE;
                        }
                        else if (string.IsNullOrEmpty(block.Id))
                        {
                            block.Id = BlockIds.AIR;
                        }

                        section.blocks[bx, by - sectionYOffset, bz] = block;
                    }
                }
            }
        }
    }
}
