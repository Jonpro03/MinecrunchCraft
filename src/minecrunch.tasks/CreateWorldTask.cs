using System;
using minecrunch.models;
using minecrunch.models.Chunks;

namespace minecrunch.tasks
{
    public sealed class CreateWorldTask : ThreadedTask
    {
        public readonly string seed;
        public int worldSpawnX = 0;
        public int worldSpawnY = 0;
        public int worldSpawnZ = 0;
        

        public CreateWorldTask(string seed, string worldName)
        {
            this.chunk = chunk;
            fileName = $"{worldName}/chunks/{chunk.name}";
        }

        protected override void ThreadFunction()
        {
            Serializer.SerializeToFile(chunk, fileName, true);
        }
    }
}

