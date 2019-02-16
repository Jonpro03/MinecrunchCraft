using System;
using System.Collections.Generic;
using System.Linq;
using SharpNoise.Modules;

namespace minecrunch.utilities
{
    public sealed class PerlinNoise
    {
        private Perlin perlin;
        const float BIOME_SIZE_FACTOR = 200f;  // Todo: get these values from settings module
        const float MAGIC_SEED_FACTOR1 = 0.0145f;
        const float CAVE_FILL_PERCENT = 0.35f;
        const float CAVE_HEIGHT_FACTOR = 0.55f;
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
            perlin = new Perlin
            {
                Seed = 365449857, //Todo: get from settings
                //Frequency = 1,
                Lacunarity = 3,
                Quality = SharpNoise.NoiseQuality.Fast,
                OctaveCount = 3,
                //Persistence = 1
            };
        }

        public int Biome(int bx, int bz)
        {
            return (int) perlin.GetValue(bx / BIOME_SIZE_FACTOR, bz / BIOME_SIZE_FACTOR, 0);
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public int Terrain(float x, float z, int biome = 5)
        {
            int TerrainHeight = 50;
            //Generate a value from the given position, position is divided to make the noise more frequent.
            double perlin1 = perlin.GetValue(x * MAGIC_SEED_FACTOR1, z * MAGIC_SEED_FACTOR1, biome) * 10;

            int result = (int)perlin1 + TerrainHeight;
            return result < 0 ? 0 : result > 255 ? 255 : result;
        }

        public bool Cave(float x, float y, float z)
        {
            y *= CAVE_HEIGHT_FACTOR;
            x *= CAVE_STRETCH_FACTOR;
            z *= CAVE_STRETCH_FACTOR;

            List<double> Perlins = new List<double>()
            {
                perlin.GetValue(x, y, z),
                perlin.GetValue(x, z, y),
                perlin.GetValue(z, y, x),
                perlin.GetValue(z, x, y),
                perlin.GetValue(y, x, z),
                perlin.GetValue(y, z, x)
            };

            return Perlins.Average() < CAVE_FILL_PERCENT;
        }
    }
}
