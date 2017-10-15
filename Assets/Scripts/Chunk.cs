using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts
{
    public class Chunk : MonoBehaviour
    {
        public bool Generated { get; set; }

        public Vector2 WorldPosition { get; set; }

        public Vector2 ChunkPosition { get; set; }

        public Mesh mesh { get; set; }



    }
}
