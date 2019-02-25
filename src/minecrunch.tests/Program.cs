using System;
using System.Linq;
using System.Threading;
using minecrunch.models.Blocks;
using minecrunch.models.Chunks;
using minecrunch.parameters.Blocks;
using minecrunch.tasks;

namespace minecrunch.tests
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var blockdata = BlockInfo.Instance.GetAllBlockData();
            Console.WriteLine(blockdata.Count);

            Chunk c = new Chunk()
            {
                name = "chunk0,0",
                x = 0,
                y = 0
            };
            ChunkGenerateTerrainTask cgt = new ChunkGenerateTerrainTask(c);
            cgt.Start();
            while (!cgt.IsDone) {
                //Console.WriteLine("Processing Terrain");
                Thread.Sleep(100);
            }

            int numGrass = 0;
            int numStone = 0;
            int numBR = 0;
            for (var x = 0; x < 16; x++)
            {
                for (var y = 0; y < 16; y++)
                {
                    for (var z = 0; z < 16; z++)
                    {
                        if (cgt.chunk.sections[3].blocks[x, y, z] is null) { continue; }
                        string id = cgt.chunk.sections[3].blocks[x, y, z].Id;
                        if (id is BlockIds.GRASS) { numGrass++; }
                        if (id is BlockIds.STONE) { numStone++; }
                        if (id is BlockIds.BEDROCK) { numBR++; }
                    }
                }
            }
            Console.WriteLine($"Grass: {numGrass}");
            Console.WriteLine($"Stone: {numStone}");
            Console.WriteLine($"Bedrock: {numBR}");

            ChunkGenerateCavesTask cgct = new ChunkGenerateCavesTask(c);
            cgct.Start();
            while (!cgct.IsDone)
            {
                Thread.Sleep(10);
            }

            ChunkGenerateOresTask ores = new ChunkGenerateOresTask(c);
            ores.Start();
            while (!ores.IsDone)
            {
                Thread.Sleep(10);
            }

            ChunkGenerateEnvironmentTask env = new ChunkGenerateEnvironmentTask(c);
            env.Start();
            while (!env.IsDone) { Thread.Sleep(100); }

            int startTime = 0;
            ChunkCalculateFacesTask faces = new ChunkCalculateFacesTask(c);
            faces.Start();
            while (!faces.IsDone)
            {
                startTime++;
                Thread.Sleep(10);
            }
            Console.WriteLine($"Took {startTime/100.0f} seconds to finish faces.");

            startTime = 0;
            ChunkCalcVerticiesTask verts = new ChunkCalcVerticiesTask(c);
            verts.Start();
            while(!verts.IsDone)
            {
                startTime++;
                Thread.Sleep(10);
            }
            Console.WriteLine($"Took {startTime / 100.0f} seconds to finish verticies.");
            Console.WriteLine($"Num Verticies {verts.chunk.sections[0].Mesh.Verticies.Count}");


            Thread.Sleep(2);

        }
    }
}
