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
        public override string Texture { get { return "Materials/GrassBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return 2; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/GrassWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/DirtMined"; } }

        public GrassBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
        {

        }

        public override void OnDestroyed()
        {

        }

        public override void OnPlaced()
        {

        }
    }
}
