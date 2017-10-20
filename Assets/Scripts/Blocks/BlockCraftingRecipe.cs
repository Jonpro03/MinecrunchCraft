using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blocks
{
    public class BlockCraftingRecipe
    {
        public BlockIdentification[,] recipe;

        //This constructor is only to be used if the block is not craftable thus no recipe
        public BlockCraftingRecipe()
        {
            recipe = null;
        }

        public BlockCraftingRecipe(BlockIdentification[,] blockRecipe)
        {
            if (IsCorrectSize(blockRecipe))
            {
                recipe = blockRecipe;
            }
        }

        private bool IsCorrectSize(BlockIdentification[,] recipe)
        {
            //Check to ensure that the 2D array has 3 rows
            if(recipe.Length == 3)
            {
                //Ensure each row has only 3 elements
                for( int i = 0; i < recipe.Length; i++)
                {
                    if (recipe.GetLength(i) != 3)
                        return false;
                }
                return true;
            }
            return false;
        }

        public bool HasNoRecipe()
        {
            return recipe == null;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            BlockCraftingRecipe bcr = (BlockCraftingRecipe)obj;
            
            for(int i = 0; i < bcr.recipe.Length; i++)
            {
                for (int j = 0; j < bcr.recipe.GetLength(i); j++)
                {
                    if (recipe[i, j] != bcr.recipe[i, j])
                        return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
