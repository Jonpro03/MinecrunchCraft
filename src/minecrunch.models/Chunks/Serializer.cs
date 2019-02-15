using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using minecrunch.models.Surrogates;
using UnityEngine;

namespace minecrunch.models.Chunks
{
    public static class Serializer
    {
        private static BinaryFormatter formatter;

        static Serializer()
        {
            formatter = new BinaryFormatter();
            SurrogateSelector ss = new SurrogateSelector();
            ss.AddSurrogate(typeof(Vector2),
                new StreamingContext(StreamingContextStates.All),
                new Vector2SerializationSurrogate());
            ss.AddSurrogate(typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3SerializationSurrogate());
            formatter.SurrogateSelector = ss;
        }

        /// <summary>
        /// Serialize the specified chunk. Save to filePath.
        /// </summary>
        /// <returns>Whether saved successfully.</returns>
        /// <param name="chunk">Chunk Data.</param>
        /// <param name="filePath">File path and name.</param>
        public static bool Serialize(Chunk chunk, string filePath)
        {
            try
            {
                using (FileStream chunkFile = File.Create(filePath))
                {
                    formatter.Serialize(chunkFile, chunk);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Load a chunk from disk.
        /// </summary>
        /// <returns>The deserialize.</returns>
        /// <param name="filePath">File path.</param>
        public static Chunk Deserialize(string filePath)
        {
            Chunk chunk = null;
            try
            {
                using (FileStream chunkFile = File.OpenRead(filePath))
                {
                    chunk = formatter.Deserialize(chunkFile) as Chunk;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return chunk;
        }
    }
}
