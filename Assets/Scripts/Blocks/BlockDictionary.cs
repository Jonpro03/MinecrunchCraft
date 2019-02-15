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

        public static IBlockState GetBlockById (string id)
        {
            Type t = Lookup[id];
            if (t is null)
            {
                return new BlocStatse() as IBlockState;
            }
            IBlockState instance = Activator.CreateInstance(t) as IBlockState;
            return instance;
        }
    }
}
