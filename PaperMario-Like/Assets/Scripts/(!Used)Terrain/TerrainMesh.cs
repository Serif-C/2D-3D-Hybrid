using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    [SerializeField] private int xSize = 20;
    [SerializeField] private int zSize = 20;

    [SerializeField] private float xMagnitude = .2f;
    [SerializeField] private float yMagnitude = 2f;
    [SerializeField] private float zMagnitude = .2f;

    [SerializeField] private Gradient gradient;

    private float minHeight;
    private float maxHeight;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
    }

    private void Update()
    {
        CreateQuads();
        UpdateMesh();
    }

    private void CreateQuads()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * xMagnitude, z * zMagnitude) * yMagnitude;

                if (y > maxHeight)
                    maxHeight = y;
                if (y < minHeight)
                    minHeight = y;

                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        int cvp = 0; // cvp == current vertex position
        int tri = 0;
        triangles = new int[xSize * zSize * 6];

        for(int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[tri + 0] = cvp + 0;
                triangles[tri + 1] = cvp + xSize + 1;
                triangles[tri + 2] = cvp + 1;
                triangles[tri + 3] = cvp + xSize + 1;
                triangles[tri + 4] = cvp + xSize + 2;
                triangles[tri + 5] = cvp + 1;

                cvp++;
                tri += 6;
            }
            // necessary so that no triangle connection is made from previous vertex to the next vertex
            cvp++;
        }

        colors = new Color[vertices.Length];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }
}
