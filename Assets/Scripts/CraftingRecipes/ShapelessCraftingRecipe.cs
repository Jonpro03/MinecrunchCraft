using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.CraftingRecipes
{
    //This class is used for recipes that dont have any specific shape but only need 
    //  the items to appear up on the crafting table regardless of positioning
    public abstract class ShapelessCraftingRecipe: CraftingRecipe
    {
        public List<List<BlockIdentification>> recipeBlocks;

        public override bool IsValid(List<RecipeItem> items)
        {
            /*If number of items passed is not same as number of items recipe is expecting,
                then it is not same recipe
            */
            if (items.Count() != recipeBlocks.Count())
                return false;

            List<List<BlockIdentification>> temp = new List<List<BlockIdentification>>(recipeBlocks);

            //Go through each passed item and remove it from recipe list
            foreach (var item in items) {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i].Any(x => x == item.BlockID))
                    {
                        temp.RemoveAt(i);
                        break;
                    }
                }            
            }
                

            //If no items remain in recipe list then that means the recipes are the same, else not the same
            return temp.Count().Equals(0);
        }

        public ShapelessCraftingRecipe()
        {
            recipeBlocks.Add(GridLock1);
            recipeBlocks.Add(GridLock2);
            recipeBlocks.Add(GridLock3);
            recipeBlocks.Add(GridLock4);
            recipeBlocks.Add(GridLock5);
            recipeBlocks.Add(GridLock6);
            recipeBlocks.Add(GridLock7);
            recipeBlocks.Add(GridLock8);
            recipeBlocks.Add(GridLock9);

            //Removes all empty list leaving only list of blocks in which number of list will be equivalent number of blocks in recipe
            recipeBlocks.RemoveAll(x => x.Count().Equals(0));
        }
    }
}
