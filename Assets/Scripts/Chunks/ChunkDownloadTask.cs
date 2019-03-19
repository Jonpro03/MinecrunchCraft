using System;
using System.IO;
using System.Net;
using minecrunch.tasks;
using minecrunch.models;
using minecrunch.models.Chunks;

namespace Assets.Scripts.Chunks
{
    public sealed class ChunkDownloadTask : ThreadedTask
    {
        // Store the result of the downloaded chunk.
        public Chunk chunk;

        // Quick ref the chunk coords.
        public readonly int cx, cy;
        private readonly string url;

        public ChunkDownloadTask(string host, string world, int x, int y)
        {
            cx = x;
            cy = y;
            url = $"{host}/api/chunk/{world}/{x}/{y}";
        }

        protected override void ThreadFunction()
        {
            chunk = null;
            WebClient webClient = new WebClient();
            byte[] result = null;
            try
            {
                result = webClient.DownloadData(url);
            }
            catch (Exception e)
            {
                this.e = e;
                return;
            }
            if (result != null)
            {
                chunk = Serializer.DeserializeFromStream<Chunk>(new MemoryStream(result), true);
            }
        }
    }
}
