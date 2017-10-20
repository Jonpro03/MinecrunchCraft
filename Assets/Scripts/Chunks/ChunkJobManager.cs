using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    public class ChunkJobManager
    {
        private List<ChunkGenerateJob> ChunkGenerateJobs { get; set; }

        private List<ChunkUpdateJob> ChunkUpdateJobs { get; set; }

        public List<Chunk> CompletedJobs { get; set; }

        public ChunkJobManager()
        {
            ChunkGenerateJobs = new List<ChunkGenerateJob>();
            ChunkUpdateJobs = new List<ChunkUpdateJob>();
            CompletedJobs = new List<Chunk>();
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
    }
}
