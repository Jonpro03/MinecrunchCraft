using minecrunch.tasks;

namespace MinecrunchServer.Logic
{
    public sealed class ChunkSaveWorker
    {
        public TaskQueues queues = TaskQueues.Instance;

        public void Run()
        {
            while (queues.SaveChunkTasks.TryDequeue(out SaveChunkTask task))
            {
                task.Start();
            }
        }
    }
}
