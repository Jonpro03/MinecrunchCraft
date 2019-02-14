using System;
using minecrunch.data.models;
using Assets.Scripts.Blocks;
using System.Collections.Generic;
using minecrunch.game.models;

namespace minecrunch.mappers
{
    public static class BlockMapper
    {
        public static BlockDataModel MapGameToData(IBlockBase block)
        {
            return new BlockDataModel(block.BlockId);
        }

        public static IBlockBase MapDataToGame(BlockDataModel bdm)
        {
            System.Type blockType = BlockDictionary.Lookup[bdm.id];
            return BlockDictionary.GetBlockById(bdm.id)
        }
    }
}
