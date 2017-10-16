using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class Block : IBlock
    {
        public Vector3 PositionInChunk { get; private set; }

        public Vector3 PositionInWorld { get; private set; }

        public bool IsTopLevel { get; set; }

        public List<Vector3> Verticies { get; set; }

        public List<Vector2> UVs { get; set; }

        private bool leftVisible, rightVisible, topVisible, bottomVisible, frontVisible, backVisible;

        public bool LeftVisible
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

        public bool RightVisible
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

        public bool TopVisible
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

        public bool BottomVisible
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

        public bool FrontVisible
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

        public bool BackVisible
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

        public Block(Vector3 posInChunk, bool visible, Vector2 chunkLoc)
        {
            PositionInChunk = posInChunk;
            PositionInWorld = new Vector3((chunkLoc.x * 16) + posInChunk.x, posInChunk.y, (chunkLoc.y * 16) + posInChunk.z);
            IsTopLevel = visible;
            leftVisible = rightVisible = topVisible = bottomVisible = frontVisible = backVisible = false;
            Verticies = new List<Vector3>();
            UVs = new List<Vector2>();
        }

    }
}
