using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.Chunks
{
    public class Chunk : MonoBehaviour
    {
        public bool Generated { get; set; }

        public bool HasUpdate { get; set; }

        public bool IsDrawn { get; set; }

        public bool IsSerialized { get; set; }

        public Vector2 WorldPosition { get; private set; }

        public Vector2 ChunkPosition { get; private set; }

        public Block[,,] Blocks { get; set; }

        public Dictionary<int, string> Materials;

        public Dictionary<int, List<int>> Triangles;

        public List<Vector3> Verticies;

        public List<Vector2> UVs;

        public float Biome { get; set; }

        public void InitializeChunk(Vector2 coordinate)
        {
            Generated = false;
            HasUpdate = false;
            IsDrawn = false;
            IsSerialized = false;
            ChunkPosition = coordinate;
            WorldPosition = ChunkPosition * 16;
            Blocks = new Block[16, 256, 16];
            Materials = new Dictionary<int, string>();
            Triangles = new Dictionary<int, List<int>>();
            Verticies = new List<Vector3>();
            UVs = new List<Vector2>();
        }
    }
}
