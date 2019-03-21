﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Chunks;
using Assets.Scripts.Utility;
using minecrunch.models.Chunks;
using System.Diagnostics;

namespace Assets.Scripts.World
{
    public class WorldTerrain : MonoBehaviour
    {
        //Public variable for the size of the terrain, width and heigth
        public int RenderSize = 5;

        private static ChunkJobManager chunkJobs;

        public static List<Chunk> Chunks { get; set; }

        public static HashSet<Vector2Int> InProgressChunks { get; set; }
        public static HashSet<Vector2Int> LoadedChunks { get; set; }

        //private string host = "https://minecrunchserver20190302073446.azurewebsites.net";
        private string host = "http://localhost:5000";
        //private string host = "http://localhost:55163";

        private Vector2Int lastKnownPlayerChunk = new Vector2Int(0, 0);

        //Generates the terrain
        private void Start()
        {

            if (!Directory.Exists(World.WorldSaveFolder + "/chunks"))
            {
                Directory.CreateDirectory(World.WorldSaveFolder + "/chunks");
                Directory.CreateDirectory(World.WorldSaveFolder + "/meshes");
            }

            Chunks = new List<Chunk>();
            InProgressChunks = new HashSet<Vector2Int>();
            LoadedChunks = new HashSet<Vector2Int>();
            chunkJobs = new ChunkJobManager();

            InvokeRepeating("ChunkMaintanence", 0, 0.5f);
            InvokeRepeating("GenerateChunksAroundPlayer", 0, 5);
        }

        private void OnGUI()
        {
            // Show chunk pipeline status
            GUI.Label(new Rect(10, 10, 1000, 20), string.Format(
                "Q:{0} D:{1} C:{1}",
                InProgressChunks.Count,
                chunkJobs.ChunkDownloads.Count,
                chunkJobs.ChunkCalcVerticiesTasks.Count
                ));
        }

        private void FixedUpdate()
        {
            // Check if there are chunks ready to render
            if (chunkJobs.CompletedChunks.Count > 0)
            {
                var chunk = chunkJobs.CompletedChunks.First();
                if (!DrawChunk(chunk))
                {
                    chunkJobs.CompletedChunks.Remove(chunk);
                    return;
                }
                InProgressChunks.Remove(new Vector2Int(chunk.x,chunk.y));
                chunkJobs.CompletedChunks.Remove(chunk);
                LoadedChunks.Add(new Vector2Int(chunk.x, chunk.y));
                Chunks.Add(chunk);
            }
        }

        private void ChunkMaintanence()
        {
            // Sort the chunks to draw the ones nearest the player.
            var player = World.Players.First();
            var playerChunkLoc = Coordinates.ChunkPlayerIsIn(player.transform.position);
            if (playerChunkLoc != lastKnownPlayerChunk)
            {
                chunkJobs.ChunkDownloads.Sort((chunk1, chunk2) =>
                {
                    return Math.Abs(playerChunkLoc.x - chunk1.cx) +
                    Math.Abs(playerChunkLoc.y - chunk1.cy) <
                    Math.Abs(playerChunkLoc.x - chunk2.cx) +
                    Math.Abs(playerChunkLoc.y - chunk2.cy) ? -1 : 1;
                });

                chunkJobs.ChunkCalcVerticiesTasks.Sort((chunk1, chunk2) =>
                {
                    return Math.Abs(playerChunkLoc.x - chunk1.chunk.x) +
                    Math.Abs(playerChunkLoc.y - chunk1.chunk.y) <
                    Math.Abs(playerChunkLoc.x - chunk2.chunk.x) +
                    Math.Abs(playerChunkLoc.y - chunk2.chunk.y) ? -1 : 1;
                });
                lastKnownPlayerChunk = playerChunkLoc;
            }

            chunkJobs.Update();
        }

        private void OnApplicationQuit()
        {
            chunkJobs.StopAllJobs();
        }

        public void GenerateChunksAroundPlayer()
        {
            foreach (Player.WorldPlayer player in World.Players)
            {
                var watch = Stopwatch.StartNew();
                var playerChunkLoc = Coordinates.ChunkPlayerIsIn(player.transform.position);
                HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();

                for (var a = 0; a <= RenderSize; a++)
                {
                    for (var b = 0; b <= RenderSize; b++)
                    {
                        var loc = playerChunkLoc + (Vector2Int.up * a) + (Vector2Int.right * b);

                        neededChunks.Add(playerChunkLoc + (Vector2Int.up * a) + (Vector2Int.right * b));
                        neededChunks.Add(playerChunkLoc + (Vector2Int.up * a) + (Vector2Int.left * b));
                        neededChunks.Add(playerChunkLoc + (Vector2Int.down * a) + (Vector2Int.right * b));
                        neededChunks.Add(playerChunkLoc + (Vector2Int.down * a) + (Vector2Int.left * b));
                    }
                }
                watch.Stop();
                UnityEngine.Debug.Log($"neededChunk: {watch.ElapsedMilliseconds}");

                watch = Stopwatch.StartNew();
                var chunksToLoad = new HashSet<Vector2Int>(neededChunks.Except(LoadedChunks));
                chunksToLoad.ExceptWith(InProgressChunks);

                foreach (var chunkCoord in chunksToLoad)
                {
                    string chunkName = $"chunk{chunkCoord.x},{chunkCoord.y}";

                    // Cleared for departure.
                    InProgressChunks.Add(chunkCoord);
                    chunkJobs.ChunkDownloads.Add(new ChunkDownloadTask(host, "world1", chunkCoord.x, chunkCoord.y));
                }
                watch.Stop();
                UnityEngine.Debug.Log($"startDownloads: {watch.ElapsedMilliseconds}");

                watch = Stopwatch.StartNew();
                // Comment this section to disable removing chunks no longer near the player.
                // Usefull when testing terrain gen.
                List<Chunk> toRemove = Chunks.Where(c => !neededChunks.Any(v2 => v2.x == c.x && v2.y == c.y)).ToList();

                toRemove.ForEach(c => {
                    Destroy(GameObject.Find(c.name));
                    Chunks.Remove(c);
                    LoadedChunks.Remove(new Vector2Int(c.x, c.y));
                    chunkJobs.ChunkDownloads.RemoveAll(t => t.cx == c.x && t.cy == c.y);
                    // Todo: remove chunks from the chunk queues that are no longer needed.
                });
                watch.Stop();
                UnityEngine.Debug.Log($"cleanup: {watch.ElapsedMilliseconds}");
            }
        }

        private bool DrawChunk(Chunk chunk)
        {
            var watch = Stopwatch.StartNew();
            GameObject chunkGameObject = GameObject.Find(chunk.name);
            if (chunkGameObject is null)
            {
                chunkGameObject = new GameObject(chunk.name);
            }
            
            chunkGameObject.transform.position = new Vector3(chunk.x * 16, 0, chunk.y * 16);

            foreach(var section in chunk.sections)
            {
                GameObject subChunkGameObject = new GameObject(chunk.name + "-" + section.name);
                subChunkGameObject.transform.SetParent(chunkGameObject.transform);
                subChunkGameObject.transform.localPosition = new Vector3(0, 0, 0);
                if (!DrawSubChunk(section, subChunkGameObject))
                {
                    Destroy(chunkGameObject);
                    return false;
                }
            }
            watch.Stop();
            //UnityEngine.Debug.Log($"Chunk times {chunk.terrainTimeMs} : {chunk.blockFaceTimeMs} : {chunk.verticieTimeMs} : {watch.ElapsedMilliseconds}");
            return true;
        }

        private bool DrawSubChunk(ChunkSection section, GameObject subChunkGameObject)
        {   
            if (section.Mesh.Materials.Count.Equals(0))
            {
                return true;
            }

            Material[] mats = new Material[section.Mesh.Materials.Count];
            MeshRenderer meshRenderer;
            Mesh subchunkMesh = new Mesh();

            foreach (int key in section.Mesh.Materials.Keys)
            {
                mats[key] = Resources.Load<Material>(section.Mesh.Materials[key]);
            }

            if (subChunkGameObject.GetComponents<MeshRenderer>().Count().Equals(0))
            {
                meshRenderer = subChunkGameObject.AddComponent<MeshRenderer>();
            }
            else
            {
                meshRenderer = subChunkGameObject.GetComponent<MeshRenderer>();
            }

            meshRenderer.materials = mats;
            subchunkMesh = new Mesh();
            subchunkMesh.name = section.name + "mesh";
            subchunkMesh.subMeshCount = section.Mesh.Materials.Count;
            subchunkMesh.SetVertices(section.Mesh.Verticies);

            if (subchunkMesh.vertices.Count() != section.Mesh.UVs.Count)
            {
                UnityEngine.Debug.LogError("Failed to set verticies!");
                // Todo: Throttle the render rate in the chunk manager.
                return false;
            }

            foreach (int key in section.Mesh.Quads.Keys)
            {
                try
                {
                    subchunkMesh.SetIndices(section.Mesh.Quads[key].ToArray(), MeshTopology.Quads, key);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError("Failed to set indicies!");
                    return false;
                }

            }
            subchunkMesh.SetUVs(0, section.Mesh.UVs);

            var meshFilter = subChunkGameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = subchunkMesh;

            subchunkMesh.RecalculateNormals();
            subChunkGameObject.AddComponent<MeshCollider>();

            return true;
        }
    }
}
