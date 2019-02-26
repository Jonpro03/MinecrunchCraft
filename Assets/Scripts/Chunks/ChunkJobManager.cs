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
        public List<ChunkGenerateTerrainTask> ChunkGenerateTerrainTasks { get; set; }

        public List<ChunkGenerateCavesTask> ChunkGenerateCavesTasks { get; set; }

        public List<ChunkGenerateOresTask> ChunkGenerateOresTasks { get; set; }

        public List<ChunkGenerateEnvironmentTask> ChunkGenerateEnvironmentTasks { get; set; }

        public List<ChunkCalculateFacesTask> ChunkCalculateFacesTasks { get; set; }

        public List<ChunkCalcVerticiesTask> ChunkCalcVerticiesTasks { get; set; }

        public List<Chunk> CompletedChunks { get; set; }

        public ChunkJobManager()
        {
            ChunkGenerateTerrainTasks = new List<ChunkGenerateTerrainTask>();
            ChunkGenerateCavesTasks = new List<ChunkGenerateCavesTask>();
            ChunkGenerateOresTasks = new List<ChunkGenerateOresTask>();
            ChunkGenerateEnvironmentTasks = new List<ChunkGenerateEnvironmentTask>();
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
                }
            }
            ChunkGenerateTerrainTasks.RemoveAll(task => task.IsDone);

            foreach (var job in ChunkGenerateCavesTasks)
            {
                if (job.IsDone)
                {
                    var nextJob = new ChunkGenerateOresTask(job.chunk);
                    ChunkGenerateOresTasks.Add(nextJob);
                }
            }
            ChunkGenerateCavesTasks.RemoveAll(task => task.IsDone);


            foreach (var job in ChunkGenerateOresTasks)
            {
                if (job.IsDone)
                {
                    var nextJob = new ChunkGenerateEnvironmentTask(job.chunk);
                    ChunkGenerateEnvironmentTasks.Add(nextJob);
                }
            }
            ChunkGenerateOresTasks.RemoveAll(task => task.IsDone);


            foreach (var job in ChunkGenerateEnvironmentTasks)
            {
                if (job.IsDone)
                {
                    var nextJob = new ChunkCalculateFacesTask(job.chunk);
                    ChunkCalculateFacesTasks.Add(nextJob);
                }
            }
            ChunkGenerateEnvironmentTasks.RemoveAll(task => task.IsDone);

            foreach (var job in ChunkCalculateFacesTasks)
            {
                if (job.IsDone && !job.IsRunning)
                {
                    var nextJob = new ChunkCalcVerticiesTask(job.chunk);
                    ChunkCalcVerticiesTasks.Add(nextJob);
                    //ChunkCalculateFacesTasks.First(t => t.IsDone == false)?.Start();
                }
            }
            ChunkCalculateFacesTasks.RemoveAll(task => task.IsDone && !task.IsRunning);

            foreach (var job in ChunkCalcVerticiesTasks)
            {
                if (job.IsDone)
                {
                    if (job.e != null)
                    {
                        Debug.LogException(job.e);
                    }

                    string meshPath = World.World.WorldSaveFolder + $"/chunks/{job.chunk.name}.dat";
                    //new ChunkMeshSaveTask(job.chunk, meshPath).Start();
                    CompletedChunks.Add(job.chunk);
                }
            }
            ChunkCalcVerticiesTasks.RemoveAll(task => task.IsDone);

            int parallel = 2;
            //if (ChunkCalcVerticiesTasks.Count < parallel)
            ChunkGenerateTerrainTasks.Take(parallel).ToList().ForEach(t => t.Start());
            ChunkGenerateCavesTasks.Take(parallel).ToList().ForEach(t => t.Start());
            ChunkGenerateOresTasks.Take(parallel).ToList().ForEach(t => t.Start());
            ChunkGenerateEnvironmentTasks.Take(parallel).ToList().ForEach(t => t.Start());
            var runningFaceTasks = ChunkCalculateFacesTasks.Where(t => t.IsRunning is true).Count();
            if (ChunkCalculateFacesTasks.Count > 0 && runningFaceTasks.Equals(0))
                ChunkCalculateFacesTasks[0].Start();
            ChunkCalcVerticiesTasks.Take(parallel).ToList().ForEach(t => t.Start());

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
