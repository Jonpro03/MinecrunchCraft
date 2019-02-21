using System;
using System.Collections.Generic;
using System.Linq;
using SharpNoise.Modules;

namespace minecrunch.models
{
    public class PerlinNoise
    {
        private Perlin terrainPerlin;
        private Perlin cavePerlin;
        private Voronoi biomePerlin;
        private int biome_size;
        private float cave_fill;
        private float cave_height;
        private float cave_stretch;
        const float MAGIC_SEED_FACTOR1 = 0.0145f;

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
            biome_size = Properties.Settings.Default.BIOME_SIZE_FACTOR * 100;
            cave_fill = (Properties.Settings.Default.CAVE_FILL_PERCENT / 100.0f * 0.70f) + 0.30f;
            cave_height = Properties.Settings.Default.CAVE_HEIGHT_PERCENT / 100.0f * 0.25f;
            cave_stretch = Properties.Settings.Default.CAVE_STRETCH_PERCENT / 1000.0f * 0.8f;

            terrainPerlin = new Perlin
            {
                Seed = 10, //Todo: get from settings
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
                Seed = 3265087,
                Lacunarity = 0,
                Quality = SharpNoise.NoiseQuality.Fast,
                OctaveCount = 1
            };
        }

        public int Biome(int bx, int by, int bz)
        {
            double val = biomePerlin.GetValue(bx / biome_size, by / biome_size, bz / biome_size) * 2;
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
            y *= cave_height;
            x *= cave_stretch;
            z *= cave_stretch;
            double val = cavePerlin.GetValue(x, y, z);
            return !(val < cave_fill && val > 0);
        }
    }
}
