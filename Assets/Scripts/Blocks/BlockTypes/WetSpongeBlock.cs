using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class WetSpongeBlock : Block
    {
        public override string Texture { get { return "Materials/WetSpongeBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/SpongeWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/SpongeMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/SpongeBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/SpongePlaced"; } }

        public WetSpongeBlock(Vector3 pos) : base(pos)
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
