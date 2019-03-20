using System;
using System.Collections.Concurrent;
using minecrunch.tasks;

namespace MinecrunchServer.Logic
{
    public sealed class TaskQueues
    {
        public ConcurrentQueue<ChunkGenerateTerrainTask> TerrainTasks { get; set; }
        public ConcurrentQueue<ChunkCalculateFacesTask> BlockFacesTasks { get; set; }
        public ConcurrentQueue<SaveChunkTask> SaveChunkTasks { get; set; }

        private static readonly Lazy<TaskQueues> lazy = new Lazy<TaskQueues>(() => new TaskQueues());
        public static TaskQueues Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private TaskQueues()
        {
            TerrainTasks = new ConcurrentQueue<ChunkGenerateTerrainTask>();
            BlockFacesTasks = new ConcurrentQueue<ChunkCalculateFacesTask>();
            SaveChunkTasks = new ConcurrentQueue<SaveChunkTask>();
        }
    }
}
