using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField] private List<Score> highScores;
    public GameData(List<Score> Highscores)
    {
        highScores = new List<Score>();
        highScores = Highscores;
    }
    public void AddScore(string name, int score)
    {
        //Add new score
        Score newScore = new Score(name, score);
        highScores.Add(newScore);
        SortScores();

        //Remove any scores above 10
        if (highScores.Count > 10)
        {
            for (int i = highScores.Count - 1; i > 10; i--)
            {
                highScores.RemoveAt(i);
            }
        }
    }
    public void SortScores()
    {
        if (highScores.Count > 1)
        {
            highScores = highScores.OrderByDescending(x => x.GetScore()).ToList();
        }
    }
    public List<Score> GetData()
    {
        return highScores;
    }
}
[Serializable]
public class Score
{
    [SerializeField] private string name;
    [SerializeField] private int score;

    public Score(string Name, int Score)
    {
        name = Name;
        score = Score;
    }
    public string GetName()
    {
        return name;
    }
    public int GetScore()
    {
        return score;
    }
}
