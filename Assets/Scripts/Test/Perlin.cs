using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Terrain;
using UnityEngine;

public class Perlin : MonoBehaviour
{

    public float[,] map;
    public int width = 32;
    public int height = 32;

    // Use this for initialization
    void Start()
    {
        map = new float[32, 32];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x,y] = PerlinNoise.Biome(new Vector2(x, y), "jon".GetHashCode()) / 10;
                Debug.Log(map[x, y].ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float perlin = map[x, y];
                Gizmos.color = new Color(perlin, perlin, perlin);
                var pos = new Vector3(-width / 2 + x + 0.5f, 0, -height / 2 + y + 0.5f);
                
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }
    }
}
