using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace minecrunch.models.Surrogates
{
    public sealed class Vector3SerializationSurrogate : ISerializationSurrogate
    {

        // Method called to serialize a Vector3 object
        public void GetObjectData(object obj,
                                  SerializationInfo info, StreamingContext context)
        {

            Vector3 v3 = (Vector3)obj;
            info.AddValue("x", v3.x);
            info.AddValue("y", v3.y);
            info.AddValue("z", v3.z);
        }

        // Method called to deserialize a Vector3 object
        public object SetObjectData(object obj,
                                           SerializationInfo info, StreamingContext context,
                                           ISurrogateSelector selector)
        {

            Vector3 v3 = (Vector3)obj;
            v3.x = info.GetSingle("x");
            v3.y = info.GetSingle("y");
            v3.z = info.GetSingle("z");
            obj = v3;
            return obj;
        }
    }
}
