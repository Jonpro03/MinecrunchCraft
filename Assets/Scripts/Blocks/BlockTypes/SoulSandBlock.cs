using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class SoulSandBlock : Block
    {
        public override string Texture { get { return "Materials/SoulSandBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/SandWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/SandMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/SandBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/SandPlaced"; } }

        public SoulSandBlock(Vector3 pos) : base(pos)
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
