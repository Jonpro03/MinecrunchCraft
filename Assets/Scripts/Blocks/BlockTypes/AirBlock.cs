using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    [Serializable]
    public class AirBlock : Block
    {
        public override string Texture { get { return null; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override bool IsVisible()
        {
            return false;
        }

        public override uint MiningDifficulty { get { return 0; } }

        public override string SoundWalkedOnAsset { get { return null; } }

        public override string SoundBeingMinedAsset { get { return null; } }

        public override string SoundBlockBrokenAsset { get { return null; } }

        public override string SoundBlockPlacedAsset { get { return null; } }

        public AirBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
        {

        }

        public AirBlock(Vector3 worldPos) : base(worldPos)
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
