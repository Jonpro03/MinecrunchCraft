using UnityEngine;
using Assets.Scripts.Chunks;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;
using System.Linq;
using Assets.Scripts.Utility;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace Assets.Scripts.World
{
    public class WorldTerrain : MonoBehaviour
    {
        //Public variable for the size of the terrain, width and heigth
        public int RenderSize = 5;

        private ChunkJobManager chunkJobs;

        private List<Chunk> Chunks { get; set; }

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
                    Chunk chunk = LoadChunk(new Vector2(cx, cz));
                    if (null == chunk)
                    {
                        GenerateChunk(new Vector2(cx, cz));

                    }
                    else
                    {
                        DrawChunk(chunk);
                        Chunks.Add(chunk);
                    }
                }
            }

            InvokeRepeating("GenerateNewChunksAroundPlayers", 1, 2);

            // Debug block        
            GameObject debugBlockGo = new GameObject("debugBlock");
            IEntity debugBlockEntity = debugBlockGo.AddComponent<BlockEntity>();
            debugBlockEntity.Block = new GlassBlock(new Vector3(0, 64, 0));

        }

        private void Update()
        {
            chunkJobs.Update();
            foreach (Chunk chunk in chunkJobs.CompletedJobs)
            {
                DrawChunk(chunk);
                Chunks.Add(chunk);
                SaveChunk(chunk);
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
                        Chunk chunk = LoadChunk(chunkCoord);
                        if (null == chunk)
                        {
                            GenerateChunk(chunkCoord);
                        }
                        else
                        {
                            DrawChunk(chunk);
                            Chunks.Add(chunk);
                        }
                    }
                }
            }
        }

        private void SaveChunk(Chunk chunk)
        {
            ChunkData chunkData = new ChunkData()
            {
                ChunkPositionX = (int) chunk.ChunkPosition.x,
                ChunkPositionZ = (int) chunk.ChunkPosition.y,
                Biome = chunk.Biome,
                Blocks = chunk.Blocks,
                Materials = chunk.Materials,
                Triangles = chunk.Triangles,
                Verticies = chunk.Verticies,
                UVs = chunk.UVs
            };

            string chunkFilePath = String.Format("{0}/chunks/{1},{2}.dat", World.WorldSaveFolder, chunk.ChunkPosition.x, chunk.ChunkPosition.y);
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector ss = new SurrogateSelector();
            ss.AddSurrogate(
                typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3SerializationSurrogate());
            ss.AddSurrogate(
                typeof(Vector2),
                new StreamingContext(StreamingContextStates.All), 
                new Vector2SerializationSurrogate());
            formatter.SurrogateSelector = ss;
            using (FileStream chunkFile = File.Create(chunkFilePath))
            {
                formatter.Serialize(chunkFile, chunkData);
            }
        }

        private Chunk LoadChunk(Vector2 chunkCoord)
        {
            string chunkFilePath = String.Format("{0}/chunks/{1},{2}.dat", World.WorldSaveFolder, chunkCoord.x, chunkCoord.y);
            if (!File.Exists(chunkFilePath))
            {
                return null;
            }
            ChunkData data;
            BinaryFormatter formatter = new BinaryFormatter();
            SurrogateSelector ss = new SurrogateSelector();
            ss.AddSurrogate(
                typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3SerializationSurrogate());
            ss.AddSurrogate(
                typeof(Vector2),
                new StreamingContext(StreamingContextStates.All),
                new Vector2SerializationSurrogate());
            formatter.SurrogateSelector = ss;
            using (FileStream chunkFile = File.OpenRead(chunkFilePath))
            {
                data = (ChunkData)formatter.Deserialize(chunkFile);
            }

            GameObject chunkGameObject = new GameObject(string.Format("chunk{0},{1}", data.ChunkPositionX, data.ChunkPositionZ));
            Chunk chunk = chunkGameObject.AddComponent<Chunk>();
            chunk.InitializeChunk(new Vector2(data.ChunkPositionX, data.ChunkPositionZ));
            chunk.Biome = data.Biome;
            chunk.Generated = true;
            chunk.Blocks = data.Blocks;
            chunk.Materials = data.Materials;
            chunk.Triangles = data.Triangles;
            chunk.Verticies = data.Verticies;
            chunk.UVs = data.UVs;

            return chunk;
        }

        private void GenerateChunk(Vector2 chunkPos)
        {
            if (ChunkExists(chunkPos) && GetChunk(chunkPos).Generated)
            {
                return;
            }

            GameObject chunkGameObject = new GameObject(string.Format("chunk{0}", chunkPos.ToString()));
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

            if (!ChunkExists(chunk.ChunkPosition))
            {
                chunkGameObject.transform.position = new Vector3(chunk.ChunkPosition.x * 16, 0, chunk.ChunkPosition.y * 16);

                mats = new Material[chunk.Materials.Count];

                foreach (int key in chunk.Materials.Keys)
                {
                    mats[key] = Resources.Load<Material>(chunk.Materials[key]);
                }

                meshRenderer = chunkGameObject.AddComponent<MeshRenderer>();
            }
            else
            {
                // Todo this
                chunk.IsDrawn = true;
                return;

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

            Chunk chunkComponent = chunkGameObject.AddComponent(typeof(Chunk)) as Chunk;
            chunkComponent = chunk;
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

        [Serializable]
        public class ChunkData
        {
            public int ChunkPositionX { get; set; }
            public int ChunkPositionZ { get; set; }

            public float Biome { get; set; }

            public IBlock[,,] Blocks { get; set; }

            public Dictionary<int, string> Materials { get; set; }

            public Dictionary<int, List<int>> Triangles { get; set; }

            public List<Vector3> Verticies { get; set; }

            public List<Vector2> UVs { get; set; }
        }
    }
}