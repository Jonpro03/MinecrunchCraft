using System;
using minecrunch.tasks;

namespace MinecrunchServer.Logic
{
    public sealed class FaceCalcWorker
    {
        public TaskQueues queues = TaskQueues.Instance;
        private int runningTasks = 0;
        private readonly int threading;

        public FaceCalcWorker(int threads)
        {
            threading = threads;
        }

        // Calc Faces -> Save to Disk
        void HandleFaceCalcComplete(object obj)
        {
            var task = obj as ChunkCalculateFacesTask;
            if (task.e is null)
            {
                queues.SaveChunkTasks.Enqueue(new SaveChunkTask(task.chunk, task.worldName));
            }
            else
            {
                Console.WriteLine(task.e.StackTrace);
            }
            runningTasks--;
        }

        public void Run()
        {
            while(queues.BlockFacesTasks.Count > 0 && runningTasks < threading)
            {
                if (queues.BlockFacesTasks.TryDequeue(out ChunkCalculateFacesTask task))
                {
                    task.ThreadComplete += HandleFaceCalcComplete;
                    task.Start();
                    runningTasks++;
                }
            }
        }
    }
}
