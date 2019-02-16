using System;
using System.Linq;
using System.Threading;
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
                x = 0,
                y = 0
            };
            ChunkGenerateTask cgt = new ChunkGenerateTask(c);
            cgt.Start();
            while(!cgt.IsDone) {
                Console.WriteLine("Processing");
                Thread.Sleep(100);
            }
            var lines = cgt.chunk.SurfaceMap.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToList();
            lines.ForEach(l => Console.WriteLine(l));

        }
    }
}
