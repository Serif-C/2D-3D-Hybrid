using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlaceSpawnPoints : MonoBehaviour
{
    /// <summary>
    /// Dont know yet if I want random spawn points or manual placement
    /// </summary>
    [MenuItem("Tools/Place Enemy Spawn Points")]
    public static void PlaceSpawnPointsInScene()
    {
        GameObject plane = GameObject.FindGameObjectWithTag("Ground");


    }
}
