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
            OakWoodPlanksBlock.BlockId,
            SpruceWoodPlanksBlock.BlockId,
            BirchWoodPlanksBlock.BlockId,
            JungleWoodPlanksBlock.BlockId,
            AcaciaWoodPlankBlock.BlockId,
            DarkOakWoodPlanksBlock.BlockId
        };

    }
}
