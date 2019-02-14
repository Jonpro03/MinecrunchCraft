using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class GravelBlock : Block
    {
        public override string Texture { get { return "Materials/GravelBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return true; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/DirtWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/DirtMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/DirtBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/DirtPlaced"; } }

        public override BlockIdentification BlockId { get { return BlockIds.Gravel; } }

        public GravelBlock(Vector3 pos) : base(pos)
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
