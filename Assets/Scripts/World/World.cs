using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Chunks;
using UnityEngine;
using Assets.Scripts.Player;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(WorldTerrain))]
    public class World : MonoBehaviour
    {
        public static string Seed { get; set; }

        public static int SeedHash { get; private set; }

        public static WorldTerrain WorldTerrain { get; private set; }

        public static List<Player.Player> Players;

        private void Awake()
        {
            Seed = Seed ?? DateTime.Now.ToString("h:ss");
            SeedHash = Seed.GetHashCode();
            WorldTerrain = transform.GetComponent<WorldTerrain>();
            Players = FindObjectsOfType<Player.Player>().ToList();
        }

        
    }
}
