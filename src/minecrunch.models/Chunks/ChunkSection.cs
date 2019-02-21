using System;
using System.Collections.Generic;
using System.Linq;
using minecrunch.models.Blocks;
using UnityEngine;

namespace minecrunch.models.Chunks
{
    [Serializable]
    public class ChunkSection
    {
        public string name;
        public int number;
        public Block[,,] blocks = new Block[16, 16, 16];

        public Dictionary<int, string> Materials = new Dictionary<int, string>();

        public Dictionary<int, List<int>> Triangles = new Dictionary<int, List<int>>();

        public List<Vector3> Verticies = new List<Vector3>();

        public List<Vector2> UVs = new List<Vector2>();

        public List<Block> GetAllBlocks()
        {
            return blocks.Cast<Block>().ToList();
        }
    }
}
