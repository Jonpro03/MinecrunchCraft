using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using minecrunch.models.Chunks;
using MinecrunchServer.Logic;

namespace MinecrunchServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChunkController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post(string dimId, int x, int y)
        {
            ChunkTaskManager ctm = new ChunkTaskManager();
            Chunk chunk = new Chunk
            {
                name = $"chunk{x},{y}",
                x = x,
                y = y
            };
            ctm.AddGenerateJob(chunk);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
