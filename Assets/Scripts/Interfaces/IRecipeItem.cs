using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.Interfaces
{
    public interface IRecipeItem:
    {
        BlockIdentification BlockID { get; }
        int GridLoc { get; }
    }
}
