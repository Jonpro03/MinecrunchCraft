using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    [Serializable]
    public class BlockData
    {
        public BlockIdentification BlockId { get; set; }

        public int WorldPositionX { get; set; }

        public int WorldPositionY { get; set; }

        public int WorldPositionZ { get; set; }
    }
}
