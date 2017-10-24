﻿using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Blocks;
using Assets.Scripts.CraftingRecipes;

namespace Assets.Scripts.Interfaces
{
    public interface IBlock
    {
        BlockIdentification BlockId { get; }

        Vector3 PositionInChunk { get; }

        Vector3 PositionInWorld { get; }

        List<Vector3> Verticies { get; }

        List<Vector2> UVs { get; }

        string Texture { get; }

        bool IsTransparent { get; }

        bool IsVisible();

        bool IsGravityAffected { get; }

        float Damage { get; }

        void OnTakeDamage(float damageAmount);

        void OnPlaced();

        bool LeftVisible { get; set; }
        bool RightVisible { get; set; }
        bool TopVisible { get; set; }
        bool BottomVisible { get; set; }
        bool FrontVisible { get; set; }
        bool BackVisible { get; set; }

        void SetAllSidesVisible();

        object Clone();
    }
}
