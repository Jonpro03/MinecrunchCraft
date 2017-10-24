using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    [Serializable]
    public class GrassBlock : Block
    {
        public override string Texture { get { return "Materials/GrassBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/GrassWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/GrassMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/GrassBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/GrassPlaced"; } }

        public override BlockIdentification BlockId { get { return new BlockIdentification(2, 0); } }

        public GrassBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
        {

        }

        public GrassBlock(Vector3 worldPos) : base(worldPos)
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
