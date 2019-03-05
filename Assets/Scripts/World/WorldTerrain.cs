using UnityEngine;
using Assets.Scripts.Chunks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Assets.Scripts.Utility;
using minecrunch.models.Chunks;
using minecrunch.models.Biomes;
using UnityEditor;
using minecrunch.models.runtime;
using System.Runtime.Serialization.Formatters.Binary;
using minecrunch.tasks;
using System;
using minecrunch.models.Runtime;
using System.Net;
using minecrunch.models;
using System.Threading.Tasks;

namespace Assets.Scripts.World
{
    public class WorldTerrain : MonoBehaviour
    {
        //Public variable for the size of the terrain, width and heigth
        public int RenderSize = 5;

        private static ChunkJobManager chunkJobs;

        public static List<Chunk> Chunks { get; set; }

        public static List<string> InProgressChunks { get; set; }

        //private string url = "https://minecrunchserver20190302073446.azurewebsites.net/api/chunk/world1";
        //private string url = "http://localhost:55163/api/chunk/world1";
        //private string url = "http://localhost:5000/api/chunk/world1";

        private string host = "https://minecrunchserver20190302073446.azurewebsites.net";

        //Generates the terrain
        private void Start()
        {

            if (!Directory.Exists(World.WorldSaveFolder + "/chunks"))
            {
                Directory.CreateDirectory(World.WorldSaveFolder + "/chunks");
                Directory.CreateDirectory(World.WorldSaveFolder + "/meshes");
            }

            Chunks = new List<Chunk>();
            InProgressChunks = new List<string>();
            chunkJobs = new ChunkJobManager();

            //GenerateChunksAroundPlayer();

            InvokeRepeating("ChunkMaintanence", 0, 0.5f);
            InvokeRepeating("GenerateChunksAroundPlayer", 0, 3);
            //while (!chunkJobs.ChunkGenerateTerrainTasks.Count.Equals(0)) { ChunkMaintanence(); }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 1000, 20), string.Format(
                "V:{0} C:{1}",
                chunkJobs.ChunkCalcVerticiesTasks.Count,
                chunkJobs.CompletedChunks.Count
                ));
        }

        private void FixedUpdate()
        {
            if (chunkJobs.CompletedChunks.Count > 0)
            {
                var chunk = chunkJobs.CompletedChunks.First();
                if (!DrawChunk(chunk))
                {
                    chunkJobs.CompletedChunks.Remove(chunk);
                    return;
                }
                InProgressChunks.Remove($"chunk{chunk.x},{chunk.y}");
                chunkJobs.CompletedChunks.Remove(chunk);
                Chunks.Add(chunk);
            }
        }

        private void ChunkMaintanence()
        {
            var player = World.Players.First();
            var playerChunkLoc = Coordinates.ChunkPlayerIsIn(player.transform.position);
            chunkJobs.ChunkDownloads.Sort((chunk1, chunk2) =>
            {
                return Math.Abs(playerChunkLoc.x - chunk1.cx) +
                Math.Abs(playerChunkLoc.y - chunk1.cy) <
                Math.Abs(playerChunkLoc.x - chunk2.cx) +
                Math.Abs(playerChunkLoc.y - chunk2.cy) ? -1 : 1;
            });
            
            chunkJobs.Update();
        }

        private void OnApplicationQuit()
        {
            chunkJobs.StopAllJobs();
        }

        public void GenerateChunksAroundPlayer()
        {
            int UpdateDistance = RenderSize;
            foreach (Player.WorldPlayer player in World.Players)
            {
                var playerChunkLoc = Coordinates.ChunkPlayerIsIn(player.transform.position);
                List<Vector2Int> neededChunks = new List<Vector2Int>();

                for (var a = 0; a <= UpdateDistance; a++)
                {
                    for (var b = 0; b <= UpdateDistance; b++)
                    {
                        neededChunks.Add(playerChunkLoc + (Vector2Int.up * a) + (Vector2Int.right * b));
                        neededChunks.Add(playerChunkLoc + (Vector2Int.up * a) + (Vector2Int.left * b));
                        neededChunks.Add(playerChunkLoc + (Vector2Int.down * a) + (Vector2Int.right * b));
                        neededChunks.Add(playerChunkLoc + (Vector2Int.down * a) + (Vector2Int.left * b));
                    }
                }

                //List<Chunk> toRemove = Chunks.Where(c => c.sections[0].blocks[0,0,0] is null).ToList();
                List<Chunk> toRemove = Chunks.Where(c => !neededChunks.Any(v2 => v2.x == c.x && v2.y == c.y)).ToList();

                toRemove.ForEach(c => {
                    Destroy(GameObject.Find(c.name));
                    Chunks.Remove(c);
                    });

                //Chunks.RemoveAll(c => c.Verticies.Count == 0);

                foreach (var chunkCoord in neededChunks)
                {
                    string chunkName = $"chunk{chunkCoord.x},{chunkCoord.y}";

                    if (ChunkExists(chunkName) || InProgressChunks.Contains(chunkName)) { continue; }

                    InProgressChunks.Add(chunkName);
                    chunkJobs.ChunkDownloads.Add(new ChunkDownloadTask(host, "world1", chunkCoord.x, chunkCoord.y));
                }
            }
        }

        /**
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
**/
/**
        private bool LoadChunk(Vector2 chunkCoord)
        {
            string chunkName = $"chunk{chunkCoord.x},{chunkCoord.y}";
            
            if (File.Exists(World.WorldSaveFolder + $"/meshes/{chunkName}-subchunk0.mesh"))
            {
                GameObject chunkGameObject = new GameObject(chunkName);

                for (var x=0; x<8; x++)
                {
                    string subchunkName = $"subchunk{x}";
                    string meshPath = World.WorldSaveFolder + $"/meshes/{chunkName}-{subchunkName}.mesh";
                    string matPath = World.WorldSaveFolder + $"/meshes/{chunkName}-{subchunkName}.mat";
                    string triPath = World.WorldSaveFolder + $"/meshes/{chunkName}-{subchunkName}.tri";

                    if (!File.Exists(meshPath)) { continue; }

                    var bf = new BinaryFormatter();
                    Material[] mats;
                    MeshRenderer meshRenderer;
                    GameObject subChunkGameObject = new GameObject(chunkName + "-" + subchunkName);
                    Dictionary<int, string> matDict;
                    Dictionary<int, List<int>> triDict;
                    using (FileStream materials = File.OpenRead(matPath))
                    {
                        matDict = bf.Deserialize(materials) as Dictionary<int, string>;
                        mats = new Material[matDict.Count];
                        foreach (int key in matDict.Keys)
                        {
                            mats[key] = Resources.Load<Material>(matDict[key]);
                        }
                    }
                    using (FileStream triangles = File.OpenRead(triPath))
                    {
                        triDict = bf.Deserialize(triangles) as Dictionary<int, List<int>>;
                    }

                    meshRenderer = subChunkGameObject.AddComponent<MeshRenderer>();
                    meshRenderer.materials = mats;

                    var meshBytes = File.ReadAllBytes(meshPath);
                    var subchunkMesh = MeshSerializer.ReadMesh(meshBytes);
                    subchunkMesh.triangles = new int[0];
                    subchunkMesh.subMeshCount = matDict.Count;
                    foreach (int key in triDict.Keys)
                    {
                        subchunkMesh.SetTriangles(triDict[key], key);
                    }

                    subchunkMesh.name = subchunkName + "mesh";
                    var meshFilter = subChunkGameObject.AddComponent<MeshFilter>();
                    meshFilter.mesh = subchunkMesh;
                    
                    subchunkMesh.RecalculateNormals();
                    subChunkGameObject.AddComponent<MeshCollider>();

                    subChunkGameObject.transform.SetParent(chunkGameObject.transform);
                    subChunkGameObject.transform.localPosition = new Vector3(0, 0, 0);
                    
                }
                chunkGameObject.transform.position = new Vector3(chunkCoord.x * 16, 0, chunkCoord.y * 16);

                return true;
            }


            string chunkFilePath = string.Format("{0}/chunks/{1},{2}.dat", World.WorldSaveFolder, chunkCoord.x, chunkCoord.y);
            if (!File.Exists(chunkFilePath))
            {
                return false;
            }
            //chunkJobs.AddChunkLoadJob(chunkCoord);
            return true;
        }
**/
        private bool DrawChunk(Chunk chunk)
        {
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
            return true;
            //Debug.Log($"Chunk time {chunk.processTimeMs / 1000.0f} seconds");
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
                Debug.LogError("Failed to set verticies!");
                return false;
            }


            foreach (int key in section.Mesh.Quads.Keys)
            {
                subchunkMesh.SetIndices(section.Mesh.Quads[key].ToArray(), MeshTopology.Quads, key);
            }
            

            subchunkMesh.SetUVs(0, section.Mesh.UVs);

            var meshFilter = subChunkGameObject.AddComponent<MeshFilter>();

            meshFilter.mesh = subchunkMesh;
            subchunkMesh.RecalculateNormals();
            subChunkGameObject.AddComponent<MeshCollider>();

            return true;
        }

        /**
        private Chunk GetChunk(Vector2 chunkLoc)
        {
            return Chunks.FirstOrDefault(x => x.ChunkPosition == chunkLoc);
        }
    **/
        private bool ChunkExists(string name)
        {
            return Chunks.Count(c => c.name == name) > 0;
            //return GameObject.Find(name) != null;
        }
    }
}
