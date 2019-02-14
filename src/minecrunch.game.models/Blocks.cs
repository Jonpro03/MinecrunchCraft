using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace minecrunch.game.models
{
    public class Blocks
    {
        private const string JsonFile = "blocks.json";
        public BlockJsonModel blocksJson;


        public Blocks()
        {
            var js = new DataContractJsonSerializer(typeof(BlockJsonModel));
            using (StreamReader stream = File.OpenText(JsonFile))
            {
                string json = stream.ReadToEnd();
                blocksJson = JsonConvert.Deserialize

            }
        }

        public BlockBase GetNewBlock(string id)
        {
            BlockJsonModel.Block blockJson = blocksJson.Blocks.Find(x => x.BlockId == id);
            var block = new BlockBase
            {
                BlockId = id,
                Texture = blockJson.Texture
            };
            return block;
        }
    }
}
