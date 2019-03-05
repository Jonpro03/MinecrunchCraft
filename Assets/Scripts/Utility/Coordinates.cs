using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class Coordinates
    {
        public static void WorldPosToChunkPos(Vector3 worldPos, out Vector3 chunkPos, out Vector2 chunk)
        {
            chunk = Vector2.zero;
            if (worldPos.x < 0)
            {
                chunk.x = Mathf.FloorToInt(worldPos.x / 16);
            }
            else
            {
                chunk.x = (int)worldPos.x / 16;
            }

            if (worldPos.y < 0)
            {
                chunk.y = Mathf.FloorToInt(worldPos.z / 16);
            }
            else
            {
                chunk.y = (int)worldPos.z / 16;
            }

            chunkPos = new Vector3(
                Mathf.Abs(worldPos.x - (chunk.x * 16)),
                worldPos.y,
                Mathf.Abs(worldPos.z - (chunk.y * 16)));
        }

        public static Vector2Int ChunkPlayerIsIn(Vector3 position)
        {
            return new Vector2Int((int)(position.x / 16.0f), (int)(position.z / 16.0f));
        }
    }
}
