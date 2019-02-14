using System;

namespace minecrunch.data.models
{
    [Serializable]
    public class ChunkSectionDataModel
    {
        public readonly int sectionNum;
        public readonly BlockDataModel[,,] blocks;
        public ChunkSectionDataModel(int sectionNum, BlockDataModel[,,] blocks)
        {
            this.sectionNum = sectionNum;
            this.blocks = blocks;
        }
    }
}
