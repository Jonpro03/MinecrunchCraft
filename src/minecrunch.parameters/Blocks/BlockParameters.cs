using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace minecrunch.parameters.Blocks
{
    [Serializable]
    [XmlRoot("blockParameters")]
    public class BlockParameters
    {
        [Serializable]
        public class Block
        {
            [XmlAttribute("id")]
            public string Id { get; set; }
            [XmlElement("texture")]
            public string Texture { get; set; }
            [XmlElement("name")]
            public string Name { get; set; }
        }

        [XmlArray("blocks"), XmlArrayItem(typeof(Block), ElementName = "block")]
        public List<Block> BlockList { get; set; }
    }
}
