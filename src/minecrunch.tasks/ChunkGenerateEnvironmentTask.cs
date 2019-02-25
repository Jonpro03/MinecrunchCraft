using System;
using minecrunch.models;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;

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
            for (var tree = 0; tree < 3; tree++)
            {
                int treeX = rand.Next(0, 9);
                int treeZ = rand.Next(0, 9);
                int treeY = chunk.SurfaceMap[treeX+3,treeZ+3];
                int treeHeight = rand.Next(6, 9);

                if (chunk.GetBlockByChunkCoord(treeX, treeY, treeZ)?.Id != BlockIds.GRASS) { return; }
                for (var y = 0; y < treeHeight; y++)
                {
                    var block = chunk.GetBlockByChunkCoord(treeX + 3, treeY + y, treeZ + 3);
                    if (block is null)
                    {
                        block = new Block
                        {
                            x = (byte) (treeX + 3),
                            y = (byte) (treeY + y),
                            z = (byte) (treeZ + 3)
                        };
                    }
                    block.Id = BlockIds.ACACIA_WOOD;
                    chunk.SetBlock(block);
                }

                for (int x = 0; x < 7; x++)
                {
                    for (int y = treeHeight-1; y < treeHeight+3; y++)
                    {
                        for (int z = 0; z < 7; z++)
                        {
                            if (!((x == 0 || x == 6) && (z == 0 || z == 6)))
                            {
                                var block = chunk.GetBlockByChunkCoord(treeX + x, treeY + y, treeZ + z);
                                if (block is null)
                                {
                                    block = new Block
                                    {
                                        x = (byte)(treeX + x),
                                        y = (byte)(treeY + y),
                                        z = (byte)(treeZ + z)
                                    };
                                }
                                block.Id = BlockIds.OAK_LEAVES;
                                chunk.SetBlock(block);
                            }
                        }
                    }
                }

            }
        }
    }
}
