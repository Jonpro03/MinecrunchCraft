using System;

namespace minecrunch.models.World
{
    [Serializable]
    public class World
    {
        public string Seed { get; set; }
        public int SpawnChunkX { get; set; }
        public int SpawnChunkY { get; set; }
    }
}
