using System;
using System.Collections.Generic;
using System.Linq;
using minecrunch.models.Generator;
using SharpNoise.Models;
using SharpNoise.Modules;

namespace minecrunch.models
{
    public class PerlinNoise
    {
        private Perlin cavePerlin;
        private Voronoi biomePerlin;
        private int biome_size;
        private float cave_fill;
        private float cave_height;
        private float cave_stretch;
        const float WORLD_HORZ_STRETCH_FACTOR = 1 / 128.0f; //0.000008925f;
        const float WORLD_VERT_STRETCH_FACTOR = 1;//0.018625f;

        WorldGenerationSettings WorldSettings;
        Module WorldGen;

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
            // Todo: get these settings from WorldGenerationSettings
            biome_size = 2 * 100;
            cave_fill = (20 / 100.0f * 0.70f) + 0.30f;
            cave_height = 50 / 100.0f * 0.25f;
            cave_stretch = 80 / 1000.0f * 0.8f;

            WorldSettings = new WorldGenerationSettings();
            WorldGen = new WorldGenerator(WorldSettings).CreateModule();

            biomePerlin = new Voronoi
            {
                Seed = 1,
                Frequency = 3
            };

            cavePerlin = new Perlin
            {
                Seed = 1,
                Lacunarity = 0,
                Quality = SharpNoise.NoiseQuality.Fast,
                OctaveCount = 1
            };
        }

        public void SetWorldGeneratorSettings(WorldGenerationSettings settings)
        {
            WorldGen = new WorldGenerator(settings).CreateModule();
            biomePerlin.Seed = settings.Seed;
            cavePerlin.Seed = settings.Seed;
        }

        public int Biome(int bx, int by, int bz)
        {
            double val = biomePerlin.GetValue(bx / biome_size, by / biome_size, bz / biome_size) * 2;
            return (int) val > 1 ? 1 : (int) val;
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public double Terrain(float x, float z)
        {
            int terrainOffset = 3;
            return terrainOffset 
                + WorldGen.GetValue(x * WORLD_HORZ_STRETCH_FACTOR, 0, z * WORLD_HORZ_STRETCH_FACTOR)
                * WORLD_VERT_STRETCH_FACTOR;
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
