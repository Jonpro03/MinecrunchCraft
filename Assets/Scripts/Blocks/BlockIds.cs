using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Blocks
{
    public class BlockIds
    {
        public static BlockIdentification Air = new BlockIdentification(0, 0);
        public static BlockIdentification Stone = new BlockIdentification(1, 0);
        public static BlockIdentification Granite = new BlockIdentification(1, 1);
        public static BlockIdentification PolishedGranite = new BlockIdentification(1, 2);
        public static BlockIdentification Diorite = new BlockIdentification(1, 3);
        public static BlockIdentification PolishedDiorite = new BlockIdentification(1, 4);
        public static BlockIdentification Andesite = new BlockIdentification(1, 5);
        public static BlockIdentification PolishedAndesite = new BlockIdentification(1, 6);

        public static BlockIdentification Grass = new BlockIdentification(2, 0);
        public static BlockIdentification Dirt = new BlockIdentification(3, 0);
        public static BlockIdentification CoarseDirt = new BlockIdentification(3, 1);
        public static BlockIdentification Podzol = new BlockIdentification(3, 2);

        public static BlockIdentification Cobblestone = new BlockIdentification(4, 0);

        public static BlockIdentification OakWoodPlanks = new BlockIdentification(5, 0);
        public static BlockIdentification SpruceWoodPlanks = new BlockIdentification(5, 1);
        public static BlockIdentification BirchWoodPlanks = new BlockIdentification(5, 2);
        public static BlockIdentification JungleWoodPlanks = new BlockIdentification(5, 3);
        public static BlockIdentification AcaciaWoodPlanks = new BlockIdentification(5, 4);
        public static BlockIdentification DarkOakWoodPlanks = new BlockIdentification(5, 5);

        public static BlockIdentification OakSapling = new BlockIdentification(6, 0);
        public static BlockIdentification SpruceSapling = new BlockIdentification(6, 1);
        public static BlockIdentification BirchSapling = new BlockIdentification(6, 2);
        public static BlockIdentification JungleSapling = new BlockIdentification(6, 3);
        public static BlockIdentification AcaciaSapling = new BlockIdentification(6, 4);
        public static BlockIdentification DarkOakSapling = new BlockIdentification(6, 5);

        public static BlockIdentification Bedrock = new BlockIdentification(7, 0);
        public static BlockIdentification FlowingWater = new BlockIdentification(8, 0);
        public static BlockIdentification StillWater = new BlockIdentification(9, 0);
        public static BlockIdentification FlowingLava = new BlockIdentification(10, 0);
        public static BlockIdentification StillLava = new BlockIdentification(11, 0);

        public static BlockIdentification Sand = new BlockIdentification(12, 0);
        public static BlockIdentification RedSand = new BlockIdentification(12, 1);
        public static BlockIdentification Gravel = new BlockIdentification(13, 0);

        public static BlockIdentification GoldOre = new BlockIdentification(14, 0);
        public static BlockIdentification IronOre = new BlockIdentification(15, 0);
        public static BlockIdentification CoalOre = new BlockIdentification(16, 0);

        public static BlockIdentification OakWood = new BlockIdentification(17, 0);
        public static BlockIdentification SpruceWood = new BlockIdentification(17, 1);
        public static BlockIdentification BirchWood = new BlockIdentification(17, 2);
        public static BlockIdentification JungleWood = new BlockIdentification(17, 3);

        public static BlockIdentification OakLeaves = new BlockIdentification(18, 0);
        public static BlockIdentification SpruceLeaves = new BlockIdentification(18, 1);
        public static BlockIdentification BirchLeaves = new BlockIdentification(18, 2);
        public static BlockIdentification JungleLeaves = new BlockIdentification(18, 3);

        public static BlockIdentification Sponge = new BlockIdentification(19, 0);
        public static BlockIdentification WetSponge = new BlockIdentification(19, 1);

        public static BlockIdentification Glass = new BlockIdentification(20, 0);

        public static BlockIdentification LapisLazuliOre = new BlockIdentification(21, 0);
        public static BlockIdentification LapisLazuliBlock = new BlockIdentification(22, 0);

        public static BlockIdentification Dispenser = new BlockIdentification(23, 0);

        public static BlockIdentification Sandstone = new BlockIdentification(24, 0);
        public static BlockIdentification ChiseledSandstone = new BlockIdentification(24, 1);
        public static BlockIdentification SmoothSandstone = new BlockIdentification(24, 2);

        public static BlockIdentification Bookshelf = new BlockIdentification(47, 0);

        public static BlockIdentification DiamondOre = new BlockIdentification(56, 0);
        public static BlockIdentification DiamondBlock = new BlockIdentification(57, 0);

        public static BlockIdentification StoneBricks = new BlockIdentification(98, 0);
        public static BlockIdentification MossyStoneBricks = new BlockIdentification(98, 1);
        public static BlockIdentification CrackedStoneBricks = new BlockIdentification(98, 2);
        public static BlockIdentification ChiseledStoneBricks = new BlockIdentification(98, 2);

        public static BlockIdentification AcaciaWood = new BlockIdentification(162, 0);
        public static BlockIdentification DarkOakWood = new BlockIdentification(162, 1);

        public static BlockIdentification BoneBlock = new BlockIdentification(216, 0);

        public static BlockIdentification Book = new BlockIdentification(340, 0);

        public static Dictionary<BlockIdentification, Type> BlockDictionary = new Dictionary<BlockIdentification, Type>
        {
            { Air, typeof(AirBlock) },
            { Stone, typeof(StoneBlock) },
            { Granite, typeof(GraniteBlock) },
            { PolishedGranite, typeof(PolishedGraniteBlock) },
            { Diorite, typeof(DioriteBlock) },
            { PolishedDiorite, typeof(PolishedDioriteBlock) },
            { Bedrock, typeof(BedrockBlock) },
            { Grass, typeof(GrassBlock) },
            { Dirt, typeof(DirtBlock) },
            { Sand, typeof(SandBlock) },
            { Gravel, typeof(GravelBlock) }
        };

        public static Type GetBlockType(int blockId, int metaData = 0)
        {
            return BlockDictionary.FirstOrDefault(x => x.Key.Id == blockId && x.Key.Meta == metaData).Value;
        }
    }
}
