using System;
namespace minecrunch.data.models
{
    [Serializable]
    public class BlockDataModel
    {
        public readonly string id;

        public BlockDataModel(string id)
        {
            this.id = id;
}
    }
}
