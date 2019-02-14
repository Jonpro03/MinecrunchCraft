using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class WoolBlock : Block
    {
        public override string Texture { get { return "Materials/WoolBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/WoolWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/WoolMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/WoolBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/WoolPlaced"; } }

        public string Color { get; set; }

        public WoolBlock(Vector3 pos) : base(pos)
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
