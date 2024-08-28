using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Levels
    {
        Intro = 0,
        Menu = 1,
        Game = 2,
        Highscore = 3,
        Ending = 4
    }
    public static GameManager Instance;

    public GameData data {get; private set;}

    public bool isWin {get; private set;}
    public int tempScore {get; private set;}

    private string path;

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
        tempScore = 0;
        isWin = false;
        DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath + "/game.data";
        LoadData();
    }
    private void LoadData()
    {
        if (File.Exists(path))
        {
            //Load from path
            string fileContents = File.ReadAllText(path);
            data = JsonUtility.FromJson<GameData>(fileContents);
        }else
        {
            //Create default to path
            List<Score> defaultScores = new()
            {
                new Score("HAL", 20010),
                new Score("MORT", 210000),
                new Score("DEC", 20490),
                new Score("JAM", 17010),
                new Score("L3T", 103037),
                new Score("ART", 42420),
                new Score("DOC", 88000),
                new Score("VEG", 9000)
            };
            defaultScores = defaultScores.OrderByDescending(x => x.GetScore()).ToList();
            data = new GameData(defaultScores);
            SaveData();
        }
    }
    private void SaveData()
    {
        //Save to path
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonString);
    }
    public void SetTempScore(int Score, bool IsWin)
    {
        tempScore = Score;
        isWin = IsWin;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene((int)Levels.Intro);
        }
    }
}
