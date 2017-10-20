using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(WorldTerrain))]
    public class World : MonoBehaviour
    {
        public static string Seed { get; set; }

        public static int SeedHash { get; private set; }

        public static WorldTerrain WorldTerrain { get; private set; }

        public static List<Player.WorldPlayer> Players;

        public static string WorldName = "world";

        public static string WorldSaveFolder;

        private void Awake()
        {
            Seed = Seed ?? DateTime.Now.ToString("h:ss");
            SeedHash = Seed.GetHashCode();
            WorldTerrain = transform.GetComponent<WorldTerrain>();
            Players = FindObjectsOfType<Player.WorldPlayer>().ToList();
            WorldSaveFolder = Application.persistentDataPath + "/" + WorldName;

            if (!Directory.Exists(WorldSaveFolder))
            {
                Directory.CreateDirectory(WorldSaveFolder);
            }
        }

        
    }
}
