using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace minecrunch.tasks
{
    public sealed class ChunkGenerateOresTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private BlockInfo bInfo;
        private IEnumerable<Block> blocks;
        private Random rand;

        public ChunkGenerateOresTask(Chunk c)
        {
            chunk = c;
            bInfo = BlockInfo.Instance;
            rand = new Random();
        }

        protected override void ThreadFunction()
        {
            return;
            blocks = chunk.GetAllBlocks().Where(b => b?.Id is BlockIds.STONE);
            AddCoal();
            AddAndesite();
            AddIron();
            AddDiamond();
        }

        private void AddCoal()
        {
            int numOreVeins = rand.Next(5, 10);

            for (int v = 0; v < numOreVeins; v++)
            {
                int randIndex = rand.Next(0, blocks.Count());
                var b = blocks.Skip(randIndex).First();
                int oreX = b.x > 12 ? 12 : b.x;
                int oreY = b.y > 255 ? 255: b.y;
                int oreZ = b.z > 12 ? 12 : b.z;

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreY + y, oreZ + z);
                            //if (block.Id != BlockIds.AIR)
                            if (block != null)
                            {
                                block.Id = BlockIds.COAL_ORE;
                                //chunk.SetBlock(block);
                            }
                        }
                    }
                }
            }
        }

        private void AddIron()
        {
            int numOreVeins = rand.Next(1, 4);

            for (int v = 0; v < numOreVeins; v++)
            {
                int randIndex = rand.Next(0, blocks.Count());
                var b = blocks.Skip(randIndex).First();
                int oreX = b.x > 12 ? 12 : b.x;
                int oreY = b.y > 255 ? 255 : b.y;
                int oreZ = b.z > 12 ? 12 : b.z;

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreY + y, oreZ + z);
                            //if (block.Id != BlockIds.AIR)
                            if (block != null)
                            {
                                block.Id = BlockIds.IRON_ORE;
                                //chunk.SetBlock(block);
                            }
                        }
                    }
                }
            }
        }

        private void AddDiamond()
        {
            int numOreVeins = rand.Next(2, 4);
            for (var ore = 0; ore < numOreVeins; ore++)
            {
                int randIndex = rand.Next(0, blocks.Count());
                var b = blocks.Skip(randIndex).First();
                int oreX = b.x > 12 ? 12 : b.x;
                int oreY = b.y > 255 ? 255 : b.y;
                int oreZ = b.z > 12 ? 12 : b.z;
                if (chunk.GetBlockByChunkCoord(oreX, oreY, oreZ)?.Id != BlockIds.STONE) { continue; }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreY + y, oreZ + z);
                            //if (block.Id != BlockIds.AIR)
                            if (block != null)
                            {
                                block.Id = BlockIds.DIAMOND_ORE;
                                //chunk.SetBlock(block);
                            }

                        }
                    }
                }

            }
        }

        private void AddAndesite()
        {
            int numOreVeins = rand.Next(2, 8);
            for (var ore = 0; ore < numOreVeins; ore++)
            {
                int randIndex = rand.Next(0, blocks.Count());
                var b = blocks.Skip(randIndex).First();
                int oreX = b.x > 12 ? 12 : b.x;
                int oreY = b.y > 255 ? 255 : b.y;
                int oreZ = b.z > 12 ? 12 : b.z;
                if (chunk.GetBlockByChunkCoord(oreX, oreY, oreZ)?.Id != BlockIds.STONE) { continue; }

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreY + y, oreZ + z);
                            //if (block.Id != BlockIds.AIR)
                            if (block != null)
                            {
                                block.Id = BlockIds.ANDESITE;
                                //chunk.SetBlock(block);
                            }
                        }
                    }
                }

            }
        }
    }
}
