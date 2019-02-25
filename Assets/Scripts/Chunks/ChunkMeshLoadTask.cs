using minecrunch.tasks;
using minecrunch.models;
using minecrunch.models.Chunks;
using minecrunch.models.Runtime;

namespace Assets.Scripts.Chunks
{
    public sealed class ChunkMeshLoadTask : ThreadedTask
    {
        private Chunk chunk;
        private readonly string name;

        public ChunkMeshLoadTask(string fileName)
        {
            name = fileName;
        }

        protected override void ThreadFunction()
        {
            chunk = Serializer.DeserializeFromFile<Chunk>(name); 
        }
    }
}
