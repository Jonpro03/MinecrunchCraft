using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IDrawable
    {
        List<Vector3> Verticies { get; }

        List<Vector2> UVs { get; }

        string Texture { get; }

        bool IsTransparent { get; }

        bool IsVisible();

        bool LeftVisible { get; set; }
        bool RightVisible { get; set; }
        bool TopVisible { get; set; }
        bool BottomVisible { get; set; }
        bool FrontVisible { get; set; }
        bool BackVisible { get; set; }

        void SetAllSidesVisible();

    }
}
