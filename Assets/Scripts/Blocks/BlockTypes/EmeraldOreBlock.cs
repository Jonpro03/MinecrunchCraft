using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class EmeraldOreBlock : Block
    {
        public override string Texture { get { return "Materials/EmeraldOreBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.IRON_TOOL; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/StoneWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/StoneMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/StoneBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/StonePlaced"; } }

        public static BlockIdentification BlockId { get { return new BlockIdentification(129, 0); } }

        public override BlockCraftingRecipe BlockRecipe { get { return new BlockCraftingRecipe(); } }

        public EmeraldOreBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
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
