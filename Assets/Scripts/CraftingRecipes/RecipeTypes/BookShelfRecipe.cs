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
        public override List<BlockIdentification> GridLock1 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLock2 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLock3 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLock4 { get { return new List<BlockIdentification> { BookItem.ID }; } }
        public override List<BlockIdentification> GridLock5 { get { return new List<BlockIdentification> { BookItem.ID }; } }
        public override List<BlockIdentification> GridLock6 { get { return new List<BlockIdentification> { BookItem.ID }; } }
        public override List<BlockIdentification> GridLock7 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLock8 { get { return BlockGroups.WOOD_PLANKS; } }
        public override List<BlockIdentification> GridLock9 { get { return BlockGroups.WOOD_PLANKS; } }

        public override int blocksCreated { get { return 1; } }

    }
}
