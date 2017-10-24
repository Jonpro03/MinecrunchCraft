using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes.RecipeTypes
{
    class AndesiteRecipe : ShapelessCraftingRecipe
    {
        public override List<BlockIdentification> GridLoc1 { get { return new List<BlockIdentification> { BlockIds.Diorite }; } }
        public override List<BlockIdentification> GridLoc2 { get { return new List<BlockIdentification> { BlockIds.Cobblestone }; } }
        public override List<BlockIdentification> GridLoc3 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLoc4 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLoc5 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLoc6 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLoc7 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLoc8 { get { return BlockGroups.NO_BLOCK; } }
        public override List<BlockIdentification> GridLoc9 { get { return BlockGroups.NO_BLOCK; } }

        public override int blocksCreated { get { return 2; } }
    }
}