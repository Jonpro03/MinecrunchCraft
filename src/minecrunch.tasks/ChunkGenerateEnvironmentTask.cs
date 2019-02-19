using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using minecrunch.utilities;

namespace minecrunch.tasks
{
    public sealed class ChunkGenerateEnvironmentTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private PerlinNoise pNoise;
        private BlockInfo bInfo;

        public ChunkGenerateEnvironmentTask(Chunk c)
        {
            chunk = c;
            bInfo = BlockInfo.Instance;
            pNoise = PerlinNoise.Instance;
        }

        protected override void ThreadFunction()
        {
            AddTrees();
        }

        private void AddTrees()
        {
            Random rand = new Random();
            for (var tree = 0; tree < 2; tree++)
            {
                int treeX = rand.Next(0, 9);
                int treeZ = rand.Next(0, 9);
                int treeY = chunk.SurfaceMap[$"{treeX+3},{treeZ+3}"];
                if (chunk.GetBlockByChunkCoord(treeX, treeY, treeZ).Id != BlockIds.GRASS) { return; }
                for (var y = 0; y < 6; y++)
                {
                    chunk.GetBlockByChunkCoord(treeX+3, treeY + y, treeZ+3).Id = BlockIds.OAK_WOOD;
                }

                for (int x = 0; x < 7; x++)
                {
                    for (int y = 4; y < 7; y++)
                    {
                        for (int z = 0; z < 7; z++)
                        {
                            if (!((x == 0 || x == 6) && (z == 0 || z == 6)))
                                chunk.GetBlockByChunkCoord(treeX+x, treeY+y, treeZ+z).Id = BlockIds.OAK_LEAVES;
                        }
                    }
                }

            }
        }
    }
}
