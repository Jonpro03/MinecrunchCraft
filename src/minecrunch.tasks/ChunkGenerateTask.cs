using System.Collections.Generic;
using System.Threading.Tasks;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;

namespace minecrunch.tasks
{
    public sealed class ChunkGenerateTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private readonly List<ChunkSection> sections;


        public ChunkGenerateTask(Chunk chunk)
        {
            this.chunk = chunk;
            for (int x=0; x<chunk.sections.Length; x++)
            {
                chunk.sections[x].sectionNum = x;
            }
            sections = new List<ChunkSection>(chunk.sections);
        }

        protected override void ThreadFunction()
        {
            Parallel.ForEach(sections, ProcessSection);
        }

        private void ProcessSection(ChunkSection section)
        {
            int sectionYOffset = 16 * section.sectionNum;
            for (int bx = 0; bx < 16; bx++)
            {

            }
        }
    }
}
