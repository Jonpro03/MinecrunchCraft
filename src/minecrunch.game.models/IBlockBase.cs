using System;
namespace minecrunch.game.models
{
    public interface IBlockBase
    {
        string BlockId { get; }
        string Texture { get; }
        int ChunkX { get; set; }
        int ChunkY { get; set; }
        int ChunkZ { get; set; }
    }
}
