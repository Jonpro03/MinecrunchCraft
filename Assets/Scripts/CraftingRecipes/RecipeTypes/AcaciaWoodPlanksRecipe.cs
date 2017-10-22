using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes.RecipeTypes
{
    class AcaciaWoodPlanksRecipe: ShapedCraftingRecipe
    {
        public override List<BlockIdentification> GridLock1 { get { return new List<BlockIdentification> { AcaciaWoodBlock.BlockId}; } }
        public override List<BlockIdentification> GridLock2 { get { return new List<BlockIdentification> { AcaciaWoodBlock.BlockId}; } }
        public override List<BlockIdentification> GridLock3 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLock4 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLock5 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLock6 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLock7 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLock8 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLock9 { get { return BlockGroups.NO_BLOCK; } }

        public override int blocksCreated { get { return 4; } }
    }
}
