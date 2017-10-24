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
         * each GridLoc corresponds to
         * 
         * -------------------------------------- 
         * | GridLoc1 | GridLoc2 | GridLoc 3 |
         * |------------------------------------|
         * | GridLoc4 | GridLoc5 | GridLoc 6 |
         * |------------------------------------|
         * | GridLoc7 | GridLoc8 | GridLoc 9 |
         * --------------------------------------
         *
         * Each of the List represents all the acceptable BlockIdentifications for that given GridLoc    
         */

        List<BlockIdentification> GridLoc1 { get; }
        List<BlockIdentification> GridLoc2 { get; }
        List<BlockIdentification> GridLoc3 { get; }
        List<BlockIdentification> GridLoc4 { get; }
        List<BlockIdentification> GridLoc5 { get; }
        List<BlockIdentification> GridLoc6 { get; }
        List<BlockIdentification> GridLoc7 { get; }
        List<BlockIdentification> GridLoc8 { get; }
        List<BlockIdentification> GridLoc9 { get; }

        bool IsValid(List<RecipeItem> items);
    }
}
