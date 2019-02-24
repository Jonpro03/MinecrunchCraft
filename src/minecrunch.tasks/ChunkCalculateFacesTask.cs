using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
                    section.blocks[bx, yIndex, bz].SetFaceVisible(Sides.Top, true);
                }

                // Bottom
                if (bInfo.IsTransparent(
                    chunk.GetBlockByChunkCoord(bx, by is 0 ? by : by - 1, bz).Id))
                {
                    section.blocks[bx, yIndex, bz].SetFaceVisible(Sides.Bottom, true);
                }

                // Left
                if (bx is 0 || bInfo.IsTransparent(
                    section.blocks[bx is 0 ? bx : bx - 1, yIndex, bz].Id))
                {
                    section.blocks[bx, yIndex, bz].SetFaceVisible(Sides.Left, true);
                }

                // Right
                if (bx is 15 || bInfo.IsTransparent(
                    section.blocks[bx is 15 ? bx : bx + 1, yIndex, bz].Id))
                {
                    section.blocks[bx, yIndex, bz].SetFaceVisible(Sides.Right, true);
                }

                // Front
                if (bz is 0 || bInfo.IsTransparent(
                    section.blocks[bx, yIndex, bz is 0 ? bz : bz - 1].Id))
                {
                    section.blocks[bx, yIndex, bz].SetFaceVisible(Sides.Front, true);
                }

                // Back
                if (bz is 15 || bInfo.IsTransparent(
                    section.blocks[bx, yIndex, bz is 15 ? bz : bz + 1].Id))
                {
                    section.blocks[bx, yIndex, bz].SetFaceVisible(Sides.Back, true);
                }
            }
        }

    }
}
