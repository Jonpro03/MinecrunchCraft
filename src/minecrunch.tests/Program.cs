using System;
using minecrunch.game.models;
using minecrunch.mappers.XML.Blocks;

namespace minecrunch.tests
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            BlockParameters blockdata = BlockInfo.GetBlockData();
            Console.WriteLine(blockdata.BlockList.Count);
        }
    }
}
