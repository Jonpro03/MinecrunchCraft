using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    public class ChunkSaveJob : ThreadedJob
    {
        public Chunk Chunk { get; private set; }

        public ChunkSaveJob(Chunk chunk)
        {
            Chunk = chunk;
        }

        protected override void ThreadFunction()
        {
            try
            {
                ChunkData chunkData = new ChunkData()
                {
                    ChunkPosition = Chunk.ChunkPosition,
                    Biome = Chunk.Biome,
                    Blocks = Chunk.Blocks
                };

                string chunkFilePath = String.Format(
                    "{0}/chunks/{1},{2}.dat",
                    World.World.WorldSaveFolder,
                    Chunk.ChunkPosition.x,
                    Chunk.ChunkPosition.y);
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
                using (FileStream chunkFile = File.Create(chunkFilePath))
                {
                    formatter.Serialize(chunkFile, chunkData);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
