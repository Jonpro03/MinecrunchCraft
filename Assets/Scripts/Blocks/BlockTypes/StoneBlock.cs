using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    [Serializable]
    public class StoneBlock : Block
    {
        public override BlockIdentification BlockId
        {
            get
            {
                return BlockIds.Stone;
            }
        }

        public override string Texture { get { return "Materials/StoneBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.WOOD_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/StoneWalk"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/StoneBreak"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/StoneMined"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/StonePlaced"; } }


        public StoneBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
        {

        }

        public StoneBlock(Vector3 worldPos) : base(worldPos)
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
