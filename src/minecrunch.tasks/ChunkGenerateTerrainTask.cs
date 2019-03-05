using minecrunch.models;
using minecrunch.models.Biomes;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace minecrunch.tasks
{
    public sealed class ChunkGenerateTerrainTask : ThreadedTask
    {
        public readonly Chunk chunk;
        private readonly List<ChunkSection> sections;
        private PerlinNoise pNoise;
        private BlockInfo bInfo;
        public override event ThreadCompleteEventHandler ThreadComplete;
        private const int CAVE_BREAKTHROUGH_LIMIT = 45;
        private Random rand;
        private IEnumerable<Block> blocks;

        public ChunkGenerateTerrainTask(Chunk newChunk)
        {
            chunk = newChunk;
            for (int x = 0; x < chunk.sections.Length; x++)
            {
                chunk.sections[x] = new ChunkSection
                {
                    number = x,
                    name = $"subchunk{x}"
                };
            }
            sections = new List<ChunkSection>(chunk.sections);

            bInfo = BlockInfo.Instance;
            pNoise = PerlinNoise.Instance;
            rand = new Random();
        }

        protected override void ThreadFunction()
        {
            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    chunk.SurfaceMap[bx, bz] = pNoise.Terrain(bx + (chunk.x * 16), bz + (chunk.y * 16));
                }
            }

            Parallel.ForEach(sections, ProcessSection);

            // Generate Oceans here
            Parallel.ForEach(sections, ProcessCaves);

            blocks = chunk.GetAllBlocks().Where(b => b?.Id is BlockIds.STONE);

            AddCoal();
            AddAndesite();
            AddIron();
            AddDiamond();

            AddTrees();

            ThreadComplete(chunk);
        }

        private void ProcessSection(ChunkSection section)
        {
            int sectionYOffset = 16 * section.number;

            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    double terrainY = chunk.SurfaceMap[bx,bz];

                    for (int by = sectionYOffset; by < sectionYOffset + 16; by++)
                    {
                        Block block = new Block
                        {
                            x = (byte)bx,
                            y = (byte)by,
                            z = (byte)bz
                        };

                        // If above the terrain, just mark it air and move on.
                        if (by > terrainY)
                        {
                            //block.Id = BlockIds.AIR;
                            //section.blocks[bx, by - sectionYOffset, bz] = block;
                            continue;
                        }

                        // Ocean / River
                        if (terrainY <= 48)
                        {
                            block.Id = BlockIds.STONE;
                        }
                        // Beach
                        else if (terrainY < 52.5)
                        {
                            if (by > terrainY - 3)
                                block.Id = BlockIds.SAND;
                            else
                                block.Id = BlockIds.STONE;
                        }
                        // Grass
                        else if (terrainY < 60)
                        {
                            if (by == (int) terrainY)
                                block.Id = BlockIds.GRASS;
                            else if (by > terrainY - 2)
                                block.Id = BlockIds.DIRT;
                            else
                                block.Id = BlockIds.STONE;
                        }
                        // Mountain
                        else if (terrainY < 86)
                        {
                            if (by == (int)terrainY)
                                block.Id = BlockIds.GRASS;
                            else
                                block.Id = BlockIds.STONE;
                        }
                        else
                        {
                            block.Id = BlockIds.STONE;
                        }


                        if (by < 3)
                        {
                            block.Id = BlockIds.BEDROCK;
                        }
                        else if (by < terrainY && string.IsNullOrEmpty(block.Id))
                        {
                            block.Id = BlockIds.STONE;
                        }
                        /**
                        else if (string.IsNullOrEmpty(block.Id))
                        {
                            block.Id = BlockIds.AIR;
                        }
                        **/

                        if (!string.IsNullOrEmpty(block.Id))
                            section.blocks[bx, by - sectionYOffset, bz] = block;
                    }
                }
            }
        }

        private void ProcessCaves(ChunkSection section)
        {
            int sectionYOffset = 16 * section.number;

            for (int bx = 0; bx < 16; bx++)
            {
                for (int bz = 0; bz < 16; bz++)
                {
                    int terrainY = (int)chunk.SurfaceMap[bx, bz];

                    for (int by = sectionYOffset; by < sectionYOffset + 16; by++)
                    {
                        // If above the terrain, just move on.
                        int caveYLimit = terrainY < CAVE_BREAKTHROUGH_LIMIT ? CAVE_BREAKTHROUGH_LIMIT : terrainY - 5;
                        if (by > caveYLimit || by < 4) { continue; }

                        if (pNoise.Cave(bx + (chunk.x * 16), by, bz + (chunk.y * 16)))
                        {
                            //section.blocks[bx, by - sectionYOffset, bz].Id = BlockIds.AIR;
                            section.blocks[bx, by - sectionYOffset, bz] = null;
                        }
                    }
                }
            }
        }

        private void AddCoal()
        {
            int numOreVeins = rand.Next(5, 10);

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

        private void AddTrees()
        {
            Random rand = new Random();
            for (var tree = 0; tree < 3; tree++)
            {
                int treeX = rand.Next(0, 9);
                int treeZ = rand.Next(0, 9);
                int treeY = (int)chunk.SurfaceMap[treeX + 3, treeZ + 3];
                int treeHeight = rand.Next(6, 9);

                if (chunk.GetBlockByChunkCoord(treeX, treeY, treeZ)?.Id != BlockIds.GRASS) { return; }
                for (var y = 0; y < treeHeight; y++)
                {
                    var block = chunk.GetBlockByChunkCoord(treeX + 3, treeY + y, treeZ + 3);
                    if (block is null)
                    {
                        block = new Block
                        {
                            x = (byte)(treeX + 3),
                            y = (byte)(treeY + y),
                            z = (byte)(treeZ + 3)
                        };
                    }
                    block.Id = BlockIds.OAK_WOOD;
                    chunk.SetBlock(block);
                }

                for (int x = 0; x < 7; x++)
                {
                    for (int y = treeHeight - 1; y < treeHeight + 3; y++)
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
