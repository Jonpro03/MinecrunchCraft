using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IBlock
    {
        Vector3 PositionInChunk { get; set; }

        bool IsTopLevel { get; set; }

        List<Vector3> Verticies { get; set; }

        List<Vector2> UVs { get; set; }

        bool LeftVisible { get; set; }
        bool RightVisible { get; set; }
        bool TopVisible { get; set; }
        bool BottomVisible { get; set; }
        bool FrontVisible { get; set; }
        bool BackVisible { get; set; }
    }
}
