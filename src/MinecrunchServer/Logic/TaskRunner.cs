using System;
using System.Collections.Concurrent;
using minecrunch.tasks;

namespace MinecrunchServer.Logic
{
    public sealed class TaskRunner
    {
        private const int SIMULTANEOUS = 4; // Todo: use this
        private static readonly Lazy<TaskRunner> lazy = new Lazy<TaskRunner>(() => new TaskRunner());

        public static TaskRunner Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public ConcurrentQueue<ChunkGenerateTerrainTask> TerrainTasks { get; set; }
        public ConcurrentQueue<ChunkCalculateFacesTask> BlockFacesTasks { get; set; }
        public ConcurrentQueue<SaveChunkTask> SaveChunkTasks { get; set; }

        private TaskRunner()
        {
            TerrainTasks = new ConcurrentQueue<ChunkGenerateTerrainTask>();
            BlockFacesTasks = new ConcurrentQueue<ChunkCalculateFacesTask>();
            SaveChunkTasks = new ConcurrentQueue<SaveChunkTask>();
        }

        // Terrain Gen -> Calc Faces
        void HandleTerrainGenComplete(object obj)
        {
            var task = obj as ChunkGenerateTerrainTask;
            BlockFacesTasks.Enqueue(new ChunkCalculateFacesTask(task.chunk, task.worldName));
        }

        // Calc Faces -> Save to Disk
        void HandleFaceCalcComplete(object obj)
        {
            var task = obj as ChunkCalculateFacesTask;
            SaveChunkTasks.Enqueue(new SaveChunkTask(task.chunk, task.worldName));
        }

        public void UpdateQueues()
        {
            while (TerrainTasks.TryDequeue(out ChunkGenerateTerrainTask task))
            {
                task.ThreadComplete += HandleTerrainGenComplete;
                task.Start();                
            }

            while (BlockFacesTasks.TryDequeue(out ChunkCalculateFacesTask task))
            {
                task.ThreadComplete += HandleFaceCalcComplete;
                task.Start();
            }

            while (SaveChunkTasks.TryDequeue(out SaveChunkTask task))
            {
                task.Start();
            }
        }
    }
}
