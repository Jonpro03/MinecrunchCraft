using System;
using SharpNoise.Modules;

namespace minecrunch.utilities
{
    public sealed class PerlinNoise
    {
        private Perlin perlin;
        const float BIOME_SIZE_FACTOR = 200f;  // Todo: this better

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
                Seed = 3, //Todo: fix this
                Frequency = 1,
                Lacunarity = 1,
                Quality = SharpNoise.NoiseQuality.Best,
                OctaveCount = 1,
                Persistence = 1
            };
        }

        public double Biome(int bx, int bz)
        {
            return perlin.GetValue(bx / BIOME_SIZE_FACTOR, bz / BIOME_SIZE_FACTOR, 0);
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public static int Terrain(float x, float z, int seedHash, float biome = 5.0f)
        {
            int digitsInHash = seedHash.ToString().Length;
            float seedDecimal = seedHash / Mathf.Pow(10, digitsInHash);
            int TerrainHeight = 50;
            //Generate a value from the given position, position is divided to make the noise more frequent.
            float pass1 = Mathf.PerlinNoise((x + seedDecimal) * 0.0145f, (z + seedDecimal) * 0.0145f) * 10;
            float pass2 = Mathf.PerlinNoise((x + z) * 0.0145f, (x - z) * 0.0145f) * biome;
            float pass3 = Mathf.PerlinNoise(((seedDecimal - x)), ((seedDecimal - z))) * 0.36f;

            //Return the noise value
            return (int)((pass1 * pass2 - pass3) + TerrainHeight);
        }

        public static bool Cave(Vector3 loc, int seedHash)
        {
            const float CAVEFILLPERCENT = 0.35f;
            const float CAVEHEIGHTFACTOR = 0.55f;
            const float STRETCHFACTOR = 0.0675f;

            int digitsInHash = seedHash.ToString().Length; float seedDecimal = seedHash / Mathf.Pow(10, digitsInHash - 1);
            loc.y /= CAVEHEIGHTFACTOR;

            float x = loc.x * STRETCHFACTOR / seedDecimal;
            float y = loc.y * STRETCHFACTOR / seedDecimal;
            float z = loc.z * STRETCHFACTOR / seedDecimal;

            List<float> Perlins = new List<float>()
            {
                Mathf.PerlinNoise(x, z),
                Mathf.PerlinNoise(y, z),
                Mathf.PerlinNoise(z, y),
                Mathf.PerlinNoise(x, y),
                Mathf.PerlinNoise(y, x),
                Mathf.PerlinNoise(z, x),
                Mathf.PerlinNoise(x, x),
                Mathf.PerlinNoise(y, y)
            };

            return Perlins.Average() < CAVEFILLPERCENT;
        }
    }
}
