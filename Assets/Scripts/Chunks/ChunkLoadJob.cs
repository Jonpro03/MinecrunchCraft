using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    public class ChunkLoadJob : ThreadedJob
    {
        public Chunk Chunk { get; private set;  }

        public Vector2 chunkCoord;

        public ChunkLoadJob(Vector2 chunkCoord, Chunk chunk)
        {
            this.chunkCoord = chunkCoord;
            Chunk = chunk;
        }

        protected override void ThreadFunction()
        {
            try
            {
                string chunkFilePath = String.Format(
                    "{0}/chunks/{1},{2}.dat",
                    World.World.WorldSaveFolder,
                    chunkCoord.x,
                    chunkCoord.y);
                if (!File.Exists(chunkFilePath))
                {
                    return;
                }
                ChunkData data;
                BinaryFormatter formatter = new BinaryFormatter();
                SurrogateSelector ss = new SurrogateSelector();
                ss.AddSurrogate(
                    typeof(Vector3),
                    new StreamingContext(StreamingContextStates.All),
                    new Vector3SerializationSurrogate());
                ss.AddSurrogate(
                    typeof(Vector2),
                    new StreamingContext(StreamingContextStates.All),
                    new Vector2SerializationSurrogate());
                formatter.SurrogateSelector = ss;
                using (FileStream chunkFile = File.OpenRead(chunkFilePath))
                {
                    data = (ChunkData)formatter.Deserialize(chunkFile);
                }

                Chunk.InitializeChunk(data.ChunkPosition);
                Chunk.Biome = data.Biome;
                Chunk.Generated = true;
                Chunk.Blocks = data.Blocks;
                Chunk.IsSerialized = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
