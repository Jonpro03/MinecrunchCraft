using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using minecrunch.models.Chunks;
using minecrunch.tasks;

namespace MinecrunchServer.Logic
{
    public sealed class TaskRunner
    {
        private const int SIMULTANEOUS = 2;
        private static readonly Lazy<TaskRunner> lazy = new Lazy<TaskRunner>(() => new TaskRunner());
        private static bool queueLock = false;
        private static bool taskLock = false;

        public static TaskRunner Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public List<ChunkGenerateTerrainTask> TerrainTasks { get; set; }
        public List<ChunkGenerateOceansTask> OceansTasks { get; set; }
        public List<ChunkGenerateCavesTask> CavesTasks { get; set; }
        public List<ChunkGenerateOresTask> OresTasks { get; set; }
        public List<ChunkGenerateEnvironmentTask> EnvironmentTasks { get; set; }
        public List<ChunkCalculateFacesTask> BlockFacesTasks { get; set; }
        public List<SaveChunkTask> SaveChunkTasks { get; set; }

        private TaskRunner()
        {
            TerrainTasks = new List<ChunkGenerateTerrainTask>();
            OceansTasks = new List<ChunkGenerateOceansTask>();
            CavesTasks = new List<ChunkGenerateCavesTask>();
            OresTasks = new List<ChunkGenerateOresTask>();
            EnvironmentTasks = new List<ChunkGenerateEnvironmentTask>();
            BlockFacesTasks = new List<ChunkCalculateFacesTask>();
            SaveChunkTasks = new List<SaveChunkTask>();

        }

        public void UpdateQueues(object source, ElapsedEventArgs e)
        {
            if (queueLock || taskLock) { return; }
            queueLock = true;

            // Terrain -> Oceans
            foreach (var job in TerrainTasks.Where(t => t.IsDone))
            {
                var nextTask = new ChunkGenerateOceansTask(job.chunk);
                OceansTasks.Add(nextTask);
                nextTask.Start();
            }
            TerrainTasks.RemoveAll(task => task.IsDone);

            // Oceans -> Caves
            foreach (var job in OceansTasks.Where(t => t.IsDone))
            {
                var nextTask = new ChunkGenerateCavesTask(job.chunk);
                CavesTasks.Add(nextTask);
                nextTask.Start();
            }
            OceansTasks.RemoveAll(task => task.IsDone);

            // Caves -> Ores
            foreach (var job in CavesTasks.Where(t => t.IsDone))
            {
                var nextTask = new ChunkGenerateOresTask(job.chunk);
                OresTasks.Add(nextTask);
                nextTask.Start();
            }
            CavesTasks.RemoveAll(task => task.IsDone);

            // Ores -> Environment
            foreach (var job in OresTasks.Where(t => t.IsDone))
            {
                var nextTask = new ChunkGenerateEnvironmentTask(job.chunk);
                EnvironmentTasks.Add(nextTask);
                nextTask.Start();
            }
            OresTasks.RemoveAll(task => task.IsDone);

            // Environment -> Faces
            foreach (var job in EnvironmentTasks.Where(t => t.IsDone))
            {
                var nextTask = new ChunkCalculateFacesTask(job.chunk);
                BlockFacesTasks.Add(nextTask);
            }
            EnvironmentTasks.RemoveAll(task => task.IsDone);

            // Faces -> Cache
            Program.ChunkCache.AddRange(BlockFacesTasks.Where(t => t.IsDone).Select(t => t.chunk));
            foreach (var b in BlockFacesTasks.Where(t => t.IsDone))
            {
                new SaveChunkTask(b.chunk, "world1").Start();
            }
            BlockFacesTasks.RemoveAll(task => task.IsDone);

            queueLock = false;
        }

        public void RunTasks(object source, ElapsedEventArgs e)
        {
            if (taskLock || queueLock) { return; }
            taskLock = true;
            foreach (var task in TerrainTasks.Take(SIMULTANEOUS)) { task.Start(); }
            // Only run full speed on face calculations if the terrain gen queue is empty
            int faceThreads = TerrainTasks.Count.Equals(0) ? 1 : SIMULTANEOUS;
            int neededFaceThreads = faceThreads - BlockFacesTasks.Count(t => t.IsRunning);
            foreach (var task in BlockFacesTasks.Take(neededFaceThreads)) { task.Start(); }

            taskLock = false;
        }
    }
}
