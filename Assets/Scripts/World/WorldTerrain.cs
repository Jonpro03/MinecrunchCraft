using UnityEngine;
using Assets.Scripts.Chunks;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;
using System.Linq;
using Assets.Scripts.Utility;
using System;
using System.IO;

namespace Assets.Scripts.World
{
    public class WorldTerrain : MonoBehaviour
    {
        //Public variable for the size of the terrain, width and heigth
        public int RenderSize = 5;

        private static ChunkJobManager chunkJobs;

        public static List<Chunk> Chunks { get; set; }

        //Generates the terrain
        private void Awake()
        {

            if (!Directory.Exists(World.WorldSaveFolder + "/chunks"))
            {
                Directory.CreateDirectory(World.WorldSaveFolder + "/chunks");
            }

            Chunks = new List<Chunk>();
            chunkJobs = new ChunkJobManager();

            Vector2 playerLoc = Coordinates.ChunkPlayerIsIn(transform.position);

            // Create the chunks
            for (int cx = (-1 * RenderSize) + (int)playerLoc.x; cx < RenderSize; cx++)
            {
                for (int cz = (-1 * RenderSize) + (int)playerLoc.y; cz < RenderSize; cz++)
                {
                    Vector2 chunkCoord = new Vector2(cx, cz);
                    bool chunkOnDisk = LoadChunk(new Vector2(cx, cz));
                    if (!chunkOnDisk)
                    {
                        GenerateChunk(chunkCoord);
                    }
                }
            }


            InvokeRepeating("GenerateNewChunksAroundPlayers", 5, 5);

            // Debug block        
            GameObject debugBlockGo = new GameObject("debugBlock");
            IEntity debugBlockEntity = debugBlockGo.AddComponent<BlockEntity>();
            debugBlockEntity.Block = new GlassBlock(new Vector3(0, 64, 0));

        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 1000, 20), String.Format(
                "Generate:{0} Update:{1} Load:{2} Total:{3}",
                chunkJobs.ChunkGenerateJobs.Count,
                chunkJobs.ChunkUpdateJobs.Count,
                chunkJobs.ChunkLoadJobs.Count,
                Chunks.Count
                ));
        }

        private void FixedUpdate()
        {
            chunkJobs.Update();
            foreach (Chunk chunk in chunkJobs.CompletedJobs)
            {
                Chunks.Add(chunk);
                DrawChunk(chunk);
                if (!chunk.IsSerialized)
                {
                    SaveChunk(chunk);
                }
            }
            chunkJobs.CompletedJobs.RemoveAll(c => c.IsDrawn);
        }

        public void GenerateNewChunksAroundPlayers()
        {
            const int UpdateDistance = 2;
            foreach (Player.WorldPlayer player in World.Players)
            {
                var playerPos = Coordinates.ChunkPlayerIsIn(player.transform.position);
                List<Vector2> neededChunks = new List<Vector2>();

                for (var a = 0; a <= UpdateDistance; a++)
                {
                    for (var b = 0; b <= UpdateDistance; b++)
                    {
                        neededChunks.Add(playerPos + (Vector2.up * a) + (Vector2.right * b));
                        neededChunks.Add(playerPos + (Vector2.up * a) + (Vector2.left * b));
                        neededChunks.Add(playerPos + (Vector2.down * a) + (Vector2.right * b));
                        neededChunks.Add(playerPos + (Vector2.down * a) + (Vector2.left * b));
                    }
                }

                foreach (var chunkCoord in neededChunks)
                {
                    if (!ChunkExists(chunkCoord))
                    {
                        bool chunkOnDisk = LoadChunk(chunkCoord);
                        if (!chunkOnDisk)
                        {
                            GenerateChunk(chunkCoord);
                        }
                    }
                }
            }
        }

        public static IBlock GetBlockRef(Vector3 worldPos)
        {
            Vector3 blockRelativePos;
            Vector2 chunkPos;
            Coordinates.WorldPosToChunkPos(worldPos, out blockRelativePos, out chunkPos);
            Chunk chunk = Chunks.FirstOrDefault(c => c.ChunkPosition == chunkPos);
            if (null == chunk)
            {
                return null;
            }
            return (IBlock) chunk.Blocks[(int)blockRelativePos.x, (int)blockRelativePos.y, (int)blockRelativePos.z].Clone();
        }

        public static bool UpdateBlock(IBlock blockRef)
        {
            Vector2 chunkPos;
            Vector3 blockPosInChunk;
            Coordinates.WorldPosToChunkPos(blockRef.PositionInWorld, out blockPosInChunk, out chunkPos);
            Chunk chunk = Chunks.FirstOrDefault(c => c.ChunkPosition == chunkPos);
            if (null == chunk)
            {
                return false;
            }
            chunk.Blocks[(int)blockPosInChunk.x, (int)blockPosInChunk.y, (int)blockPosInChunk.z] = blockRef;
            return true;
        }

        public static void ScheduleChunkUpdate(Chunk chunk)
        {
            chunk.HasUpdate = true;
            chunkJobs.AddUpdateJob(chunk);
        }

        public static void ScheduleChunkUpdate(Vector2 chunkLoc)
        {
            Chunk chunk = Chunks.FirstOrDefault(c => c.ChunkPosition == chunkLoc);
            if (null != chunk)
            {
                ScheduleChunkUpdate(chunk);
            }
        }

        private void SaveChunk(Chunk chunk)
        {
            //chunkJobs.AddChunkSaveJob(chunk);
        }

        private bool LoadChunk(Vector2 chunkCoord)
        {
            string chunkFilePath = String.Format("{0}/chunks/{1},{2}.dat", World.WorldSaveFolder, chunkCoord.x, chunkCoord.y);
            if (!File.Exists(chunkFilePath))
            {
                return false;
            }

            GameObject chunkGameObject = new GameObject(string.Format("chunk{0}loaded", chunkCoord.ToString()));
            Chunk chunk = chunkGameObject.AddComponent<Chunk>();
            if (!chunkJobs.AddChunkLoadJob(chunkCoord, chunk))
            {
                Destroy(chunkGameObject);
            }
            return true;
        }

        private void GenerateChunk(Vector2 chunkPos)
        {
            if (ChunkExists(chunkPos))
            {
                return;
            }

            GameObject chunkGameObject = new GameObject(string.Format("chunk{0}generated", chunkPos.ToString()));
            Chunk chunk = chunkGameObject.AddComponent<Chunk>();
            chunk.InitializeChunk(chunkPos);
            chunk.Biome = PerlinNoise.Biome(chunkPos, World.SeedHash);
            chunkJobs.AddGenerateJob(chunk);
        }

        private void DrawChunk(Chunk chunk)
        {
            GameObject chunkGameObject = chunk.gameObject;
            Material[] mats;
            MeshRenderer meshRenderer;
            Mesh chunkMesh = new Mesh();

            if (chunk.IsDrawn)
            {
                return;
            }

            chunkGameObject.transform.position = new Vector3(chunk.ChunkPosition.x * 16, 0, chunk.ChunkPosition.y * 16);
            mats = new Material[chunk.Materials.Count];

            foreach (int key in chunk.Materials.Keys)
            {
                mats[key] = Resources.Load<Material>(chunk.Materials[key]);
            }

            if (chunkGameObject.GetComponents<MeshRenderer>().Count().Equals(0))
            {
                meshRenderer = chunkGameObject.AddComponent<MeshRenderer>();
            }
            else
            {
                meshRenderer = chunkGameObject.GetComponent<MeshRenderer>();
            }

            meshRenderer.materials = mats;
            chunkMesh = new Mesh();
            chunkMesh.Clear();
            chunkMesh.name = "TerrainMesh";
            chunkMesh.SetVertices(chunk.Verticies.ToList());
            chunkMesh.subMeshCount = chunk.Materials.Count;
            chunkMesh.SetUVs(0, chunk.UVs.ToList());

            foreach (int key in chunk.Triangles.Keys)
            {
                chunkMesh.SetTriangles(chunk.Triangles[key].ToList(), key);
            }

            var meshFilter = chunkGameObject.AddComponent<MeshFilter>();

            meshFilter.mesh = chunkMesh;
            chunkMesh.RecalculateNormals();
            chunkGameObject.AddComponent<MeshCollider>();
            chunk.IsDrawn = true;
        }

        private Chunk GetChunk(Vector2 chunkLoc)
        {
            return Chunks.FirstOrDefault(x => x.ChunkPosition == chunkLoc);
        }

        private bool ChunkExists(Vector2 chunkLoc)
        {
            return Chunks.Any(x => x.ChunkPosition == chunkLoc);
        }
    }
}