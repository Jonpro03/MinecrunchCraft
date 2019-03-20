using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using minecrunch.models;
using minecrunch.models.Biomes;
using minecrunch.models.Chunks;
using minecrunch.models.Generator;
using minecrunch.models.World;
using minecrunch.tasks;
using MinecrunchServer.Logic;

namespace MinecrunchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorldController : ControllerBase
    {
        private TaskQueues queues = TaskQueues.Instance;

        // POST: api/World
        [HttpPost]
        public void Post(string name, string seed)
        {
            if (Directory.Exists(name))
            {
                return;
            }

            // Create Model
            var world = new World
            {
                Seed = seed
            };

            var genSettings = new WorldGenerationSettings
            {
                Seed = seed.GetHashCode()
            };
            PerlinNoise.Instance.SetWorldGeneratorSettings(genSettings);

            // Create world folder
            (new FileInfo(name+"/chunks/")).Directory.Create();

            // Pregenerate chunks
            for (int x=-10; x<=10; x++)
            {
                for (int y=-10; y<=10; y++)
                {
                    string chunkName = $"chunk{x},{y}";
                    var chunk = new Chunk()
                    {
                        name = chunkName,
                        x = x,
                        y = y,
                        biome = Biome.Desert
                    };
                    queues.TerrainTasks.Enqueue(new ChunkGenerateTerrainTask(chunk, name));
                }
            }


            // Choose spawn chunk

            // Serialize World Model
        }
    }
}
