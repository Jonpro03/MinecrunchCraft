using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace minecrunch.parameters.Blocks
{
    public sealed class BlockInfo
    {
        private const string filename = "BlockParameters.xml";
        private readonly BlockParameters bxml;
        private static readonly Lazy<BlockInfo> lazy = new Lazy<BlockInfo>(() => new BlockInfo());

        public static BlockInfo Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private BlockInfo()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BlockParameters));

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                bxml = serializer.Deserialize(fs) as BlockParameters;
            }
        }

        /// <summary>
        /// Returns a list of all blocks.
        /// </summary>
        public List<BlockParameters.Block> GetAllBlockData()
        {
            return bxml.BlockList;
        }

        /// <summary>
        /// Return block from ID. Return Debug Block if not found.
        /// </summary>
        /// <param name="id">Block Identifier.</param>
        public BlockParameters.Block GetBlock(string id)
        {
            BlockParameters.Block block = bxml.BlockList.FirstOrDefault(b => b.Id == id);
            return block ?? bxml.BlockList.Find(b => b.Id == "debug");
        }

        /// <summary>
        /// Returns the path to the block's texture as a string.
        /// </summary>
        /// <param name="id">Block Identifier.</param>
        public string GetBlockTexture(string id)
        {
            return GetBlock(id).Texture;
        }
    }
}
