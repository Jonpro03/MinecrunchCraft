using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.CraftingRecipes
{
    public static class BlockGroups
    {
        public static List<BlockIdentification> WOOD_PLANKS = new List<BlockIdentification>()
        {
            BlockIds.OakWoodPlanks,
            BlockIds.AcaciaWoodPlanks,
            BlockIds.BirchWoodPlanks,
            BlockIds.DarkOakWoodPlanks,
            BlockIds.JungleWoodPlanks,
            BlockIds.SpruceWoodPlanks
            
        };

        public static List<BlockIdentification> NO_BLOCK = new List<BlockIdentification>();

    }
}
