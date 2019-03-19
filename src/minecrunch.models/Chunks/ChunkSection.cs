using System;
using System.Collections.Generic;
using System.Linq;
using minecrunch.models.Blocks;
using minecrunch.models.Runtime;

namespace minecrunch.models.Chunks
{
    [Serializable]
    public class ChunkSection
    {
        public string name;
        public int number;
        public Block[,,] blocks = new Block[16, 16, 16];
        public SerializableMesh Mesh { get; set; }

        public ChunkSection()
        {
            Mesh = new SerializableMesh();
        }

        public List<Block> GetAllBlocks()
        {
            return blocks.Cast<Block>().ToList();
        }
    }
}
