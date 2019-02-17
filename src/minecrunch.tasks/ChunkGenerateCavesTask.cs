using System.Collections.Generic;
using System.Threading.Tasks;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using minecrunch.utilities;

namespace minecrunch.tasks
{
    public sealed class ChunkGenerateCavesTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private PerlinNoise pNoise;
        private BlockInfo bInfo;

        public ChunkGenerateCavesTask(Chunk c)
        {
            chunk = c;
            bInfo = BlockInfo.Instance;
            pNoise = PerlinNoise.Instance;
        }

        protected override void ThreadFunction()
        {
            Parallel.ForEach(chunk.sections, ProcessSection);
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
                        // If above the terrain, just move on.
                        if (by > 45 || by < 4) { continue; }

                        if (pNoise.Cave(bx + (chunk.x * 16), by, bz + (chunk.y * 16)))
                        {
                            section.blocks[bx, by - sectionYOffset, bz].Id = BlockIds.AIR;
                        }
                    }
                }
            }
        }
    }
}
