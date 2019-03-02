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

        public static bool SerializeToStream(object obj, out Stream stream, bool compress=false)
        {
            stream = new MemoryStream();
            try
            {
                if (compress)
                {
                    using (var compressionStream = new GZipStream(stream, CompressionMode.Compress))
                    {
                        formatter.Serialize(compressionStream, obj);
                        compressionStream.Flush();
                    }
                }
                else
                {
                    formatter.Serialize(stream, obj);
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

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
