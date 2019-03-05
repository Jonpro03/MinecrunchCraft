using Assets.Scripts.World;
using minecrunch.models.Chunks;
using minecrunch.tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    public class ChunkJobManager
    {
        public List<ChunkDownloadTask> ChunkDownloads { get; set; }
        public List<ChunkCalcVerticiesTask> ChunkCalcVerticiesTasks { get; set; }

        public List<Chunk> CompletedChunks { get; set; }

        public ChunkJobManager()
        {
            ChunkDownloads = new List<ChunkDownloadTask>();
            ChunkCalcVerticiesTasks = new List<ChunkCalcVerticiesTask>();
            CompletedChunks = new List<Chunk>();
        }

        public void Update()
        {
            
            foreach (var task in ChunkDownloads.Where(t => t.IsDone))
            {
                if (task.e != null)
                {
                    Debug.LogException(task.e);
                    WorldTerrain.InProgressChunks.Remove($"chunk{task.cx},{task.cy}");
                    continue;
                }
                if (task.chunk is null)
                {
                    WorldTerrain.InProgressChunks.Remove($"chunk{task.cx},{task.cy}");
                    Debug.LogWarning($"Failed to deserialize {task.cx}, {task.cy}");
                    continue;
                }
                ChunkCalcVerticiesTasks.Add(new ChunkCalcVerticiesTask(task.chunk));
            }
            ChunkDownloads.RemoveAll(task => task.IsDone);

            foreach (var task in ChunkCalcVerticiesTasks.Where(t => t.IsDone))
            {
                if (task.e != null)
                {
                    WorldTerrain.InProgressChunks.Remove($"chunk{task.chunk.x},{task.chunk.y}");
                    Debug.LogException(task.e);
                    continue;
                }

                CompletedChunks.Add(task.chunk);
            }
            ChunkCalcVerticiesTasks.RemoveAll(task => task.IsDone);

            int parallel = 3;
            ChunkDownloads.Take(parallel).ToList().ForEach(t => t.Start());

            ChunkCalcVerticiesTasks.Take(parallel).ToList().ForEach(t => t.Start());

        }

        public void StopAllJobs()
        {
            ChunkCalcVerticiesTasks.ForEach(task => task.Abort());

        }

        /**
        public bool AddUpdateJob(Chunk chunk)
        {
            if (!chunk.HasUpdate)
            {
                return false;
            }
            ChunkUpdateJob job = new ChunkUpdateJob(chunk);
            ChunkUpdateJobs.Add(job);
            job.Start();
            return true;
        }


        public async Task AddChunkSaveJob(Chunk chunk)
        {
            string chunkFilePath = string.Format(
                    "{0}/chunks/{1},{2}.dat",
                    World.World.WorldSaveFolder,
                    chunk.x,
                    chunk.y);

            await Task.Run(() =>Serializer.Serialize(chunk, chunkFilePath));
        }

        public async Task AddChunkLoadJob(Vector2 chunkCoord)
        {
            string name = $"chunk{chunkCoord.ToString()}";
            bool jobExists = CompletedChunks.Any(c => c.name == name);
            if (jobExists)
            {
                return;
            }

            string chunkFilePath = string.Format(
                    "{0}/chunks/{1},{2}.dat",
                    World.World.WorldSaveFolder,
                    chunkCoord.x,
                    chunkCoord.y);

            Chunk chunk = await Task.Run(() => Serializer.Deserialize(chunkFilePath));
            CompletedChunks.Add(chunk);

        }
        **/
    }
}
