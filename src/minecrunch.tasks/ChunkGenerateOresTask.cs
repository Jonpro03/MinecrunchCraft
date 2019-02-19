using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using minecrunch.utilities;

namespace minecrunch.tasks
{
    public sealed class ChunkGenerateOresTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private BlockInfo bInfo;

        public ChunkGenerateOresTask(Chunk c)
        {
            chunk = c;
            bInfo = BlockInfo.Instance;
        }

        protected override void ThreadFunction()
        {
            AddCoal();
            AddAndesite();
            AddIron();
            AddDiamond();
        }

        private void AddCoal()
        {
            Random rand = new Random();
            for (var ore = 0; ore < 30; ore++)
            {
                int oreX = rand.Next(0, 12);
                int oreY = rand.Next(0, 12);
                int oreZ = rand.Next(8, chunk.SurfaceMap[$"{oreX+2},{oreY+2}"]);
                if (chunk.GetBlockByChunkCoord(oreX, oreZ, oreY).Id != BlockIds.STONE) { continue; }

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreZ + y, oreY + z);
                            if (block.Id != BlockIds.AIR)
                                chunk.GetBlockByChunkCoord(oreX+x, oreZ+y, oreY+z).Id = BlockIds.COAL_ORE;
                        }
                    }
                }

            }
        }

        private void AddIron()
        {
            Random rand = new Random();
            for (var ore = 0; ore < 40; ore++)
            {
                int oreX = rand.Next(0, 12);
                int oreY = rand.Next(0, 12);
                int oreZ = rand.Next(8, chunk.SurfaceMap[$"{oreX + 2},{oreY + 2}"]);
                if (chunk.GetBlockByChunkCoord(oreX, oreZ, oreY).Id != BlockIds.STONE) { continue; }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreZ + y, oreY + z);
                            if (block.Id != BlockIds.AIR)
                                chunk.GetBlockByChunkCoord(oreX + x, oreZ + y, oreY + z).Id = BlockIds.IRON_ORE;
                        }
                    }
                }

            }
        }

        private void AddDiamond()
        {
            Random rand = new Random();
            for (var ore = 0; ore < 40; ore++)
            {
                int oreX = rand.Next(0, 12);
                int oreY = rand.Next(0, 12);
                int oreZ = rand.Next(8, chunk.SurfaceMap[$"{oreX + 2},{oreY + 2}"]);
                if (chunk.GetBlockByChunkCoord(oreX, oreZ, oreY).Id != BlockIds.STONE) { continue; }

                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreZ + y, oreY + z);
                            if (block.Id != BlockIds.AIR)
                                chunk.GetBlockByChunkCoord(oreX + x, oreZ + y, oreY + z).Id =  BlockIds.DIAMOND_ORE;
                        }
                    }
                }

            }
        }

        private void AddAndesite()
        {
            Random rand = new Random();
            for (var ore = 0; ore < 30; ore++)
            {
                int oreX = rand.Next(0, 12);
                int oreY = rand.Next(0, 12);
                int oreZ = rand.Next(8, chunk.SurfaceMap[$"{oreX + 2},{oreY + 2}"]);
                if (chunk.GetBlockByChunkCoord(oreX, oreZ, oreY).Id != BlockIds.STONE) { continue; }

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            Block block = chunk.GetBlockByChunkCoord(oreX + x, oreZ + y, oreY + z);
                            if (block.Id != BlockIds.AIR)
                                chunk.GetBlockByChunkCoord(oreX + x, oreZ + y, oreY + z).Id = BlockIds.ANDESITE;
                        }
                    }
                }

            }
        }
    }
}
