using System;
using System.Collections.Generic;


namespace minecrunch.models.Blocks
{
    [Serializable]
    public class Block
    {
        public byte x;
        public byte y;
        public byte z;

        public string Id { get; set; }

        public byte faceByte = 0b00000000;

        public bool GetFaceVisible(Sides side)
        {
            return (faceByte & (byte)side) == (byte)side;
        }

        public void SetFaceVisible(Sides side, bool visible)
        {
            if (visible)
            {
                faceByte |= (byte)side;
            }
            else
            {
                faceByte &= (byte)~side;
            }
        }

    }
}
