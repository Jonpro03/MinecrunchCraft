using System;
using System.Collections.Generic;
using UnityEngine;

namespace minecrunch.game.models
{
    public class BlockBase : IBlockBase
    {
        public string BlockId { get; set; }
        public string Texture { get; set; }
        public int ChunkX { get; set; }
        public int ChunkY { get; set; }
        public int ChunkZ { get; set; }

        private bool leftVisible, rightVisible, topVisible, bottomVisible, frontVisible, backVisible;

        /// <summary>
        /// List of block verticies. These do not include
        /// the chunk offset, so be sure to add the position
        /// in the chunk to each Vector3.
        /// </summary>
        /// <value>The verticies.</value>
        public virtual List<Vector3> Verticies { get; private set; }
        public virtual List<Vector2> UVs { get; private set; }
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
                        new Vector3(0, 1, 1),
                        new Vector3(0, 1, 0),
                        new Vector3(0, 0, 0),
                        new Vector3(0, 0, 0),
                        new Vector3(0, 0, 1),
                        new Vector3(0, 1, 1),
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
                        new Vector3(1, 1, 0),
                        new Vector3(1, 1, 1),
                        new Vector3(1, 0, 1),
                        new Vector3(1, 0, 1),
                        new Vector3(1, 0, 0),
                        new Vector3(1, 1, 0),
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
                        new Vector3(0, 1, 1),
                        new Vector3(1, 1, 1),
                        new Vector3(1, 1, 0),
                        new Vector3(1, 1, 0),
                        new Vector3(0, 1, 0),
                        new Vector3(0, 1, 1)
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
                        new Vector3(0, 0, 0),
                        new Vector3(1, 0, 0),
                        new Vector3(1, 0, 1),
                        new Vector3(1, 0, 1),
                        new Vector3(0, 0, 1),
                        new Vector3(0, 0, 0)
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
                        new Vector3(1, 1, 0),
                        new Vector3(1, 0, 0),
                        new Vector3(0, 0, 0),
                        new Vector3(0, 0, 0),
                        new Vector3(0, 1, 0),
                        new Vector3(1, 1, 0)
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
                        new Vector3(0, 0, 1),
                        new Vector3(1, 0, 1),
                        new Vector3(1, 1, 1),
                        new Vector3(1, 1, 1),
                        new Vector3(0, 1, 1),
                        new Vector3(0, 0, 1)
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
    }
}
