using UnityEngine;
using Assets.Scripts.Blocks;

namespace Assets.Scripts.Interfaces
{
    public interface IBlock
    {
        string BlockId { get; }
        int ChunkX { get; }
        int ChunkY { get; }
        int ChunkZ { get; }
        string Texture { get; }
        bool LeftVisible { get; set; }
        bool RightVisible { get; set; }
        bool TopVisible { get; set; }
        bool BottonVisible { get; set; }
        bool FrontVisible { get; set; }
        bool BackVisible { get; set; }


    }
}
