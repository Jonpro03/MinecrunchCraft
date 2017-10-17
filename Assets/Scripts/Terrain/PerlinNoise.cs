using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Terrain
{
    public class PerlinNoise
    {
        private static float[] BiomeNoise = new float[]
        {
            0.0135f,
            0.0136f,
            0.0138f,
            0.0144f,
            0.0145f,
            0.0146f,
            0.0147f,
            0.0148f,
            0.0153f,
            0.016f
        };

        public static float Biome(Vector2 loc, int seedHash)
        {
            const float BIOME_SIZE_FACTOR = 200f;

            int digitsInHash = seedHash.ToString().Length;
            float seedDecimal = seedHash / Mathf.Pow(10, digitsInHash);
            return Mathf.PerlinNoise((loc.x + seedDecimal) / BIOME_SIZE_FACTOR, (loc.y + seedDecimal) / BIOME_SIZE_FACTOR)*10.0f;
        }

        //Function that inputs the position and spits out a float value based on the perlin noise
        public static int Terrain(float x, float z, int seedHash, float biome = 5.0f)
        {
            int digitsInHash = seedHash.ToString().Length;
            float seedDecimal = seedHash / Mathf.Pow(10, digitsInHash);
            float noise = 0.0029f*biome;
            int TerrainHeight = 50;
            //Generate a value from the given position, position is divided to make the noise more frequent.
            float pass1 = Mathf.PerlinNoise((x + seedDecimal) * 0.0145f, (z + seedDecimal) * 0.0145f) * 10;
            float pass2 = Mathf.PerlinNoise((x+z) * 0.0145f, (x-z) * 0.0145f) * biome;
            float pass3 = Mathf.PerlinNoise(((seedDecimal - x)), ((seedDecimal - z))) * 0.36f;

            //Return the noise value
            return (int)((pass1 * pass2 - pass3) + TerrainHeight);
        }

        public static bool Cave(Vector3 loc, int seedHash)
        {
            const float CAVEFILLPERCENT = 0.35f;
            const float CAVEHEIGHTFACTOR = 0.55f;
            const float STRETCHFACTOR = 0.0675f;

            int digitsInHash = seedHash.ToString().Length;            float seedDecimal = seedHash / Mathf.Pow(10, digitsInHash - 1);
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
