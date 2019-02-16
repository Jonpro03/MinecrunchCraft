using minecrunch.models.Biomes;
using System;
using System.Collections.Generic;

namespace minecrunch.models.Chunks
{
    [Serializable]
    public class Chunk
    {
        public int x;
        public int y;
        public Biome biome;
        public ChunkSection[] sections = new ChunkSection[16];
        public Dictionary<string, int> SurfaceMap = new Dictionary<string, int>();
    }
}
