using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine.Rendering;

public class CombatHandler : MonoBehaviour
{
    public static CombatHandler Instance;

    public enum ExplosionType
    {
        normal,
        big
    }

    [SerializeField] private bool debugDisable;
    [SerializeField] private bool debugLevel;

    [Header("Data")]
    [SerializeField] private TextAsset groundList;
    [SerializeField] private TextAsset skyList;
    [SerializeField] private TextAsset spaceList;

    [Header("Ground Ships")]
    [SerializeField] private GameObject G_HOMING;
    [SerializeField] private GameObject G_STATIC;
    [SerializeField] private GameObject G_AIMED;
    [SerializeField] private GameObject G_FRIGATE;
    [SerializeField] private GameObject G_BOSS;

    [Header("Sky Ships")]
    [SerializeField] private GameObject SK_HOMING;
    [SerializeField] private GameObject SK_STATIC;
    [SerializeField] private GameObject SK_AIMED;
    [SerializeField] private GameObject SK_FRIGATE;
    [SerializeField] private GameObject SK_BOSS;

    [Header("Space Ships")]
    [SerializeField] private GameObject SP_HOMING;
    [SerializeField] private GameObject SP_STATIC;
    [SerializeField] private GameObject SP_AIMED;
    [SerializeField] private GameObject SP_FRIGATE;
    [SerializeField] private GameObject SP_BOSS;

    [Header("External")]
    [SerializeField] private GameObject explosionRef;
    [SerializeField] private GameObject explosionBigRef;

    public bool isPaused {get; private set;}
    public bool skyPaused {get; private set;}
    public bool spacePaused {get; private set;}

    private List<SpawnData> spawnLogGround;
    private List<SpawnData> spawnLogSky;
    private List<SpawnData> spawnLogSpace;

    private int groundIndex;
    private int skyIndex;
    private int spaceIndex;

    private float timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            INIT();
        }else
        {
            Destroy(gameObject);
        }
    }
    private void INIT()
    {
        if (debugDisable)
        {
            isPaused = true;
            return;
        }
        spawnLogGround = new List<SpawnData>();
        spawnLogSky = new List<SpawnData>();
        spawnLogSpace = new List<SpawnData>();

        timer = 0f;

        isPaused = true;
        skyPaused = true;
        spacePaused = true;
        if (debugLevel)
        {
            skyPaused = false;
            spacePaused = false;
        }
        

        ParseFile(groundList, spawnLogGround);
        ParseFile(skyList, spawnLogSky);
        ParseFile(spaceList, spawnLogSpace);

        groundIndex = 0;
        skyIndex = 0;
        spaceIndex = 0;

        StartCoroutine(IStartDelay());
    }
    private System.Collections.IEnumerator IStartDelay()
    {
        yield return new WaitForSeconds(2f);
        isPaused = false;
    }
    void Update()
    {
        if (!isPaused && !debugDisable)
        {
            if (groundIndex < spawnLogGround.Count) //If can spawn objects
            {
                if (spawnLogGround[groundIndex].timeStamp <= timer) //If passed spawn timer
                {
                    GameObject newShip = Instantiate(spawnLogGround[groundIndex].enemy);
                    newShip.transform.position = spawnLogGround[groundIndex].spawnPos + (Vector3.up * 3);
                    newShip.GetComponent<Enemy>().INIT(spawnLogGround[groundIndex].spawnPos);
                    groundIndex++;
                }
            }
            if (!skyPaused && skyIndex < spawnLogSky.Count) //If can spawn objects
            {
                if (spawnLogSky[skyIndex].timeStamp <= timer) //If passed spawn timer
                {
                    GameObject newShip = Instantiate(spawnLogSky[skyIndex].enemy);
                    newShip.transform.position = spawnLogSky[skyIndex].spawnPos + (Vector3.up * 3);
                    newShip.GetComponent<Enemy>().INIT(spawnLogSky[skyIndex].spawnPos);
                    skyIndex++;
                }
            }
            if ( !spacePaused && spaceIndex < spawnLogSpace.Count) //If can spawn objects
            {
                if (spawnLogSpace[spaceIndex].timeStamp <= timer) //If passed spawn timer
                {
                    GameObject newShip = Instantiate(spawnLogSpace[spaceIndex].enemy);
                    newShip.transform.position = spawnLogSpace[spaceIndex].spawnPos + (Vector3.up * 3);
                    newShip.GetComponent<Enemy>().INIT(spawnLogSpace[spaceIndex].spawnPos);
                    spaceIndex++;
                }
            }
            timer += Time.deltaTime;
        }
    }
    private void ParseFile (TextAsset file, List<SpawnData> layer) 
    {   
        //Split into lines
        string line = file.text;
        string[] lines = Regex.Split (line, "\\r?\\n");

        //For each line in file
        for ( int i=0; i < lines.Length; i++)
        {
            //Split into values
            string valueLine = lines[i];

            //skip comments
            if (valueLine[0] == '#')
            {
                continue;
            }
            string[] values = Regex.Split ( valueLine, "," ); // your splitter here

            int intLayer = ListToLayer(layer);
            float timeStamp =  float.Parse(values[0]);
            GameObject enemy = StringToGameobject(values[1], intLayer);
            Vector3 targetPos = LaneToPosition(values[2], intLayer);
            SpawnData data = new SpawnData(timeStamp, enemy, targetPos);
            layer.Add(data);
        }
    }
    private int ListToLayer(List<SpawnData> list)
    {
        if (list == spawnLogGround)
        {
            return 0;
        }
        if (list == spawnLogSky)
        {
            return 1;
        }
        if (list == spawnLogSpace)
        {
            return 2;
        }
        throw new Exception("Couldn't convert list to int: " + list);
    }
    private Vector3 LaneToPosition(string lane, int layer)
    {
        int intLane = int.Parse(lane);
        float zOffset = 0;
        switch (layer)
        {
            case 0:
                zOffset = 0;
                break;
            case 1:
                zOffset = -20;
                break;
            case 2:
                zOffset = -200;
                break;
        }
        switch (intLane)
        {
            case 11:
                return new Vector3(-1.8f, 2, zOffset);
            case 12: 
                return new Vector3(-1.8f, 0, zOffset);
            case 21:
                return new Vector3(-0.9f, 2, zOffset);
            case 22:
                return new Vector3(-0.9f, 0, zOffset);
            case 31:
                return new Vector3(0, 2, zOffset);
            case 32:
                return new Vector3(0, 0, zOffset);
            case 41:
                return new Vector3(0.9f, 2, zOffset);
            case 42:
                return new Vector3(0.9f, 0, zOffset);
            case 51:
                return new Vector3(1.8f, 2, zOffset);
            case 52:
                return new Vector3(1.8f, 0, zOffset);
        }
        throw new Exception("Couldn't convert lane to position: " + lane);
    }
    private GameObject StringToGameobject(string value, int layer)
    {
        switch (value)
        {
            case "HOMING":
                switch (layer)
                {
                    case 0:
                        return G_HOMING;    
                    case 1:
                        return SK_HOMING;
                    case 2:
                        return SP_HOMING;
                }
                break;
            case "STATIC":
                switch (layer)
                {
                    case 0:
                        return G_STATIC;
                    case 1:
                        return SK_STATIC;
                    case 2:
                        return SP_STATIC;
                }
                break;
            case "AIMED":
                switch (layer)
                {
                    case 0:
                        return G_AIMED;
                    case 1:
                        return SK_AIMED;
                    case 2:
                        return SP_AIMED;
                }
                break;
            case "FRIGATE":
                switch (layer)
                {
                    case 0:
                        return G_FRIGATE;
                    case 1:
                        return SK_FRIGATE;
                    case 2:
                        return SP_FRIGATE;
                }
                break;
            case "BOSS":
                switch (layer)
                {
                    case 0:
                        return G_BOSS;
                    case 1:
                        return SK_BOSS;
                    case 2:
                        return SP_BOSS;
                }
                break;
        }
        throw new Exception("Couldn't convert string to gameobject:" + value);
    }
    public void AdvanceTimestamp(float zOffset)
    {
        switch (zOffset)
        {
            case 0f:
                foreach(SpawnData data in spawnLogGround)
                {
                    data.timeStamp += timer + 5;
                }
                groundIndex = 0;
                break;
            case -20f:
                foreach(SpawnData data in spawnLogSky)
                {
                    data.timeStamp += timer;
                }
                skyIndex = 0;
                break;
            case -200f:
                foreach(SpawnData data in spawnLogSpace)
                {
                    data.timeStamp += timer;
                }
                spaceIndex = 0;
                break;
        }
    }
    public void SpawnExplosion(ExplosionType type, Vector3 position)
    {
        GameObject explosion = null;
        switch (type)
        {
            case ExplosionType.normal:
                explosion = explosionRef;
                break;
            case ExplosionType.big:
                explosion = explosionBigRef;
                break;
        }
        Instantiate(explosion, position, quaternion.identity);
    }
    public void SpawnExplosionWithDelay(ExplosionType type, Vector3 position, float delay)
    {
        StartCoroutine(ISpawnExplosion(type, position, delay));
    }
    private System.Collections.IEnumerator ISpawnExplosion(ExplosionType type, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnExplosion(type, position);
    }
    public void Pause()
    {
        isPaused = true;
    }
    public void UnPause()
    {
        isPaused = false;
    }
    public void UnpauseSky()
    {
        skyPaused = false;
    }
    public void UnpauseSpace()
    {
        spacePaused = false;
    }
}
