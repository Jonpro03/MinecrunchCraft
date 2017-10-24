using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes
{
    public abstract class CraftingRecipe : ICraftingRecipe
    {
        public abstract List<BlockIdentification> GridLoc1 { get; }
        public abstract List<BlockIdentification> GridLoc2 { get; }
        public abstract List<BlockIdentification> GridLoc3 { get; }
        public abstract List<BlockIdentification> GridLoc4 { get; }
        public abstract List<BlockIdentification> GridLoc5 { get; }
        public abstract List<BlockIdentification> GridLoc6 { get; }
        public abstract List<BlockIdentification> GridLoc7 { get; }
        public abstract List<BlockIdentification> GridLoc8 { get; }
        public abstract List<BlockIdentification> GridLoc9 { get; }

        public abstract int blocksCreated { get; }

        public abstract bool IsValid(List<RecipeItem> items);
    }
}
