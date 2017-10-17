using System;
using Assets.Scripts.Terrain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;

namespace UnitTests
{
    [TestClass]
    public class PerlinNoiseTests
    {
        [TestMethod]
        public void TestBiome()
        {
            float result = PerlinNoise.Biome(new Vector2(0, 0), 1234);
        }
    }
}
