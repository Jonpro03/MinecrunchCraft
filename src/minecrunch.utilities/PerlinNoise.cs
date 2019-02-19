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
        private Voronoi biomePerlin;
        const float BIOME_SIZE_FACTOR = 300;  // Todo: get these values from settings module
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
                Seed = 1, //Todo: get from settings
                //Frequency = 1,
                Lacunarity = 3,
                Quality = SharpNoise.NoiseQuality.Best,
                OctaveCount = 3,
                //Persistence = 1
            };

            biomePerlin = new Voronoi
            {
                Seed = 3,
                Frequency=3
            };

            cavePerlin = new Perlin
            {
                Seed = 326500087,
                Lacunarity = 1,
                Quality = SharpNoise.NoiseQuality.Standard,
                OctaveCount = 1
            };
        }

        public int Biome(int bx, int by, int bz)
        {
            double val = biomePerlin.GetValue(bx / BIOME_SIZE_FACTOR, by / BIOME_SIZE_FACTOR, bz / BIOME_SIZE_FACTOR) * 2;
            return (int) val > 1 ? 1 : (int) val;
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public int Terrain(float x, float z, int biome = 5)
        {
            int TerrainHeight = 50;
            //Generate a value from the given position, position is divided to make the noise more frequent.
            double perlin1 = terrainPerlin.GetValue(x * MAGIC_SEED_FACTOR1, 0, z * MAGIC_SEED_FACTOR1) * 10;
            double perlin2 = terrainPerlin.GetValue(x / 400, 0, z / 400) * 5;

            int result = (int) (perlin1 * perlin2) + TerrainHeight;
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
