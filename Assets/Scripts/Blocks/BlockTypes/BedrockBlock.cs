﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.CraftingRecipes;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    [Serializable]
    public class BedrockBlock : Block
    {
        public override string Texture { get { return "Materials/BedrockBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return 0; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/StoneWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/StoneMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/StoneBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/StonePlaced"; } }

        public override BlockIdentification BlockId { get { return BlockIds.Bedrock; } }

        public BedrockBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
        {

        }

        public BedrockBlock(Vector3 worldPosition) : base(worldPosition)
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
