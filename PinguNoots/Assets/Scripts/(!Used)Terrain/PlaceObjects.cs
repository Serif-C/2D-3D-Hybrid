using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlaceObjects : MonoBehaviour
{
    public GameObject[] objects;
    public int numOfObjects;
    public float spawnRadius;

    private void Start()
    {
        GenerateObjects();
    }

    private void GenerateObjects()
    {
        for(int i = 0; i < numOfObjects; i++)
        {
            Vector3 randomPosition = GetRandomNavMeshPosition(spawnRadius);
            int randomIndex = Random.Range(0, objects.Length);
            GameObject newGameObject = Instantiate(objects[randomIndex], randomPosition, Quaternion.identity);
            newGameObject.transform.SetParent(transform);
        }
    }

    private Vector3 GetRandomNavMeshPosition(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit navHit;

        if(NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas))
        {
            return navHit.position;
        }

        return transform.position;
    }
}
