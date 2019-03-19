using minecrunch.models.Surrogates;
using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace minecrunch.models
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
        /// Serialize an object to a stream.
        /// </summary>
        /// <param name="obj">Anonymous type object.</param>
        /// <param name="stream">Stream to which the object will be serialized.</param>
        /// <param name="compress">Whether to compress the stream.</param>
        /// <returns>Whether successful.</returns>
        public static bool SerializeToStream(object obj, out Stream stream, bool compress=false)
        {
            stream = new MemoryStream();
            try
            {
                if (compress)
                {
                    using (var tempstream = new MemoryStream()) // Double buffer needed
                    {
                        using (var compressionStream = new GZipStream(tempstream, CompressionMode.Compress))
                        {
                            formatter.Serialize(compressionStream, obj);
                            tempstream.Position = 0;
                            tempstream.CopyTo(stream);
                        }
                    }
                    
                }
                else
                {
                    formatter.Serialize(stream, obj);
                    stream.Position = 0;
                }

            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Serialize an object to a file.
        /// </summary>
        /// <param name="obj">Anonymouse boject type to serialize.</param>
        /// <param name="filePath">File to which the object will be serialized.</param>
        /// <param name="compress">Whether to compress the stream.</param>
        /// <returns>Whether successful.</returns>
        public static bool SerializeToFile(object obj, string filePath, bool compress = false)
        {
            try
            {
                using (FileStream fileStream = File.Create(filePath))
                {
                    if (compress)
                    {
                        using (var compressionStream = new GZipStream(
                        fileStream, CompressionMode.Compress))
                        {
                            formatter.Serialize(compressionStream, obj);
                            compressionStream.Flush();
                        }
                    }
                    else
                    {
                        formatter.Serialize(fileStream, obj);
                    }
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
        /// Deserialize an object from a stream to a specific type.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="stream">Stream from which the object should be read from.</param>
        /// <param name="isCompressed">Whether the stream is compressed.</param>
        /// <returns>The object or null.</returns>
        public static T DeserializeFromStream<T>(Stream stream, bool isCompressed = false)
        {
            object obj = null;
            try
            {
                if (isCompressed)
                {
                    using (var compressionStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        obj = formatter.Deserialize(compressionStream);
                    }
                }
                else
                {
                    obj = formatter.Deserialize(stream);
                }
            } 
            catch (Exception)
            {
                return default(T);
            }
            return (T)obj;

        }

        /// <summary>
        /// Deserialize an object from a file to a specific type.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="filePath">Path of the file.</param>
        /// <param name="isCompressed">Whether the file stream is compressed.</param>
        /// <returns>The object or null.</returns>
        public static T DeserializeFromFile<T>(string filePath, bool isCompressed = false)
        {
            object obj = null;
            try
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    if (isCompressed)
                    {
                        using (var compressionStream = new GZipStream(fileStream, CompressionMode.Decompress))
                        {
                            obj = formatter.Deserialize(compressionStream);
                        }
                    }
                    else
                    {
                        obj = formatter.Deserialize(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return default(T);
            }

            return (T)obj;
        }
    }
}
