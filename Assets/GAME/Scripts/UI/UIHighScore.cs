using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIHighScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreText;
    private List<Score> scores;
    void Start()
    {
        scores = GameManager.Instance.data.GetData();
        StageHandler.Instance._UpdateScore += UpdateHighscores;
        UpdateHighscores();
    }

    public void UpdateHighscores()
    {
        //Clear text
        highscoreText.text = "";
        
        string queuedText = "";

        //Reorder scores with current player score
        bool currentAdded = false;
        int currentScore = StageHandler.Instance.score;

        //Only add as many scores as can fit
        int maxIndex = scores.Count < 7 ? scores.Count : 7;

        int scoreIndex = 1;
        //Write scores
        for (int i = 0; i < maxIndex; i++)
        {
            if (!currentAdded)
            {
                if (currentScore > scores[i].GetScore())
                {
                    queuedText += "<color=#21b136>" + scoreIndex + "] " + UIScore.ScoreToDisplay(currentScore) + "\n";
                    queuedText += "-YOU" + "</color>" + "\n";
                    currentAdded = true;
                    scoreIndex++;
                }
            }
            queuedText += scoreIndex + "] " + (scores[i].GetName() == "MORT" ? "<color=#a41759>-":"")+ UIScore.ScoreToDisplay(scores[i].GetScore()) + "\n";
            queuedText += "-" + scores[i].GetName() + (scores[i].GetName() == "MORT" ? "</color>":"") + "\n";
            scoreIndex++;
        }
        if (!currentAdded)
        {
            queuedText +=  "<color=#21b136>" + scoreIndex + "] " + UIScore.ScoreToDisplay(currentScore) + "\n";
            queuedText += "-YOU" + "</color>" + "\n";
            scoreIndex++;
        }
        //If space for scores
        if (maxIndex < 7)
        {
            for (int i = maxIndex; i < 7; i++)
            {
                queuedText += "<color=#251534>" + scoreIndex + "] " +  UIScore.ScoreToDisplay(0) + "\n";
                queuedText += "-/// </color>" + "\n";
                scoreIndex++;
            }
        }

        //Write to screen
        highscoreText.text = queuedText;
    }
    void OnDisable()
    {
        StageHandler.Instance._UpdateScore -= UpdateHighscores;
    }
}
