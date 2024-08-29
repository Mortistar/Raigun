using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnData
{
    public float timeStamp;
    public float initTimeStamp;
    public GameObject enemy {get; private set;}
    public Vector3 spawnPos {get; private set;}

    public SpawnData(float TimeStamp, GameObject Enemy, Vector3 Lane)
    {
        initTimeStamp = TimeStamp;
        timeStamp = TimeStamp;
        enemy = Enemy;
        spawnPos = Lane;
    }
}
