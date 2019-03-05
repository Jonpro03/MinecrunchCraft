using minecrunch.tasks;
using minecrunch.models;
using minecrunch.models.Chunks;
using System.Net;
using System;
using System.IO;

namespace Assets.Scripts.Chunks
{
    public sealed class ChunkDownloadTask : ThreadedTask
    {
        public Chunk chunk;
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
