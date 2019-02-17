using System;
using System.Collections.Generic;


namespace minecrunch.models.Blocks
{
    [Serializable]
    public class Block : IBlock
    {
        public string Id { get; set; }

        public Sides FacesVisible { get; set; }

        public Block()
        {
            FacesVisible = new Sides();
        }

    }
}
