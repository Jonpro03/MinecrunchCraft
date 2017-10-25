using System.Collections.Generic;
using Assets.Scripts.Blocks;
using Assets.Scripts.Interfaces;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.Chunks
{
    public class BlockUpdateJob : ThreadedJob
    {
        private Vector3 blockWorldPosition;
        public List<Vector2> ChunksToRedraw;

        public BlockUpdateJob(Vector3 blockWorldLoc)
        {
            blockWorldPosition = blockWorldLoc;
        }

        protected override void ThreadFunction()
        {
            bool reqsRedraw = false;
            Block block = WorldTerrain.GetBlockRef(blockWorldPosition);
            int bx, by, bz;
            bx = (int) blockWorldPosition.x;
            by = (int) blockWorldPosition.y;
            bz = (int) blockWorldPosition.z;
            // Top
            if (!(WorldTerrain.GetBlockRef(new Vector3(bx, Mathf.Min(by + 1, 255), bz)) is AirBlock))
            {
                block.TopVisible = true;
                reqsRedraw = true;
            }

            // Bottom
            if (!(WorldTerrain.GetBlockRef(new Vector3(bx, Mathf.Max(by - 1, 0), bz)) is AirBlock))
            {
                block.BottomVisible = true;
                reqsRedraw = true;
            }

            // Left
            if (!(WorldTerrain.GetBlockRef(new Vector3(Mathf.Max(bx - 1, 0), by, bz)) is AirBlock))
            {
                block.LeftVisible = true;
                reqsRedraw = true;
            }

            // Right
            if (!(WorldTerrain.GetBlockRef(new Vector3(Mathf.Min(bx + 1, 15), by, bz)) is AirBlock))
            {
                block.RightVisible = true;
                reqsRedraw = true;
            }

            // Front
            if (!(WorldTerrain.GetBlockRef(new Vector3(bx, by, Mathf.Max(bz - 1, 0))) is AirBlock))
            {
                block.FrontVisible = true;
                reqsRedraw = true;
            }

            // Back
            if (!(WorldTerrain.GetBlockRef(new Vector3(bx, by, Mathf.Min(bz + 1, 15))) is AirBlock))
            {
                block.BackVisible = true;
                reqsRedraw = true;
            }

            if (reqsRedraw)
            {
                Vector2 chunkPos;
                Vector3 blockLocInChunk;
                Utility.Coordinates.WorldPosToChunkPos(blockWorldPosition, out blockLocInChunk, out chunkPos);
                WorldTerrain.ScheduleChunkUpdate(chunkPos);
            }
        }
    }
}
