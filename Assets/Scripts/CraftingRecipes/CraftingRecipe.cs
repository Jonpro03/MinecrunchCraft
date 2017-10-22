using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes
{
    public abstract class CraftingRecipe: ICraftingRecipe
    {
        public abstract List<BlockIdentification> GridLock1 { get; }
        public abstract List<BlockIdentification> GridLock2 { get; }
        public abstract List<BlockIdentification> GridLock3 { get; }
        public abstract List<BlockIdentification> GridLock4 { get; }
        public abstract List<BlockIdentification> GridLock5 { get; }
        public abstract List<BlockIdentification> GridLock6 { get; }
        public abstract List<BlockIdentification> GridLock7 { get; }
        public abstract List<BlockIdentification> GridLock8 { get; }
        public abstract List<BlockIdentification> GridLock9 { get; }

        public abstract int blocksCreated { get; }

        public abstract bool IsValid(List<RecipeItem> items);
    }
}
