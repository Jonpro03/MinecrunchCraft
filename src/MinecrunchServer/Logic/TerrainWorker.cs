using System;
using minecrunch.tasks;

namespace MinecrunchServer.Logic
{
    public sealed class TerrainWorker
    {
        public TaskQueues queues = TaskQueues.Instance;
        private int runningTasks = 0;
        private readonly int threading;

        public TerrainWorker(int threads)
        {
            threading = threads;
        }

        // Terrain Gen -> Calc Faces
        void HandleTerrainGenComplete(object obj)
        {
            var task = obj as ChunkGenerateTerrainTask;
            if (task.e is null)
            {
                queues.BlockFacesTasks.Enqueue(new ChunkCalculateFacesTask(task.chunk, task.worldName));
            }
            else
            {
                Console.WriteLine(task.e.StackTrace);
            }
            runningTasks--;
        }

        public void Run()
        {
            while(!queues.TerrainTasks.IsEmpty && runningTasks < threading)
            {
                if (queues.TerrainTasks.TryDequeue(out ChunkGenerateTerrainTask task))
                {
                    task.ThreadComplete += HandleTerrainGenComplete;
                    task.Start();
                    runningTasks++;
                }
            }
        }
    }
}
