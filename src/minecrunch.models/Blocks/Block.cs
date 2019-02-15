using System;
using System.Collections.Generic;
using UnityEngine;

namespace minecrunch.models.Blocks
{
    [Serializable]
    public class Block : IBlock
    {
        public string BlockId { get; set; }

        public Sides FacesVisible { get; set; }

    }
}
