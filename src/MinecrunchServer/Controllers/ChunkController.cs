using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using minecrunch.models;
using minecrunch.models.Biomes;
using minecrunch.models.Chunks;
using minecrunch.tasks;
using MinecrunchServer.Logic;

namespace MinecrunchServer.Controllers
{
    [Route("api/chunk")]
    [ApiController]
    public class ChunkController : ControllerBase
    {
        public TaskRunner trInstance = TaskRunner.Instance;
        
        // GET api/chunk
        [HttpGet]
        public string Get()
        {
            return $"{trInstance.TerrainTasks.Count} + {trInstance.BlockFacesTasks.Count}";
        }

        // GET api/chunk/world1/0/0 
        [HttpGet("{world}/{x}/{y}")]
        public async Task<IActionResult> Get(string world, int x, int y)
        {
            string chunkName = $"chunk{x},{y}";
            string filePath = $"{world}/chunks/{chunkName}";
            (new FileInfo(filePath)).Directory.Create();

            // Chunk chunk = Program.ChunkCache.FirstOrDefault(c => c.name == chunkName);
            
            // Reading chunks out of the cache when compression is enabled is super broken for some reason.
            // Force reading off disk.

            Chunk chunk = null;
            if (chunk is null)
            {
                if (System.IO.File.Exists(filePath))
                {
                    var fStream = System.IO.File.OpenRead(filePath);
                    return File(fStream, "application/octet-stream");
                }
            }
            else
            {
                Serializer.SerializeToStream(chunk, out Stream s, true);
                s.Position = 0;
                return File(s, "application/octet-stream");
            }
            
            // New chunk. Generate it.
            chunk = new Chunk()
            {
                name = chunkName,
                x = x,
                y = y,
                biome = Biome.Desert // Todo: this, obviously
            };
            trInstance.TerrainTasks.Enqueue(new ChunkGenerateTerrainTask(chunk, world));
            return NotFound();
        }
    }
}
