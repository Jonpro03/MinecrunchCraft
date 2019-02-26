using System;
using System.Collections.Generic;
using UnityEngine;

namespace minecrunch.models.Runtime
{
    [Serializable]
    public class SerializableMesh
    {
        public Dictionary<int, string> Materials = new Dictionary<int, string>();

        public Dictionary<int, List<int>> Quads = new Dictionary<int, List<int>>();

        public List<Vector3> Verticies = new List<Vector3>();

        public List<Vector2> UVs = new List<Vector2>();
    }
}
