using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class PumpkinBlock : Block
    {
        public override string Texture { get { return "Materials/PumpkinBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/WoodWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/WoodMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/WoodBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/WoodPlaced"; } }

        public PumpkinBlock(Vector3 pos) : base(pos)
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
