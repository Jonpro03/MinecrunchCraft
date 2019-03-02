using System;
using minecrunch.models;
using minecrunch.models.Chunks;

namespace minecrunch.tasks
{
    public sealed class SaveChunkTask : ThreadedTask
    {
        private readonly Chunk chunk;
        private readonly string fileName;

        public SaveChunkTask(Chunk chunk, string worldName)
        {
            this.chunk = chunk;
            fileName = $"{worldName}/chunks/{chunk.name}";
        }

        protected override void ThreadFunction()
        {
            Serializer.SerializeToFile(chunk, fileName);
        }
    }
}
