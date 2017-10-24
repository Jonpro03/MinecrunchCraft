using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes
{
    //Class used for recipes that have specific shapes to the recipes but can be
    // freely placed around the crafting table as long as the shape and positioning
    // of blocks to each other is maintained
    public abstract class ShapedCraftingRecipe: CraftingRecipe
    {
        public List<List<BlockIdentification>> recipeBlocks;

        public override bool IsValid(List<RecipeItem> items)
        {
            /*If the number of non-empty list in the recipeBlocks list is not the same as number of 
             * itmes passed then the recipes are not the same
            */
            if (!sameSizeWithoutEmptyList(recipeBlocks, items))
                return false;

            //Get the first non-empty gridloc
            int block1GridLoc = getNextNonEmptyGridLoc(0);
            //Item that corresponds to block1GridLoc
            RecipeItem item = null;

            //Get the item from passed items list that has same block id as first item in recipeBlocks list
            //If found then remove that item from list, save to its own variable and break
            for(int i = 0; i<items.Count; i++)
            {
                if (recipeBlocks[block1GridLoc - 1].Any(x => x == items[i].BlockID))
                {
                    item = items[i];
                    items.RemoveAt(i);
                    break;
                }                
            }


            //If item is still null after previous iteration, then that means no items in passed item list
            // corresponds to the first non-empty gridLoc thus it must be two different recipes
            if (item == null)
                return false;


            
            //Iterate through the rest of the blocks in RecipeList that are not empty list
            for(int i = block1GridLoc; i < recipeBlocks.Count; i++)
            {
                int block2GridLoc = getNextNonEmptyGridLoc(block1GridLoc);
                if (block2GridLoc == -1)
                    return false;
                bool matched = false;

                //Iterate through the remaining items list
                for(int j = 0; j<items.Count; j++)
                {
                    //In order for the item from passed items list to correspond to block from RecipeBlock it must maintain same positioning according to first block/item found earlier
                    // 1st: Must have same BlockID
                    // 2nd: The difference in GridLoc from first item/block must be the same as new block/item from these two iterations
                    // If both these conditions are met then it can be said that the item and block are one and the same
                    if (recipeBlocks[block2GridLoc].Any(x => x == items[j].BlockID && (Math.Abs(block1GridLoc - block2GridLoc) == Math.Abs(item.GridLoc - items[j].GridLoc))))
                    {
                        matched = true;
                        items.RemoveAt(j);
                        break;
                    }                       
                }
                //If iterated through all items in list and not one matched to block from outerloop, then that means the recipes are different
                if (!matched)
                    return false;
            }
            //If manage to iterate through the entire RecipeBlock list that means all blocks were matched to an item and this means the recipes are the same thus return true
            return true;



        }

        private bool sameSizeWithoutEmptyList(List<List<BlockIdentification>> recipeBlocks, List<RecipeItem> items)
        {
            recipeBlocks.RemoveAll(x => x.Count().Equals(0));
            return recipeBlocks.Count == items.Count;
        }

        //Returns an int representing next gridLoc expecting a block, returns -1 if no more gridlocs
        private int getNextNonEmptyGridLoc( int currentGridLoc)
        {
            int newGridLoc = currentGridLoc + 1;
            while(newGridLoc < 9)
            {
                //if the list at gridloc position is not empty, then a block is expected here for recipe so this is next gridloc
                //  position to check for. If its empty then keep searching for non-empty list
                // Index in recipeBlocks = newGridLoc - 1
                if (recipeBlocks[newGridLoc - 1].Count != 0)
                    return newGridLoc;

                newGridLoc += 1;    
            }
            return -1;


        }

        public ShapedCraftingRecipe()
        {
            //Will keep empty list, the index position in list will correspond to gridlock position
            recipeBlocks.Add(GridLoc1);
            recipeBlocks.Add(GridLoc2);
            recipeBlocks.Add(GridLoc3);
            recipeBlocks.Add(GridLoc4);
            recipeBlocks.Add(GridLoc5);
            recipeBlocks.Add(GridLoc6);
            recipeBlocks.Add(GridLoc7);
            recipeBlocks.Add(GridLoc8);
            recipeBlocks.Add(GridLoc9);
        }

    }
}
