using minecrunch.models.Biomes;
using minecrunch.models.Blocks;
using System;
using System.Collections.Generic;

namespace minecrunch.models.Chunks
{
    [Serializable]
    public class Chunk
    {
        public long lastUpdated;
        public string name;
        public int x;
        public int y;
        public Biome biome;
        public ChunkSection[] sections = new ChunkSection[16];
        public double[,] SurfaceMap = new double[16, 16];
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

        public void SetBlock(Block block)
        {
            int sectionBlockIsIn = (int)(block.y / 16.0);
            int ySection = block.y % 16;

            // Empty (air) sections will be null.
            if (sections[sectionBlockIsIn] is null)
            {
                sections[sectionBlockIsIn] = new ChunkSection();
            }
            sections[sectionBlockIsIn].blocks[block.x, ySection, block.z] = block;
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
