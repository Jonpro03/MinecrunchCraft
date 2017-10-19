using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public abstract class Block : Component, IBlock
    {
        public virtual Vector3 PositionInChunk { get; private set; }

        public virtual Vector3 PositionInWorld { get; private set; }

        public virtual List<Vector3> Verticies { get; private set; }

        public virtual List<Vector2> UVs { get; private set; }

        public abstract string Texture { get; }

        public abstract bool IsTransparent { get; }

        public abstract bool IsGravityAffected { get; }

        public abstract uint MiningDifficulty { get; }

        public abstract string SoundWalkedOnAsset { get; }

        public abstract string SoundBeingMinedAsset { get; }

        public abstract string SoundBlockBrokenAsset { get; }

        public abstract string SoundBlockPlacedAsset { get; }

        public virtual float Damage { get; private set; }

        private bool leftVisible, rightVisible, topVisible, bottomVisible, frontVisible, backVisible;

        public virtual bool LeftVisible
        {
            get { return leftVisible; }
            set
            {
                leftVisible = value;
                if (value == true)
                {
                    Verticies.AddRange(new List<Vector3>
                    {
                        new Vector3(0, 1, 1) + PositionInChunk,
                        new Vector3(0, 1, 0) + PositionInChunk,
                        new Vector3(0, 0, 0) + PositionInChunk,
                        new Vector3(0, 0, 0) + PositionInChunk,
                        new Vector3(0, 0, 1) + PositionInChunk,                        
                        new Vector3(0, 1, 1) + PositionInChunk,
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0.333f + 0.333f, 0.5f + 0.5f),
                        new Vector2(0 + 0.333f, 0.5f + 0.5f),
                        new Vector2(0 + 0.333f, 0 + 0.5f),
                        new Vector2(0 + 0.333f, 0 + 0.5f),
                        new Vector2(0.333f + 0.333f, 0 + 0.5f),
                        new Vector2(0.333f + 0.333f, 0.5f + 0.5f),
                    });
                }
            }
        }

        public virtual bool RightVisible
        {
            get { return rightVisible; }
            set
            {
                rightVisible = value;
                if (value == true)
                {
                    Verticies.AddRange(new List<Vector3>
                    {
                        new Vector3(1, 0, 0) + PositionInChunk,
                        new Vector3(1, 1, 0) + PositionInChunk,
                        new Vector3(1, 1, 1) + PositionInChunk,
                        new Vector3(1, 1, 1) + PositionInChunk,
                        new Vector3(1, 0, 1) + PositionInChunk,
                        new Vector3(1, 0, 0) + PositionInChunk,
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0 + 0.666f, 0 + 0.5f),
                        new Vector2(0 + 0.666f, 0.5f + 0.5f),
                        new Vector2(0.333f + 0.666f, 0.5f + 0.5f),
                        new Vector2(0.333f + 0.666f, 0.5f + 0.5f),
                        
                        new Vector2(0.333f + 0.666f, 0 + 0.5f),
                        new Vector2(0 + 0.666f, 0 + 0.5f),
                    });
                }
            }
        }

        public virtual bool TopVisible
        {
            get { return topVisible; }
            set
            {
                topVisible = value;
                if (value == true)
                {
                    Verticies.AddRange(new List<Vector3>
                    {
                        new Vector3(1, 1, 1) + PositionInChunk,
                        new Vector3(1, 1, 0) + PositionInChunk,
                        new Vector3(0, 1, 0) + PositionInChunk,
                        new Vector3(0, 1, 0) + PositionInChunk,
                        new Vector3(0, 1, 1) + PositionInChunk,
                        new Vector3(1, 1, 1) + PositionInChunk
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0, 0),
                        
                        new Vector2(0, 0.5f),
                        new Vector2(0.333f, 0.5f),
                        new Vector2(0.333f, 0.5f),
                        
                        new Vector2(0.333f, 0),
                        new Vector2(0, 0),
                    });
                }
            }
        }

        public virtual bool BottomVisible
        {
            get { return bottomVisible; }
            set
            {
                bottomVisible = value;
                if (value == true)
                {
                    Verticies.AddRange(new List<Vector3>
                    {
                        new Vector3(0, 0, 0) + PositionInChunk,
                        new Vector3(1, 0, 0) + PositionInChunk,
                        new Vector3(1, 0, 1) + PositionInChunk,
                        new Vector3(1, 0, 1) + PositionInChunk,
                        new Vector3(0, 0, 1) + PositionInChunk,
                        new Vector3(0, 0, 0) + PositionInChunk
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0.333f, 0.5f + 0.5f),
                        
                        new Vector2(0, 0.5f + 0.5f),
                        new Vector2(0, 0 + 0.5f),
                        new Vector2(0, 0 + 0.5f),
                        new Vector2(0.333f, 0 + 0.5f),
                        new Vector2(0.333f, 0.5f + 0.5f)
                    });
                }
            }
        }

        public virtual bool FrontVisible
        {
            get { return frontVisible; }
            set
            {
                frontVisible = value;
                if (value == true)
                {
                    Verticies.AddRange(new List<Vector3>
                    {
                        new Vector3(1, 1, 0) + PositionInChunk,
                        new Vector3(1, 0, 0) + PositionInChunk,
                        new Vector3(0, 0, 0) + PositionInChunk,
                        new Vector3(0, 0, 0) + PositionInChunk,
                        new Vector3(0, 1, 0) + PositionInChunk,
                        new Vector3(1, 1, 0) + PositionInChunk
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0.333f + 0.333f, 0.5f),
                        new Vector2(0.333f + 0.333f, 0),                        
                        new Vector2(0 + 0.333f, 0),
                        new Vector2(0 + 0.333f, 0),
                        
                        new Vector2(0 + 0.333f, 0.5f),
                        new Vector2(0.333f + 0.333f, 0.5f),
                    });
                }
            }
        }

        public virtual bool BackVisible
        {
            get { return backVisible; }
            set
            {
                backVisible = value;
                if (value == true)
                {
                    Verticies.AddRange(new List<Vector3>
                    {
                        new Vector3(0, 0, 1) + PositionInChunk,
                        new Vector3(1, 0, 1) + PositionInChunk,
                        new Vector3(1, 1, 1) + PositionInChunk,
                        new Vector3(1, 1, 1) + PositionInChunk,
                        new Vector3(0, 1, 1) + PositionInChunk,
                        new Vector3(0, 0, 1) + PositionInChunk
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0.333f + 0.666f, 0.5f),
                        
                        new Vector2(0 + 0.666f, 0.5f),
                        new Vector2(0 + 0.666f, 0),
                        new Vector2(0 + 0.666f, 0),
                        new Vector2(0.333f + 0.666f, 0),
                        new Vector2(0.333f + 0.666f, 0.5f)
                    });
                }
            }
        }

        

        public Block(Vector3 posInChunk, Vector2 chunk)
        {
            PositionInChunk = posInChunk;
            PositionInWorld = new Vector3((chunk.x * 16) + posInChunk.x, posInChunk.y, (chunk.y * 16) + posInChunk.z);
            leftVisible = rightVisible = topVisible = bottomVisible = frontVisible = backVisible = false;
            Verticies = new List<Vector3>();
            UVs = new List<Vector2>();
        }

        public Block(Vector3 worldPos)
        {
            Vector2 chunk;
            Vector3 chunkPos;
            Coordinates.WorldPosToChunkPos(worldPos, out chunkPos, out chunk);
            PositionInChunk = chunkPos;
            PositionInWorld = worldPos;
            
            Verticies = new List<Vector3>();
            UVs = new List<Vector2>();
            SetAllSidesVisible();
        }

        public abstract void OnDestroyed();

        public abstract void OnPlaced();

        public virtual void OnTakeDamage(float damageAmount)
        {
            Damage -= damageAmount;
            if (Damage <= 0.0f)
            {
                OnDestroyed();
            }
        }

        public virtual void SetAllSidesVisible()
        {
            FrontVisible = LeftVisible = RightVisible = TopVisible = BottomVisible = BackVisible = true;
        }
    }
}
