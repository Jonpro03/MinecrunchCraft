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

        public bool IsValid(List<IRecipeItem> items)
        {
            IEnumerable<IRecipeItem> gridLockItem;

            gridLockItem = items.Where(item => GridLock1.Contains(item.BlockID) && item.GridLoc == 1);
            bool gridLock1Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock2.Contains(item.BlockID) && item.GridLoc == 2);
            bool gridLock2Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock3.Contains(item.BlockID) && item.GridLoc == 3);
            bool gridLock3Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock4.Contains(item.BlockID) && item.GridLoc == 4);
            bool gridLock4Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock5.Contains(item.BlockID) && item.GridLoc == 5);
            bool gridLock5Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock6.Contains(item.BlockID) && item.GridLoc == 6);
            bool gridLock6Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock7.Contains(item.BlockID) && item.GridLoc == 7);
            bool gridLock7Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock8.Contains(item.BlockID) && item.GridLoc == 8);
            bool gridLock8Valid = gridLockItem.Count().Equals(1);

            gridLockItem = items.Where(item => GridLock9.Contains(item.BlockID) && item.GridLoc == 9);
            bool gridLock9Valid = gridLockItem.Count().Equals(1);

            return gridLock1Valid && gridLock2Valid && gridLock3Valid && gridLock4Valid && gridLock5Valid && gridLock5Valid
                && gridLock6Valid && gridLock7Valid && gridLock8Valid && gridLock9Valid;
        
    }
    }
}
