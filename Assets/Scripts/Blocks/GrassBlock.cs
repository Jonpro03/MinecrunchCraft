using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class GrassBlock : Block
    {
        public string Texture { get; set; }

        public GrassBlock(Vector3 chunkPos, bool visible, Vector2 chunkLoc) : base(chunkPos, visible, chunkLoc)
        {

        }


    }
}
