using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    [Serializable]
    public class GlassBlock : Block
    {
        public override string Texture { get { return "Materials/GlassBlock"; } }

        public override bool IsTransparent { get { return true; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/StoneWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/StoneMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/GlassBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/StonePlaced"; } }

        public override BlockIdentification BlockId { get { return new BlockIdentification(20, 0); } }


        public GlassBlock(Vector3 pos) : base(pos)
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
