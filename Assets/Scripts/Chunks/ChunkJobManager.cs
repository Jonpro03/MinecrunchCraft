using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.World;
using minecrunch.models.Chunks;
using minecrunch.tasks;

namespace Assets.Scripts.Chunks
{
    // Todo: switch to a singleton
    public class ChunkJobManager
    {
        // Todo: Switch to ConcurrentQueues instead of lists to match the server
        // Queues
        public List<ChunkDownloadTask> ChunkDownloads { get; set; }
        public List<ChunkCalcVerticiesTask> ChunkCalcVerticiesTasks { get; set; }
        public List<Chunk> CompletedChunks { get; set; }

        public ChunkJobManager()
        {
            ChunkDownloads = new List<ChunkDownloadTask>();
            ChunkCalcVerticiesTasks = new List<ChunkCalcVerticiesTask>();
            CompletedChunks = new List<Chunk>();
        }

        /// <summary>
        /// Manage the task queues.
        /// </summary>
        public void Update()
        {
            // Check for completed chunk downloads.
            foreach (var task in ChunkDownloads.Where(t => t.IsDone))
            {
                if (task.e != null)
                {
                    Debug.LogException(task.e);
                    WorldTerrain.InProgressChunks.Remove(new Vector2Int(task.cx,task.cy));
                    continue;
                }
                if (task.chunk is null)
                {
                    WorldTerrain.InProgressChunks.Remove(new Vector2Int(task.cx, task.cy));
                    Debug.LogWarning($"Failed to deserialize {task.cx}, {task.cy}");
                    continue;
                }
                ChunkCalcVerticiesTasks.Add(new ChunkCalcVerticiesTask(task.chunk));
            }
            ChunkDownloads.RemoveAll(task => task.IsDone);

            // Check for completed chunk verticies calculations.
            foreach (var task in ChunkCalcVerticiesTasks.Where(t => t.IsDone))
            {
                if (task.e != null)
                {
                    WorldTerrain.InProgressChunks.Remove(new Vector2Int(task.chunk.x, task.chunk.y));
                    Debug.LogException(task.e);
                    continue;
                }

                CompletedChunks.Add(task.chunk);
            }
            ChunkCalcVerticiesTasks.RemoveAll(task => task.IsDone);

            //Todo: Better task management. Check that total running tasks won't exceed desired parallelization.
            int parallel = 3;
            foreach (var task in ChunkDownloads.Take(10)) { task.Start(); }
            foreach (var task in ChunkCalcVerticiesTasks.Take(parallel)) { task.Start(); }
        }

        public void StopAllJobs()
        {
            ChunkDownloads.ForEach(task => task.Abort());
            ChunkCalcVerticiesTasks.ForEach(task => task.Abort());
        }
    }
}
