using System;
namespace minecrunch.models.Blocks
{
    public interface IBlock
    {
        string BlockId { get; }
        Sides FacesVisible { get; }
    }
}
