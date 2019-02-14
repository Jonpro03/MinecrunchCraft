using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class GraniteBlock : Block
    {
        public override string Texture { get { return "Materials/GraniteBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.WOOD_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/StoneWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/StoneMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/StoneBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/StonePlaced"; } }

        public GraniteBlock(Vector3 pos) : base(pos)
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
