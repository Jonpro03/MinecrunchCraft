using System;
namespace minecrunch.data.models
{
    [Serializable]
    public class ChunkDataModel
    {
        public readonly int x;
        public readonly int y;
        public readonly float biome;
        public readonly ChunkSectionDataModel[] sections;

        public ChunkDataModel(int x, int y, float biome, ChunkSectionDataModel[] sections)
        {
            this.x = x;
            this.y = y;
            this.biome = biome;
            this.sections = sections;
        }
    }
}
