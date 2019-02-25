using minecrunch.models;
using minecrunch.models.Chunks;
using minecrunch.models.Runtime;
using minecrunch.tasks;

namespace Assets.Scripts.Chunks
{
    public sealed class ChunkMeshSaveTask : ThreadedTask
    {
        private readonly Chunk chunk;
        private readonly string name;

        public ChunkMeshSaveTask(Chunk chunk, string fileName)
        {
            this.chunk = chunk;
            name = fileName;
        }

        protected override void ThreadFunction()
        {

            //SerializableMesh[] meshes = new SerializableMesh[16];
            //for (var x=0; x<16; x++)
            //{
            //meshes[x] = chunk.sections[x].Mesh;
            //}

            //Serializer.SerializeToFile(meshes, name);

            Serializer.SerializeToFile(chunk, name);
        }
    }
}
