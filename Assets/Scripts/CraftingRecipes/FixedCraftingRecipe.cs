using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.CraftingRecipes
{
    //This a class used for recipes that require recipes to be the exact same
    public abstract class FixedCraftingRecipe: ICraftingRecipe
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

        public bool IsValid(List<RecipeItem> items)
        {
            IEnumerable<RecipeItem> gridLockItem;

            /*
             * Below is equivalent to saying that no items are in that gridlock and
             *  the recipe does not call for any items to be there thus it is matching recipe
             * GridLock1.Count().Equals(0) && gridLockItem.Count().Equals(0)
             */

            gridLockItem = items.Where(item => GridLock1.Contains(item.BlockID) && item.GridLoc == 1);
            bool gridLock1Valid = gridLockItem.Count().Equals(1) || (GridLock1.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock2.Contains(item.BlockID) && item.GridLoc == 2);
            bool gridLock2Valid = gridLockItem.Count().Equals(1) || (GridLock2.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock3.Contains(item.BlockID) && item.GridLoc == 3);
            bool gridLock3Valid = gridLockItem.Count().Equals(1) || (GridLock3.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock4.Contains(item.BlockID) && item.GridLoc == 4);
            bool gridLock4Valid = gridLockItem.Count().Equals(1) || (GridLock4.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock5.Contains(item.BlockID) && item.GridLoc == 5);
            bool gridLock5Valid = gridLockItem.Count().Equals(1) || (GridLock5.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock6.Contains(item.BlockID) && item.GridLoc == 6);
            bool gridLock6Valid = gridLockItem.Count().Equals(1) || (GridLock6.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock7.Contains(item.BlockID) && item.GridLoc == 7);
            bool gridLock7Valid = gridLockItem.Count().Equals(1) || (GridLock7.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock8.Contains(item.BlockID) && item.GridLoc == 8);
            bool gridLock8Valid = gridLockItem.Count().Equals(1) || (GridLock8.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLock9.Contains(item.BlockID) && item.GridLoc == 9);
            bool gridLock9Valid = gridLockItem.Count().Equals(1) || (GridLock9.Count().Equals(0) && gridLockItem.Count().Equals(0));

            return gridLock1Valid && gridLock2Valid && gridLock3Valid && gridLock4Valid && gridLock5Valid && gridLock5Valid
                && gridLock6Valid && gridLock7Valid && gridLock8Valid && gridLock9Valid;

        }
    }
}
