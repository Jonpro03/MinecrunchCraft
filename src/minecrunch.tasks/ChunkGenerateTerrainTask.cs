using minecrunch.models;
using minecrunch.models.Biomes;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    chunk.SurfaceMap[bx, bz] = pNoise.Terrain(bx + (chunk.x * 16), bz + (chunk.y * 16));
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
                    double terrainY = chunk.SurfaceMap[bx,bz];

                    for (int by = sectionYOffset; by < sectionYOffset + 16; by++)
                    {
                        Block block = new Block
                        {
                            x = (byte)bx,
                            y = (byte)by,
                            z = (byte)bz
                        };

                        // If above the terrain, just mark it air and move on.
                        if (by > terrainY)
                        {
                            //block.Id = BlockIds.AIR;
                            //section.blocks[bx, by - sectionYOffset, bz] = block;
                            continue;
                        }

                        // Ocean / River
                        if (terrainY <= 48)
                        {
                            block.Id = BlockIds.STONE;
                        }
                        // Beach
                        else if (terrainY < 52.5)
                        {
                            if (by > terrainY - 3)
                                block.Id = BlockIds.SAND;
                            else
                                block.Id = BlockIds.STONE;
                        }
                        // Grass
                        else if (terrainY < 60)
                        {
                            if (by == (int) terrainY)
                                block.Id = BlockIds.GRASS;
                            else if (by > terrainY - 2)
                                block.Id = BlockIds.DIRT;
                            else
                                block.Id = BlockIds.STONE;
                        }
                        // Mountain
                        else if (terrainY < 86)
                        {
                            if (by == (int)terrainY)
                                block.Id = BlockIds.GRASS;
                            else
                                block.Id = BlockIds.STONE;
                        }
                        else
                        {
                            block.Id = BlockIds.STONE;
                        }


                        if (by < 3)
                        {
                            block.Id = BlockIds.BEDROCK;
                        }
                        else if (by < terrainY && string.IsNullOrEmpty(block.Id))
                        {
                            block.Id = BlockIds.STONE;
                        }
                        /**
                        else if (string.IsNullOrEmpty(block.Id))
                        {
                            block.Id = BlockIds.AIR;
                        }
                        **/

                        if (!string.IsNullOrEmpty(block.Id))
                            section.blocks[bx, by - sectionYOffset, bz] = block;
                    }
                }
            }
        }
    }
}
