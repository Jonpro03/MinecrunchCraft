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
        public long processTimeMs = 0;

        public Block GetBlockByChunkCoord(int x, int y, int z)
        {
            int sectionBlockIsIn = (int) (y / 16.0);
            int ySection = y % 16;

            if ((x>15) || (z>15))
            {
                return null;
            }

            return sections[sectionBlockIsIn]?.blocks[x, ySection, z] ?? null;
        }

        public List<Block> GetAllBlocks()
        {
            List<Block> allBlocks = new List<Block>();
            for (int i=0; i<16; i++)
            {
                if (sections[i] is null) { continue; }
                var s = sections[i];
                allBlocks.AddRange(s.GetAllBlocks());
            }
            return allBlocks;
        }
    }
}
