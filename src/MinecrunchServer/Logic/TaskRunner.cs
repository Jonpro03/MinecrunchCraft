using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using minecrunch.models.Chunks;
using minecrunch.tasks;

namespace MinecrunchServer.Logic
{
    public sealed class TaskRunner
    {
        private const int SIMULTANEOUS = 4;
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

        void HandleTerrainGenComplete(object obj)
        {
            Chunk chunk = obj as Chunk;
            BlockFacesTasks.Enqueue(new ChunkCalculateFacesTask(chunk));
        }

        void HandleFaceCalcComplete(object obj)
        {
            Chunk chunk = obj as Chunk;
            SaveChunkTasks.Enqueue(new SaveChunkTask(chunk, "world1"));
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

        private void Task_Finished(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
