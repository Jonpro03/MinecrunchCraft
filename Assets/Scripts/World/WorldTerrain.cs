using UnityEngine;
using Assets.Scripts.Chunks;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;
using System.Linq;
using System;
using System.IO;
using minecrunch.utilities;
using Assets.Scripts.Utility;

namespace Assets.Scripts.World
{
    public class WorldTerrain : MonoBehaviour
    {
        //Public variable for the size of the terrain, width and heigth
        public int RenderSize = 5;

        private static ChunkJobManager chunkJobs;

        public static List<Chunk> Chunks { get; set; }

        //Generates the terrain
        private void Start()
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

            //GenerateChunk(Vector2.zero);
            //LoadChunk(Vector2.zero);

            InvokeRepeating("GenerateNewChunksAroundPlayers", 10, 6);

            // Debug block        
            GameObject debugBlockGo = new GameObject("debugBlock");
            BlockEntity debugBlockEntity = debugBlockGo.AddComponent<BlockEntity>();
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

        private GameObject GetTree()
        {
            GameObject treeGo = new GameObject("Tree");
            for (var y=0; y<6; y++)
            {
                var oak = new GameObject(y.ToString());
                var oakEntity = oak.AddComponent<BlockEntity>();
                oakEntity.Block = new OakWoodBlock(new Vector3(3, y, 3));
                oak.transform.parent = treeGo.transform;
            }

            for (int x=0; x<6; x++)
            {
                for (int y=4; y<7; y++)
                {
                    for (int z=0; z<6; z++)
                    {
                        var leaves = new GameObject();
                        var leavesEntity = leaves.AddComponent<BlockEntity>();
                        leavesEntity.Block = new OakWoodLeaves(new Vector3(x, y, z));
                        leavesEntity.transform.parent = treeGo.transform;
                    }
                }
            }

            return treeGo;
        }

        private void FixedUpdate()
        {
            chunkJobs.Update();
            foreach (Chunk chunk in chunkJobs.CompletedJobs)
            {
                if (chunk == null || chunk.gameObject == null)
                {
                    return;
                }
                DrawChunk(chunk);
                if (!chunk.IsSerialized)
                {
                    SaveChunk(chunk);
                }
                // Add some trees to this chunk.
                for (int x=0; x<2; x++)
                {
                    int randX = UnityEngine.Random.Range(0, 10) + (int) chunk.WorldPosition.x;
                    int randZ = UnityEngine.Random.Range(0, 10) + (int) chunk.WorldPosition.y;
                    int startingY = minecrunch.utilities.PerlinNoise.Instance.Terrain(randX, randZ, (int) chunk.Biome);

                    var tree = GetTree();
                    tree.transform.position = new Vector3(randX, startingY-1, randZ);
                    tree.transform.parent = chunk.gameObject.transform;

                }

                
            }
            chunkJobs.CompletedJobs.RemoveAll(c => c.IsDrawn);
        }

        private void OnApplicationQuit()
        {
            chunkJobs.StopAllJobs();
        }

        public void GenerateNewChunksAroundPlayers()
        {
            int UpdateDistance = RenderSize;
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

                List<Chunk> toRemove = Chunks.Where(c => c.Verticies.Count == 0).ToList();
                toRemove.AddRange(Chunks.Where(c => !neededChunks.Any(v2 => v2 == c.ChunkPosition)));

                toRemove.ForEach(c => {
                    Destroy(c.gameObject);
                    Chunks.Remove(c);
                    });

                //Chunks.RemoveAll(c => c.Verticies.Count == 0);

                foreach (var chunkCoord in neededChunks)
                {
                    if (!ChunkExists(chunkCoord))
                    {
                        GenerateChunk(chunkCoord);
                    }
                }
                toRemove.ForEach(c => {
                    Destroy(c.gameObject);
                    Chunks.Remove(c);
                });
            }
        }

        public static Block GetBlockRef(Vector3 worldPos)
        {
            Vector3 blockRelativePos;
            Vector2 chunkPos;
            Coordinates.WorldPosToChunkPos(worldPos, out blockRelativePos, out chunkPos);
            Chunk chunk = Chunks.FirstOrDefault(c => c.ChunkPosition == chunkPos);
            if (null == chunk)
            {
                return null;
            }
            return (Block) chunk.Blocks[(int)blockRelativePos.x, (int)blockRelativePos.y, (int)blockRelativePos.z].Clone();
        }

        public static bool UpdateBlock(Block blockRef)
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
            return;
            chunkJobs.AddChunkSaveJob(chunk);
        }

        private bool LoadChunk(Vector2 chunkCoord)
        {
            return false;
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
            chunk.Biome = minecrunch.utilities.PerlinNoise.Instance.Biome((int) chunkPos.x, (int) chunkPos.y);
            Chunks.Add(chunk);
            chunkJobs.AddGenerateJob(chunk);
        }

        private void DrawChunk(Chunk chunk)
        {
            if (chunk == null || chunk.gameObject == null)
            {
                return;
            }
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