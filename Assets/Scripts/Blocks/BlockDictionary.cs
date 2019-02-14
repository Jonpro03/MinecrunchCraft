using System;
using System.Collections.Generic;
using minecrunch.game.models;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public static class BlockDictionary
    {
        public static Dictionary<string, Type> Lookup = new Dictionary<string, Type> {
            { "air", typeof(AirBlock) },
            { "stone", typeof(StoneBlock) },
        };

        public static IBlockBase GetBlockById (string id)
        {
            Type t = Lookup[id];
            if (t is null)
            {
                return new BlockBase() as IBlockBase;
            }
            IBlockBase instance = Activator.CreateInstance(t) as IBlockBase;
            return instance;
        }
    }
}
