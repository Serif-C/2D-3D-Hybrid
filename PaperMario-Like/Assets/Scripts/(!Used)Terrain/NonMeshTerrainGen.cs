using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMeshTerrainGen : MonoBehaviour
{
    [Header("Terrain Parameters")]
    [SerializeField] private Terrain terrain;
    [SerializeField] private float terrainWidth = 200f;
    [SerializeField] private float terrainLength = 200f;
    [SerializeField] private int terrainResolution = 102;
    [SerializeField] private float terrainHeightMultiplier = 30f;
    [SerializeField] private float terrainDetailMultiplier = 10f;

    [Header("Perlin Noise Parameters")]
    [SerializeField] private float noiseScale = 0.1f;
    [SerializeField] private int octaves = 4;
    [SerializeField] private float persistence = 0.5f;
    [SerializeField] private int lacunarity = 2;
    


    private void Start()
    {
        GenerateTerrain(terrain);
    }
    private void GenerateTerrain(Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;

        terrainData.heightmapResolution = terrainResolution;
        terrainData.size = new Vector3(terrainWidth, terrainHeightMultiplier, terrainLength);

        float[,] heights = GeneratePerlinNoise(terrainResolution, noiseScale, octaves, persistence, lacunarity);

        terrainData.SetHeights(0, 0, heights);
    }

    private float[,] GeneratePerlinNoise(int resolution, float scale, int octaves, float persistence, float lacunarity)
    {
        float[,] heights = new float[resolution, resolution];

        for(int x = 0; x < resolution; x++)
        {
            for(int y = 0; y < resolution; y++)
            {
                float xCoord = (float) x / resolution * scale;
                float yCoord = (float) y / resolution * scale;

                float perlinValue = Mathf.PerlinNoise(xCoord, yCoord);
                heights[x, y] = perlinValue;
            }
        }

        return heights;
    }
}
