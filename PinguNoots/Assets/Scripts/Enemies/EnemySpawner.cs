using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawns;
    [SerializeField] private GameObject enemy;
    //[SerializeField] private PlayerManager player;

    private bool hasJustLoaded = true;
    private bool hasJustRested;
    private bool hasJustRespawned;

    private bool spawned;

    private void Update()
    {
        /*
         * - If player have just loaded a level call SpawnEnemiesAtLocation
         * - If player have just rested at a camp call SpawnEnemiesAtLocation
         * - If player have just revived call SpawnEnemiesAtLocation
         */
        if(!spawned)
        {
            if (hasJustLoaded || hasJustRested || hasJustRespawned)
            {
                SpawnEnemiesAtLocation();
            }
        }
    }

    private void SpawnEnemiesAtLocation()
    {
        for (int i = 0; i < spawns.Length; i++)
        {
            Instantiate(enemy, spawns[i].transform.position, Quaternion.identity, spawns[i].transform);
        }
        spawned = true;
    }
}
