using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace minecrunch.game.models
{
    [DataContract]
    public class BlockJsonModel
    {
        public List<Block> Blocks { get; set; }

        public class Block
        {
            public string BlockId { get; set; }
            public string Texture { get; set; }
        }
    }
}
