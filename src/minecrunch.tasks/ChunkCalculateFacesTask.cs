using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using minecrunch.parameters.Blocks;
using System.Diagnostics;

namespace minecrunch.tasks
{
    public class ChunkCalculateFacesTask : ThreadedTask
    {
        public readonly Chunk chunk;
        public readonly BlockInfo bInfo;

        private readonly List<ChunkSection> sections;

        public ChunkCalculateFacesTask(Chunk c)
        {
            chunk = c;
            bInfo = BlockInfo.Instance;
            sections = new List<ChunkSection>(chunk.sections);            
        }

        protected override void ThreadFunction()
        {
            var watch = Stopwatch.StartNew();
            Parallel.ForEach(sections, ProcessSection);
            watch.Stop();
            chunk.processTimeMs = watch.ElapsedMilliseconds;
        }

        private void ProcessSection(ChunkSection section)
        {
            int sectionYOffset = 16 * section.number;

            // Check to see if there's only air in this section. If so, we're done.
            IEnumerable<Block> nonAir = from Block block in section.blocks where block.Id != BlockIds.AIR select block;
            if (nonAir.Count() is 0)
            {
                return;                
            }

            foreach (Block block in nonAir)
            {
                int bx = block.x;
                int by = block.y;
                int bz = block.z;
                int yIndex = by - sectionYOffset;

                // Check front, back, left and right to see if there's another visible block there.
                // Adjacent block might not be in this section, so use the chunk's get block method.
                // Top
                if (bInfo.IsTransparent(
                    chunk.GetBlockByChunkCoord(bx, by is 255 ? by : by + 1, bz).Id))
                {
                    section.blocks[bx, yIndex, bz].FacesVisible.top = true;
                }

                // Bottom
                if (bInfo.IsTransparent(
                    chunk.GetBlockByChunkCoord(bx, by is 0 ? by : by - 1, bz).Id))
                {
                    section.blocks[bx, yIndex, bz].FacesVisible.bottom = true;
                }

                // Left
                if (bInfo.IsTransparent(
                    section.blocks[bx is 0 ? bx : bx - 1, yIndex, bz].Id) || bx is 0)
                {
                    section.blocks[bx, yIndex, bz].FacesVisible.left = true;
                }

                // Right
                if (bInfo.IsTransparent(
                    section.blocks[bx is 15 ? bx : bx + 1, yIndex, bz].Id) || bx is 15)
                {
                    section.blocks[bx, yIndex, bz].FacesVisible.right = true;
                }

                // Front
                if (bInfo.IsTransparent(
                    section.blocks[bx, yIndex, bz is 0 ? bz : bz - 1].Id) || bz is 0)
                {
                    section.blocks[bx, yIndex, bz].FacesVisible.front = true;
                }

                // Back
                if (bInfo.IsTransparent(
                    section.blocks[bx, yIndex, bz is 15 ? bz : bz + 1].Id) || bz is 15)
                {
                    section.blocks[bx, yIndex, bz].FacesVisible.back = true;
                }
            }
        }
        private void ProcessSection2(ChunkSection section)
        {
            int sectionYOffset = 16 * section.number;
            /**
            // Check to see if there's only air in this section. If so, we're done.
            IEnumerable<Block> nonAir = from Block block in section.blocks where block.Id != BlockIds.AIR select block;
            if (nonAir.Count() is 0)
            {
                return;                
            }
    **/
            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    for (int by = sectionYOffset; by < sectionYOffset + 16; by++)
                    {
                        int yIndex = by - sectionYOffset;

                        // If we're air, than we're done here
                        if (section.blocks[bx, yIndex, bz].Id is BlockIds.AIR) { continue; }

                        // Check front, back, left and right to see if there's another visible block there.
                        // Adjacent block might not be in this section, so use the chunk's get block method.
                        // Top
                        if (bInfo.IsTransparent(
                            chunk.GetBlockByChunkCoord(bx, by is 255 ? by : by + 1, bz).Id))
                        {
                            section.blocks[bx, yIndex, bz].FacesVisible.top = true;
                        }

                        // Bottom
                        if (bInfo.IsTransparent(
                            chunk.GetBlockByChunkCoord(bx, by is 0 ? by : by - 1, bz).Id))
                        {
                            section.blocks[bx, yIndex, bz].FacesVisible.bottom = true;
                        }

                        // Left
                        if (bInfo.IsTransparent(
                            section.blocks[bx is 0 ? bx : bx - 1, yIndex, bz].Id) || bx is 0)
                        {
                            section.blocks[bx, yIndex, bz].FacesVisible.left = true;
                        }

                        // Right
                        if (bInfo.IsTransparent(
                            section.blocks[bx is 15 ? bx : bx + 1, yIndex, bz].Id) || bx is 15)
                        {
                            section.blocks[bx, yIndex, bz].FacesVisible.right = true;
                        }

                        // Front
                        if (bInfo.IsTransparent(
                            section.blocks[bx, yIndex, bz is 0 ? bz : bz - 1].Id) || bz is 0)
                        {
                            section.blocks[bx, yIndex, bz].FacesVisible.front = true;
                        }

                        // Back
                        if (bInfo.IsTransparent(
                            section.blocks[bx, yIndex, bz is 15 ? bz : bz + 1].Id) || bz is 15)
                        {
                            section.blocks[bx, yIndex, bz].FacesVisible.back = true;
                        }
                    }
                }
            }
        }


    }
}
