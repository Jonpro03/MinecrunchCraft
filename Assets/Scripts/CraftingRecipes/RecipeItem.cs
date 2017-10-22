using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes
{
    public class RecipeItem: IRecipeItem
    {
        public BlockIdentification BlockID { get; set; }
        public int GridLoc { get; set; }
    }
}
