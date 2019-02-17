using minecrunch.models.Chunks;
using minecrunch.tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    public class ChunkJobManager
    {
        public List<ChunkGenerateTerrainTask> ChunkGenerateTerrainTasks { get; set; }

        public List<ChunkGenerateCavesTask> ChunkGenerateCavesTasks { get; set; }

        public List<ChunkCalculateFacesTask> ChunkCalculateFacesTasks { get; set; }

        public List<ChunkCalcVerticiesTask> ChunkCalcVerticiesTasks { get; set; }

        public List<Chunk> CompletedChunks { get; set; }

        public ChunkJobManager()
        {
            ChunkGenerateTerrainTasks = new List<ChunkGenerateTerrainTask>();
            ChunkGenerateCavesTasks = new List<ChunkGenerateCavesTask>();
            ChunkCalculateFacesTasks = new List<ChunkCalculateFacesTask>();
            ChunkCalcVerticiesTasks = new List<ChunkCalcVerticiesTask>();
            CompletedChunks = new List<Chunk>();
        }

        public void Update()
        {
            foreach (var job in ChunkGenerateTerrainTasks)
            {
                if (job.IsDone)
                {
                    var nextJob = new ChunkGenerateCavesTask(job.chunk);
                    ChunkGenerateCavesTasks.Add(nextJob);                 
                    nextJob.Start();
                }
            }
            ChunkGenerateTerrainTasks.RemoveAll(task => task.IsDone);


            foreach (var job in ChunkGenerateCavesTasks)
            {
                if (job.IsDone)
                {
                    var nextJob = new ChunkCalculateFacesTask(job.chunk);
                    ChunkCalculateFacesTasks.Add(nextJob);
                    nextJob.Start();
                }
            }
            ChunkGenerateCavesTasks.RemoveAll(task => task.IsDone);

            foreach (var job in ChunkCalculateFacesTasks)
            {
                if (job.IsDone)
                {
                    var nextJob = new ChunkCalcVerticiesTask(job.chunk);
                    ChunkCalcVerticiesTasks.Add(nextJob);
                    nextJob.Start();
                }
            }
            ChunkCalculateFacesTasks.RemoveAll(task => task.IsDone);

            foreach (var job in ChunkCalcVerticiesTasks)
            {
                if (job.IsDone)
                {
                    CompletedChunks.Add(job.chunk);
                }
            }
            ChunkCalcVerticiesTasks.RemoveAll(task => task.IsDone);
        }

        public void StopAllJobs()
        {
            ChunkGenerateTerrainTasks.ForEach(task => task.Abort());
            ChunkGenerateCavesTasks.ForEach(task => task.Abort());
            ChunkCalculateFacesTasks.ForEach(task => task.Abort());
            ChunkCalcVerticiesTasks.ForEach(task => task.Abort());

        }

        public bool AddGenerateJob(Chunk chunk)
        {

            if (chunk.GetBlockByChunkCoord(0,0,0) != null) // Hacky way to see if the chunk is generated.
            {
                return false;
            }
            var job = new ChunkGenerateTerrainTask(chunk);
            ChunkGenerateTerrainTasks.Add(job);
            job.Start();
            return true;
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

        public void AddChunkSaveJob(Chunk chunk)
        {
            ChunkSaveJob job = new ChunkSaveJob(chunk);
            job.Start();
        }

        public bool AddChunkLoadJob(Vector2 chunkCoord, Chunk chunk)
        {
            bool jobExists = ChunkLoadJobs.Any(clj => clj.chunkCoord == chunkCoord);
            if (jobExists)
            {
                return false;
            }
            ChunkLoadJob job = new ChunkLoadJob(chunkCoord, chunk);
            ChunkLoadJobs.Add(job);
            job.Start();
            return true;

        }
    **/
    }
}
