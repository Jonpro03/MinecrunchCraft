using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Blocks
{
    public class BlockIdentification
    {
        public uint Id;
        public uint Meta;

        public BlockIdentification(uint BlockId, uint MetaData)
        {
            Id = BlockId;
            Meta = MetaData;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            BlockIdentification BlockId = (BlockIdentification)obj;

            return Id == BlockId.Id && Meta == BlockId.Meta;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
