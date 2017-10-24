using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes
{
    public class BookShelfRecipe: ShapedCraftingRecipe
    {
        public override List<BlockIdentification> GridLoc1 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLoc2 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLoc3 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLoc4 { get { return new List<BlockIdentification> { BlockIds.Book }; } }
        public override List<BlockIdentification> GridLoc5 { get { return new List<BlockIdentification> { BlockIds.Book }; } }
        public override List<BlockIdentification> GridLoc6 { get { return new List<BlockIdentification> { BlockIds.Book }; } }
        public override List<BlockIdentification> GridLoc7 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLoc8 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLoc9 { get { return BlockGroups.WOOD_PLANKS; } }

        public override int blocksCreated { get { return 1; } }

    }
}
