using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utility;
using UnityEngine;


namespace Assets.Scripts.Blocks
{
    [Serializable]
    public abstract class Block : IDrawable
    {
        public virtual BlockIdentification BlockId { get; set; }

        public virtual Vector3 PositionInChunk { get; private set; }

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
                        new Vector2(0, 0.5f),
                        new Vector2(0.25f, 0.5f),
                        new Vector2(0.25f, 0),
                        new Vector2(0.25f, 0),
                        new Vector2(0, 0),
                        new Vector2(0, 0.5f),
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
                        new Vector3(1, 1, 0) + PositionInChunk,
                        new Vector3(1, 1, 1) + PositionInChunk,
                        new Vector3(1, 0, 1) + PositionInChunk,
                        new Vector3(1, 0, 1) + PositionInChunk,
                        new Vector3(1, 0, 0) + PositionInChunk,
                        new Vector3(1, 1, 0) + PositionInChunk,
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0.5f, 0.5f),                                         
                        new Vector2(0.75f, 0.5f),
                        new Vector2(0.75f, 0),
                        new Vector2(0.75f, 0),
                        new Vector2(0.5f, 0),                        
                        new Vector2(0.5f, 0.5f),
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
                        new Vector3(0, 1, 1) + PositionInChunk,
                        new Vector3(1, 1, 1) + PositionInChunk,
                        new Vector3(1, 1, 0) + PositionInChunk,
                        new Vector3(1, 1, 0) + PositionInChunk,
                        new Vector3(0, 1, 0) + PositionInChunk,
                        new Vector3(0, 1, 1) + PositionInChunk
                    });

                    UVs.AddRange(new List<Vector2>
                    {
                        new Vector2(0.25f + 0.25f, 0.5f + 0.5f),
                        new Vector2(0 + 0.25f, 0.5f + 0.5f),
                        new Vector2(0 + 0.25f, 0 + 0.5f),
                        new Vector2(0 + 0.25f, 0 + 0.5f),
                        new Vector2(0.25f + 0.25f, 0 + 0.5f),
                        new Vector2(0.25f + 0.25f, 0.5f + 0.5f),
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
                        new Vector2(0.25f, 0.5f + 0.5f),
                        
                        new Vector2(0, 0.5f + 0.5f),
                        new Vector2(0, 0 + 0.5f),
                        new Vector2(0, 0 + 0.5f),
                        new Vector2(0.25f, 0 + 0.5f),
                        new Vector2(0.25f, 0.5f + 0.5f)
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
                        new Vector2(0.25f + 0.25f, 0.5f),
                        new Vector2(0.25f + 0.25f, 0),                        
                        new Vector2(0 + 0.25f, 0),
                        new Vector2(0 + 0.25f, 0),                        
                        new Vector2(0 + 0.25f, 0.5f),
                        new Vector2(0.25f + 0.25f, 0.5f),
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
                        new Vector2(0.5f, 1),
                        new Vector2(0.75f, 1),
                        new Vector2(0.75f, 0.5f),
                        new Vector2(0.75f, 0.5f),
                        new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 1),
                    });
                }
            }
        }

        public Block(Vector3 posInChunk)
        {
            PositionInChunk = posInChunk;
            leftVisible = rightVisible = topVisible = bottomVisible = frontVisible = backVisible = false;
            Verticies = new List<Vector3>();
            UVs = new List<Vector2>();
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

        public virtual bool IsVisible()
        {
            return FrontVisible || BackVisible || LeftVisible || RightVisible || TopVisible || BottomVisible;
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
