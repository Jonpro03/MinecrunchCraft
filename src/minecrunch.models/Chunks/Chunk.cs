using System;
namespace minecrunch.models.Chunks
{
    [Serializable]
    public class Chunk
    {
        public int x;
        public int y;
        public float biome;
        public ChunkSection[] sections = new ChunkSection[16];
    }
}
