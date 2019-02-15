using System;
using minecrunch.models.Blocks;

namespace minecrunch.models.Chunks
{
    [Serializable]
    public class ChunkSection
    {
        public int sectionNum;
        public Block[,,] blocks = new Block[16, 16, 16];
    }
}
