using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private List<Sprite> multSprites;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image scoreMult;
    // Start is called before the first frame update
    void Start()
    {
        StageHandler.Instance._UpdateScore += UpdateScore;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = ScoreToDisplay(StageHandler.Instance.score);
        scoreMult.sprite = multSprites[StageHandler.Instance.scoreMult - 1];
    }
    public static string ScoreToDisplay(int score)
    {
        string tempScore = score.ToString();
        if (tempScore.Length < 9)
        {
            for (int i = tempScore.Length; i < 9; i++)
            {
                tempScore = "0" + tempScore;
            }
        }
        return tempScore;
    }
    void OnDisable()
    {
        StageHandler.Instance._UpdateScore -= UpdateScore;
    }
}
