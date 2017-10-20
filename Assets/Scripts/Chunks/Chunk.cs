using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Chunks
{
    public class Chunk : MonoBehaviour
    {
        public bool Generated { get; set; }

        public bool HasUpdate { get; set; }

        public bool IsDrawn { get; set; }

        public Vector2 WorldPosition { get; private set; }

        public Vector2 ChunkPosition { get; private set; }

        public IBlock[,,] Blocks { get; private set; }

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
            ChunkPosition = coordinate;
            WorldPosition = ChunkPosition * 16;
            Blocks = new IBlock[16, 256, 16];
            Materials = new Dictionary<int, string>();
            Triangles = new Dictionary<int, List<int>>();
            Verticies = new List<Vector3>();
            UVs = new List<Vector2>();
        }

        public IBlock GetBlock(Vector3 worldPos)
        {
            Vector3 chunkLoc;
            Vector2 chunk;
            Utility.Coordinates.WorldPosToChunkPos(worldPos, out chunkLoc, out chunk);
            return Blocks[(int) chunkLoc.x, (int) chunkLoc.y, (int) chunkLoc.z];
        }
    }
}
