using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public abstract class Stairs: Component
    {
        public abstract string Texture { get; }

        public abstract bool IsTransparent { get; }

        public abstract bool IsGravityAffected { get; }

        public abstract uint MiningDifficulty { get; }

        public abstract string SoundWalkedOnAsset { get; }

        public abstract string SoundBeingMinedAsset { get; }

        public abstract string SoundBlockBrokenAsset { get; }

        public abstract string SoundBlockPlacedAsset { get; }

        public virtual float Damage { get; private set; }

        public abstract void OnDestroyed();

        public abstract void OnPlaced();


        public Stairs(Vector3 posInChunk, Vector2 chunkLoc)
        {

        }

    }
}
