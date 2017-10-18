using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class SnowBlock : Block
    {
        public override string Texture { get { return "Materials/SnowBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/SnowWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/SnowMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/SnowBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/SnowPlaced"; } }

        public SnowBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
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
