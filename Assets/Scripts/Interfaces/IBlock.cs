using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IBlock
    {
        Vector3 PositionInChunk { get; }

        Vector3 PositionInWorld { get; }

        List<Vector3> Verticies { get; }

        List<Vector2> UVs { get; }

        string Texture { get; }

        string SoundWalkedOnAsset { get; }

        string SoundBeingMinedAsset { get; }

        string SoundBlockBrokenAsset { get; }

        string SoundBlockPlacedAsset { get; }

        bool IsTransparent { get; }

        bool IsGravityAffected { get; }

        uint MiningDifficulty { get; }

        float Damage { get; }

        void OnTakeDamage(float damageAmount);

        void OnDestroyed();

        void OnPlaced();

        bool LeftVisible { get; set; }
        bool RightVisible { get; set; }
        bool TopVisible { get; set; }
        bool BottomVisible { get; set; }
        bool FrontVisible { get; set; }
        bool BackVisible { get; set; }
    }
}
