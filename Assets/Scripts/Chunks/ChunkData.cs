using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    [Serializable]
    public class ChunkData
    {
        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public float Biome { get; set; }

        public BlockData[,,] Blocks { get; set; }
    }
}
