using System.Runtime.Serialization;
using UnityEngine;

namespace minecrunch.models.Surrogates
{
    public sealed class Vector2SerializationSurrogate : ISerializationSurrogate
    {

        // Method called to serialize a Vector2 object
        public void GetObjectData(object obj,
                                  SerializationInfo info, StreamingContext context)
        {

            Vector2 v2 = (Vector2)obj;
            info.AddValue("x", v2.x);
            info.AddValue("y", v2.y);
        }

        // Method called to deserialize a Vector2 object
        public object SetObjectData(object obj,
                                           SerializationInfo info, StreamingContext context,
                                           ISurrogateSelector selector)
        {

            Vector2 v2 = (Vector2)obj;
            v2.x = info.GetSingle("x");
            v2.y = info.GetSingle("y");
            obj = v2;
            return obj;
        }
    }
}
