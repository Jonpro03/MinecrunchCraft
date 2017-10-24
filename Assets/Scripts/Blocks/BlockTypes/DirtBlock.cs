using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    [Serializable]
    public class DirtBlock : Block
    {
        public override string Texture { get { return "Materials/DirtBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/DirtWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/DirtMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/DirtBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/DirtPlaced"; } }

        public override BlockIdentification BlockId { get { return BlockIds.Dirt; } }

        public DirtBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
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
