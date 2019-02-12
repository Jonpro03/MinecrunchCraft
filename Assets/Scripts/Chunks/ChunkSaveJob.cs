using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Blocks;
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
            return;
            try
            {
                ChunkData chunkData = new ChunkData()
                {
                    PositionX = (int) Chunk.ChunkPosition.x,
                    PositionY = (int) Chunk.ChunkPosition.y,
                    Biome = Chunk.Biome,
                    Blocks = new BlockData[16, 256, 16]
                };

                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 256; y++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            Block chunkBlock = Chunk.Blocks[x, y, z];

                            if (null == chunkBlock.BlockId)
                            {
                                Debug.Log(chunkBlock.PositionInChunk);
                                continue;
                            }
                            chunkData.Blocks[x, y, z] = new BlockData()
                            {
                                WorldPositionX = (int) chunkBlock.PositionInWorld.x,
                                WorldPositionY = (int)chunkBlock.PositionInWorld.y,
                                WorldPositionZ = (int)chunkBlock.PositionInWorld.z,
                                BlockId = chunkBlock.BlockId
                            };
                        }
                    }
                }

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
