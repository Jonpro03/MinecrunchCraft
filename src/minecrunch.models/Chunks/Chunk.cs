using minecrunch.models.Biomes;
using minecrunch.models.Blocks;
using System;
using System.Collections.Generic;

namespace minecrunch.models.Chunks
{
    [Serializable]
    public class Chunk
    {
        public string name;
        public int x;
        public int y;
        public Biome biome;
        public ChunkSection[] sections = new ChunkSection[16];
        public Dictionary<string, int> SurfaceMap = new Dictionary<string, int>();

        public Block GetBlockByChunkCoord(int x, int y, int z)
        {
            int sectionBlockIsIn = (int) (y / 16.0);
            int ySection = y % 16;
            return sections[sectionBlockIsIn]?.blocks[x, ySection, z] ?? null;
        }
    }
}
