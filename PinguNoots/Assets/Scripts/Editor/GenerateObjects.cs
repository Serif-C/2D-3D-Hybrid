using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateObjects : MonoBehaviour
{

    [MenuItem("Tools/Generate Objects in Scene")]
    public static void GenerateObjectsRandomly()
    {
        int numOfObjects = 1500;
        float spawnRadius = 10f;
        GameObject plane = GameObject.FindGameObjectWithTag("Ground");

        // Min and Max X-Coordinates of the plane's mesh
        float minX = plane.GetComponent<MeshFilter>().sharedMesh.bounds.min.x;
        float maxX = plane.GetComponent<MeshFilter>().sharedMesh.bounds.max.x;

        // Min and Max Z-Coordinates of the plane's mesh
        float minZ = plane.GetComponent<MeshFilter>().sharedMesh.bounds.min.z;
        float maxZ = plane.GetComponent<MeshFilter>().sharedMesh.bounds.max.z;

        GameObject objectsParent = plane.transform.Find("Objects")?.gameObject;
        if (objectsParent == null)
        {
            objectsParent = new GameObject("Objects");
            objectsParent.transform.parent = plane.transform;
        }

        string[] prefabPaths = new string[] 
        {
            "Assets/Prefabs/GrassBillboard_0.prefab", 
            "Assets/Prefabs/GrassBillboard_1.prefab", 
            "Assets/Prefabs/RockBillboard_0.prefab",
            "Assets/Prefabs/RockBillboard_1.prefab",
            "Assets/Prefabs/TreeBillboard_0.prefab" 
        };

        for (int i = 0; i < numOfObjects; i++)
        {
            string prefabPath = prefabPaths[Random.Range(0, prefabPaths.Length)];
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            float positionX = Random.Range(minX, maxX) + plane.transform.position.x;
            float positionZ = Random.Range(minZ, maxZ) + plane.transform.position.z;
            Vector3 position = new Vector3(positionX, 0.055f, positionZ) * spawnRadius;

            if (prefab != null)
            {
                Instantiate(prefab, position, Quaternion.identity, objectsParent.transform);
            }
            else
            {
                Debug.LogError("Prefab at path " + prefabPath + " not found!");
            }
        } 
    }
}
