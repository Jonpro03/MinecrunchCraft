using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class BirchWoodPlanksBlock : Block, IPlank
    {
        public override string Texture { get { return "Materials/BirchWoodPlankBlock"; } }

        public override bool IsTransparent { get { return false; } }

        public override bool IsGravityAffected { get { return false; } }

        public override uint MiningDifficulty { get { return MiningDifficulties.NO_TOOL + 4; } }

        public override string SoundWalkedOnAsset { get { return "Sounds/WoodWalk"; } }

        public override string SoundBeingMinedAsset { get { return "Sounds/WoodMined"; } }

        public override string SoundBlockBrokenAsset { get { return "Sounds/WoodBreak"; } }

        public override string SoundBlockPlacedAsset { get { return "Sounds/WoodPlaced"; } }

        public static BlockIdentification BlockId { get { return new BlockIdentification(5, 2); } }

        public override BlockCraftingRecipe BlockRecipe
        {
            get
            {
                return new BlockCraftingRecipe(new BlockIdentification[,] {
                    {AcaciaWoodBlock.BlockId, null, null },
                    {AcaciaWoodBlock.BlockId, null, null},
                    {null, null, null} });
            }
        }

        public BirchWoodPlanksBlock(Vector3 chunkPos, Vector2 chunkLoc) : base(chunkPos, chunkLoc)
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
