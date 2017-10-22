using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using Assets.Scripts.CraftingRecipes;

namespace Assets.Scripts.Interfaces
{
    public interface ICraftingRecipe
    {
        /* Muh poorly made crafting table showing which spot in table
         * each gridlock corresponds to
         * 
         * -------------------------------------- 
         * | GridLock1 | GridLock2 | GridLock 3 |
         * |------------------------------------|
         * | GridLock4 | GridLock5 | GridLock 6 |
         * |------------------------------------|
         * | GridLock7 | GridLock8 | GridLock 9 |
         * --------------------------------------
         *
         * Each of the List represents all the acceptable BlockIdentifications for that given gridlock    
         */

        List<BlockIdentification> GridLock1 { get; }
        List<BlockIdentification> GridLock2 { get; }
        List<BlockIdentification> GridLock3 { get; }
        List<BlockIdentification> GridLock4 { get; }
        List<BlockIdentification> GridLock5 { get; }
        List<BlockIdentification> GridLock6 { get; }
        List<BlockIdentification> GridLock7 { get; }
        List<BlockIdentification> GridLock8 { get; }
        List<BlockIdentification> GridLock9 { get; }

        bool IsValid(List<RecipeItem> items);
    }
}
