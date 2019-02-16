using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    public class ChunkJobManager
    {
        public List<ChunkGenerateJob> ChunkGenerateJobs { get; set; }

        public List<ChunkUpdateJob> ChunkUpdateJobs { get; private set; }

        public List<Chunk> CompletedJobs { get; set; }

        public List<ChunkLoadJob> ChunkLoadJobs { get; set; }

        public ChunkJobManager()
        {
            ChunkGenerateJobs = new List<ChunkGenerateJob>();
            ChunkUpdateJobs = new List<ChunkUpdateJob>();
            CompletedJobs = new List<Chunk>();
            ChunkLoadJobs = new List<ChunkLoadJob>();
        }

        public void Update()
        {
            foreach (ChunkGenerateJob chunkJob in ChunkGenerateJobs)
            {
                if (chunkJob.IsDone)
                {
                    // Generate jobs go into update jobs queue.
                    ChunkUpdateJob updateJob = new ChunkUpdateJob(chunkJob.Chunk);
                    ChunkUpdateJobs.Add(updateJob);
                    updateJob.Start();
                }
            }
            ChunkGenerateJobs.RemoveAll(c => c.IsDone);

            foreach (ChunkUpdateJob chunkJob in ChunkUpdateJobs)
            {
                if (chunkJob.IsDone)
                {
                    CompletedJobs.Add(chunkJob.Chunk);
                }
            }
            ChunkUpdateJobs.RemoveAll(c => c.IsDone);

            foreach (ChunkLoadJob chunkJob in ChunkLoadJobs)
            {
                if (chunkJob.IsDone)
                {
                    ChunkUpdateJob updateJob = new ChunkUpdateJob(chunkJob.Chunk);
                    ChunkUpdateJobs.Add(updateJob);
                    updateJob.Start();
                    //CompletedJobs.Add(chunkJob.Chunk);
                }
            }
            ChunkLoadJobs.RemoveAll(c => c.IsDone);
        }

        public void StopAllJobs()
        {
            foreach (ChunkGenerateJob chunkJob in ChunkGenerateJobs)
            {
                chunkJob.Abort();
            }
            foreach (ChunkUpdateJob chunkJob in ChunkUpdateJobs)
            {
                chunkJob.Abort();
            }
            foreach (ChunkLoadJob chunkJob in ChunkLoadJobs)
            {
                chunkJob.Abort();
            }
        }

        public bool AddGenerateJob(Chunk chunk)
        {
            if (chunk.Generated)
            {
                return false;
            }
            ChunkGenerateJob job = new ChunkGenerateJob(chunk);
            ChunkGenerateJobs.Add(job);
            job.Start();
            return true;
        }

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
    }
}
