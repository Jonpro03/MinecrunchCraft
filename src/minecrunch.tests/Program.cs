using System;
using minecrunch.game.models;

namespace minecrunch.tests
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Blocks b = new Blocks();
            Console.WriteLine(b.blocksJson.Blocks.Count);
        }
    }
}
