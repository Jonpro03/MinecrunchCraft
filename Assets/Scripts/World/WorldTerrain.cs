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
                Directory.CreateDirectory(World.WorldSaveFolder + "/meshes");
            }

            Chunks = new List<Chunk>();
            chunkJobs = new ChunkJobManager();

            Vector2 playerChunkLoc = Coordinates.ChunkPlayerIsIn(transform.position);

            // Create the chunks

            for (int cx = (-1 * RenderSize) + (int)playerChunkLoc.x; cx < RenderSize; cx++)
            {
                for (int cz = (-1 * RenderSize) + (int)playerChunkLoc.y; cz < RenderSize; cz++)
                {
                    Vector2 chunkCoord = new Vector2(cx, cz);
                    bool chunkOnDisk = LoadChunk(new Vector2(cx, cz));
                    if (!chunkOnDisk)
                    {
                        //GenerateChunk(chunkCoord);
                    }
                }
            }


            GenerateNewChunksAroundPlayers();

            chunkJobs.ChunkGenerateTerrainTasks.Sort((chunk1, chunk2) =>
            {
                return Math.Abs(playerChunkLoc.x - chunk1.chunk.x) + 
                Math.Abs(playerChunkLoc.y - chunk1.chunk.y) <
                Math.Abs(playerChunkLoc.x - chunk2.chunk.x) + 
                Math.Abs(playerChunkLoc.y - chunk2.chunk.y) ? -1 : 1;
            });

            
            InvokeRepeating("ChunkMaintanence", 0, 0.33f);
            InvokeRepeating("GenerateNewChunksAroundPlayers", 0, 30);
            //while (!chunkJobs.ChunkGenerateTerrainTasks.Count.Equals(0)) { ChunkMaintanence(); }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 1000, 20), string.Format(
                "T:{0} C:{1} F:{2} V:{3} D:{4} dups:{5}",
                chunkJobs.ChunkGenerateTerrainTasks.Count,
                chunkJobs.ChunkGenerateCavesTasks.Count,
                chunkJobs.ChunkCalculateFacesTasks.Count,
                chunkJobs.ChunkCalcVerticiesTasks.Count,
                chunkJobs.CompletedChunks.Count,
                chunkJobs.ChunkGenerateTerrainTasks.GroupBy(c => c.chunk.name).Where(c => c.Count() > 1).Count()
                ));
        }

        private void FixedUpdate()
        {
            if (chunkJobs.CompletedChunks.Count > 0)
            {
                var chunk = chunkJobs.CompletedChunks.First();
                if (!DrawChunk(chunk))
                {
                    Chunks.Remove(chunk);
                }
                chunkJobs.CompletedChunks.Remove(chunk);
            }
        }

        private void ChunkMaintanence()
        {
            chunkJobs.Update();
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
                var playerChunkLoc = Coordinates.ChunkPlayerIsIn(player.transform.position);
                List<Vector2> neededChunks = new List<Vector2>();

                for (var a = 0; a <= UpdateDistance; a++)
                {
                    for (var b = 0; b <= UpdateDistance; b++)
                    {
                        neededChunks.Add(playerChunkLoc + (Vector2.up * a) + (Vector2.right * b));
                        neededChunks.Add(playerChunkLoc + (Vector2.up * a) + (Vector2.left * b));
                        neededChunks.Add(playerChunkLoc + (Vector2.down * a) + (Vector2.right * b));
                        neededChunks.Add(playerChunkLoc + (Vector2.down * a) + (Vector2.left * b));
                    }
                }

                //List<Chunk> toRemove = Chunks.Where(c => c.sections[0].blocks[0,0,0] is null).ToList();
                List<Chunk> toRemove = Chunks.Where(c => !neededChunks.Any(v2 => v2.x == c.x && v2.y == c.y)).ToList();

                //toRemove.ForEach(c => {
                //    Destroy(GameObject.Find(c.name));
                //    Chunks.Remove(c);
                //    });

                //Chunks.RemoveAll(c => c.Verticies.Count == 0);

                foreach (var chunkCoord in neededChunks)
                {
                    if (!ChunkExists($"chunk{chunkCoord.ToString()}"))
                    {
                        bool chunkOnDisk = false;// LoadChunk(chunkCoord);
                        if (!chunkOnDisk)
                        {
                            GenerateChunk(chunkCoord);
                        }
                    }
                }

                chunkJobs.ChunkGenerateTerrainTasks.Sort((chunk1, chunk2) =>
                {
                    return Math.Abs(playerChunkLoc.x - chunk1.chunk.x) +
                    Math.Abs(playerChunkLoc.y - chunk1.chunk.y) <
                    Math.Abs(playerChunkLoc.x - chunk2.chunk.x) +
                    Math.Abs(playerChunkLoc.y - chunk2.chunk.y) ? -1 : 1;
                });

                chunkJobs.ChunkCalculateFacesTasks.Sort((chunk1, chunk2) =>
                {
                    return Math.Abs(playerChunkLoc.x - chunk1.chunk.x) +
                    Math.Abs(playerChunkLoc.y - chunk1.chunk.y) <
                    Math.Abs(playerChunkLoc.x - chunk2.chunk.x) +
                    Math.Abs(playerChunkLoc.y - chunk2.chunk.y) ? -1 : 1;
                }); 
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

        private bool LoadChunk(Vector2 chunkCoord)
        {
            string chunkName = $"chunk{chunkCoord.ToString()}";
            
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

        private void GenerateChunk(Vector2 chunkPos)
        {
            string chunkName = string.Format("chunk{0}", chunkPos.ToString());
            if (ChunkExists(chunkName))
            {
                return;
            }

            GameObject chunkGameObject = new GameObject(chunkName);
            Chunk chunk = new Chunk()
            {
                name = chunkName,
                x = (int) chunkPos.x,
                y = (int) chunkPos.y,
                biome = (Biome) minecrunch.utilities.PerlinNoise.Instance.Biome((int)chunkPos.x, 0, (int)chunkPos.y)
            };

            Chunks.Add(chunk);
            chunkJobs.AddGenerateJob(chunk);
        }

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

            //Save to disk
            /**
            string meshPath = World.WorldSaveFolder + $"/meshes/{subChunkGameObject.name}.mesh";
            var serializedMesh = MeshSerializer.WriteMesh(subchunkMesh, false);
            File.WriteAllBytes(meshPath, serializedMesh);

            string matPath = World.WorldSaveFolder + $"/meshes/{subChunkGameObject.name}.mat";
            using (FileStream materials = File.Create(matPath))
            {
                new BinaryFormatter().Serialize(materials, section.Mesh.Materials);
            }
            string triPath = World.WorldSaveFolder + $"/meshes/{subChunkGameObject.name}.tri";
            using (FileStream triangles = File.Create(triPath))
            {
                new BinaryFormatter().Serialize(triangles, section.Mesh.Triangles);
            }
            **/
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
            return GameObject.Find(name) != null;
        }
    }
}
