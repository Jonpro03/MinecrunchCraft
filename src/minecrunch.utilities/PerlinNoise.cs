using System;
using System.Collections.Generic;
using System.Linq;
using SharpNoise.Modules;

namespace minecrunch.utilities
{
    public class PerlinNoise
    {
        private Perlin terrainPerlin;
        private Perlin cavePerlin;
        const float BIOME_SIZE_FACTOR = 400;  // Todo: get these values from settings module
        const float MAGIC_SEED_FACTOR1 = 0.0145f;
        const float CAVE_FILL_PERCENT = 0.008f;
        const float CAVE_HEIGHT_FACTOR = 0.11f;
        const float CAVE_STRETCH_FACTOR = 0.0675f;

        private static readonly Lazy<PerlinNoise> lazy = new Lazy<PerlinNoise>(() => new PerlinNoise());

        public static PerlinNoise Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private PerlinNoise()
        {
            terrainPerlin = new Perlin
            {
                Seed = 365449857, //Todo: get from settings
                //Frequency = 1,
                Lacunarity = 3,
                Quality = SharpNoise.NoiseQuality.Fast,
                OctaveCount = 3,
                //Persistence = 1
            };

            cavePerlin = new Perlin
            {
                Seed = 326544987,
                Lacunarity = 1,
                Quality = SharpNoise.NoiseQuality.Standard,
                OctaveCount = 1
            };
        }

        public int Biome(int bx, int bz)
        {
            return (int) (terrainPerlin.GetValue(bx / BIOME_SIZE_FACTOR, bz / BIOME_SIZE_FACTOR, 0)*10) ;
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public int Terrain(float x, float z, int biome = 5)
        {
            int TerrainHeight = 50;
            //Generate a value from the given position, position is divided to make the noise more frequent.
            double perlin1 = terrainPerlin.GetValue(x * MAGIC_SEED_FACTOR1, z * MAGIC_SEED_FACTOR1, biome) * 10;

            int result = (int)perlin1 + TerrainHeight;
            return result < 0 ? 0 : result > 255 ? 255 : result;
        }

        public bool Cave(float x, float y, float z)
        {
            y *= CAVE_HEIGHT_FACTOR;
            x *= CAVE_STRETCH_FACTOR;
            z *= CAVE_STRETCH_FACTOR;

            return cavePerlin.GetValue(x, y, z) < CAVE_FILL_PERCENT;
        }
    }
}
