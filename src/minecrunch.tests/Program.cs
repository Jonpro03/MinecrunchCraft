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
            while(!cgt.IsDone) {
                //Console.WriteLine("Processing Terrain");
                Thread.Sleep(100);
            }
            var lines = cgt.chunk.SurfaceMap.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToList();
            lines.ForEach(l => Console.WriteLine(l));
            int numGrass = 0;
            int numStone = 0;
            int numBR = 0;
            for(var x = 0; x<16; x++)
            {
                for (var y=0; y<16; y++)
                {
                    for (var z=0; z<16; z++)
                    {
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
                //Console.WriteLine("Processing Caves");
                Thread.Sleep(100);
            }

            ChunkCalculateFacesTask faces = new ChunkCalculateFacesTask(c);
            faces.Start();
            while (!faces.IsDone)
            {
                //Console.WriteLine("Processing Faces");
                Thread.Sleep(100);
            }

            ChunkCalcVerticiesTask verts = new ChunkCalcVerticiesTask(c);
            verts.Start();
            while(!verts.IsDone)
            {
                //Console.WriteLine("Processing Verts");
                Thread.Sleep(100);
            }
            var things = verts.chunk.sections.Select(x => x.Materials.Count).ToList();
            things.ForEach(t => Console.WriteLine(t));

            Thread.Sleep(2);
        }
    }
}
