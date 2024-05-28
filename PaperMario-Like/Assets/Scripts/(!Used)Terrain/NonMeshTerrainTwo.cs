using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMeshTerrainTwo : MonoBehaviour
{
    [SerializeField] private Terrain terrain;

    [SerializeField] private int width;   // x-axis
    [SerializeField] private int height;  // y-axis
    [SerializeField] private int length;  // z-axis

    [SerializeField] private float scale;
    [SerializeField] private int xSeed;
    [SerializeField] private int zSeed;
    
    [SerializeField] private Gradient gradient;

    private Color[] colors;
    private float minHeight;
    private float maxHeight;

    private void Update()
    {
        GenerateTerrain(terrain);
    }

    private void GenerateTerrain(Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, height, length);
        terrainData.SetHeights(0, 0, GenerateHeights());
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, length];

        for (int x = 0, i = 0; x < width; x++)
        {
            for(int y = 0; y < length; y++)
            {
                float xCoord = (float) x / width * scale + xSeed;
                float yCoord = (float) y / length * scale + zSeed;
                float noise = Mathf.PerlinNoise(xCoord, yCoord);

                if (noise > maxHeight)
                    maxHeight = noise;
                if (noise < minHeight)
                    minHeight = noise;

                heights[x, y] = noise;
                i++;
            }
        }

        return heights;
    }
}
