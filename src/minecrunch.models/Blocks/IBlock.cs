using System;
namespace minecrunch.models.Blocks
{
    public interface IBlock
    {
        string Id { get; }
        Sides FacesVisible { get; }
    }
}
