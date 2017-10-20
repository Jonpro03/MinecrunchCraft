using UnityEngine;
using System.Collections;
using Assets.Scripts.Chunks;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Blocks;
using System.Linq;
using Assets.Scripts.Utility;
using System;

namespace Assets.Scripts.World
{
    public class WorldTerrain : MonoBehaviour
    {
        //Public variable for the size of the terrain, width and heigth
        public int RenderSize = 5;

        private ChunkJobManager chunkJobs;

        private List<Chunk> Chunks { get; set; }

        //Generates the terrain
        private void Start()
        {
            Chunks = new List<Chunk>();
            chunkJobs = new ChunkJobManager();

            Vector2 playerLoc = Coordinates.ChunkPlayerIsIn(transform.position);

            // if new world
            // Create the chunks
            for (int cx = (-1 * RenderSize) + (int)playerLoc.x; cx < RenderSize; cx++)
            {
                for (int cz = (-1 * RenderSize) + (int)playerLoc.y; cz < RenderSize; cz++)
                {
                    GenerateChunk(new Vector2(cx, cz));
                }
            }            

            InvokeRepeating("GenerateNewChunksAroundPlayer", 1, 2);

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
            }
            chunkJobs.CompletedJobs.RemoveAll(c => c.IsDrawn);
        }

        private void GenerateChunk(Vector2 chunkPos)
        {
            if (ChunkExists(chunkPos) && GetChunk(chunkPos).Generated)
            {
                return;
            }

            Chunk chunk = new Chunk(chunkPos)
            {
                Biome = PerlinNoise.Biome(chunkPos, World.SeedHash)
            };

            chunkJobs.AddGenerateJob(chunk);
        }

        public void GenerateNewChunksAroundPlayer()
        {
            const int UpdateDistance = 2;
            var playerPos = Coordinates.ChunkPlayerIsIn(World.Players[0].transform.position);
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
                    GenerateChunk(chunkCoord);
                }                
            }
        }

        private void DrawChunk(Chunk chunk)
        {
            GameObject chunkGameObject;
            Material[] mats;
            MeshRenderer meshRenderer;
            Mesh chunkMesh = new Mesh();

            if (!ChunkExists(chunk.ChunkPosition))
            {
                chunkGameObject = new GameObject(string.Format("chunk{0}", chunk.ChunkPosition.ToString()));
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
            chunk.chunkGameObject = chunkGameObject;
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
            var collider = chunkGameObject.AddComponent<MeshCollider>();
            Chunks.Add(chunk);
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