using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    [Serializable]
    public class ChunkData
    {

        public Vector2 ChunkPosition { get; set; }

        public float Biome { get; set; }

        public IBlock[,,] Blocks { get; set; }
    }
}
