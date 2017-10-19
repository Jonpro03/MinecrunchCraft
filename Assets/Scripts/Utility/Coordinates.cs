﻿using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class Coordinates
    {
        public static void WorldPosToChunkPos(Vector3 worldPos, out Vector3 chunkPos, out Vector2 chunk)
        {
            chunk = new Vector2((int)worldPos.x / 16, (int)worldPos.z / 16);
            chunkPos = new Vector3(
                worldPos.x - (chunk.x * 16),
                worldPos.y,
                worldPos.z - (chunk.y * 16));
        }
    }
}
