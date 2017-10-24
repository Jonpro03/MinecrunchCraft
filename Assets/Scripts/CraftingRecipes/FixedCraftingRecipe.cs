using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.CraftingRecipes
{
    //This a class used for recipes that require recipes to be the exact same
    public abstract class FixedCraftingRecipe: CraftingRecipe
    {
        public override bool IsValid(List<RecipeItem> items)
        {
            IEnumerable<RecipeItem> gridLockItem;

            /*
             * Below is equivalent to saying that no items are in that gridlock and
             *  the recipe does not call for any items to be there thus it is matching recipe
             * GridLock1.Count().Equals(0) && gridLockItem.Count().Equals(0)
             */

            gridLockItem = items.Where(item => GridLoc1.Contains(item.BlockID) && item.GridLoc == 1);
            bool gridLock1Valid = gridLockItem.Count().Equals(1) || (GridLoc1.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc2.Contains(item.BlockID) && item.GridLoc == 2);
            bool gridLock2Valid = gridLockItem.Count().Equals(1) || (GridLoc2.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc3.Contains(item.BlockID) && item.GridLoc == 3);
            bool gridLock3Valid = gridLockItem.Count().Equals(1) || (GridLoc3.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc4.Contains(item.BlockID) && item.GridLoc == 4);
            bool gridLock4Valid = gridLockItem.Count().Equals(1) || (GridLoc4.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc5.Contains(item.BlockID) && item.GridLoc == 5);
            bool gridLock5Valid = gridLockItem.Count().Equals(1) || (GridLoc5.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc6.Contains(item.BlockID) && item.GridLoc == 6);
            bool gridLock6Valid = gridLockItem.Count().Equals(1) || (GridLoc6.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc7.Contains(item.BlockID) && item.GridLoc == 7);
            bool gridLock7Valid = gridLockItem.Count().Equals(1) || (GridLoc7.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc8.Contains(item.BlockID) && item.GridLoc == 8);
            bool gridLock8Valid = gridLockItem.Count().Equals(1) || (GridLoc8.Count().Equals(0) && gridLockItem.Count().Equals(0));

            gridLockItem = items.Where(item => GridLoc9.Contains(item.BlockID) && item.GridLoc == 9);
            bool gridLock9Valid = gridLockItem.Count().Equals(1) || (GridLoc9.Count().Equals(0) && gridLockItem.Count().Equals(0));

            return gridLock1Valid && gridLock2Valid && gridLock3Valid && gridLock4Valid && gridLock5Valid && gridLock5Valid
                && gridLock6Valid && gridLock7Valid && gridLock8Valid && gridLock9Valid;

        }
    }
}
