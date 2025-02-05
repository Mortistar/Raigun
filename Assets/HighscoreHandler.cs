using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreHandler : MonoBehaviour
{
    [SerializeField] private bool enabledOnStart = false;
    [SerializeField] private Image highscoreTitle;
    [SerializeField] private Sprite sprDNQ;
    [SerializeField] private Sprite sprInit;

    [SerializeField] private TextMeshProUGUI highscoreText;

    [SerializeField] private UIContinue continuePanel;
    [SerializeField] private UIInitials initial;

    private CanvasGroup group;

    private bool canInitial = false;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }
    void Start()
    {
        if (enabledOnStart)
        {
            Enable();
        }
    }
    public void Enable()
    {
        group.alpha = 1f;
        if (enabledOnStart)
        {
            UpdateText();
        }
        OpenOptions();
    }   
    private void OpenOptions()
    {
        List<Score> tempData = GameManager.Instance.data.GetData();
        canInitial = GameManager.Instance.tempScore > tempData[tempData.Count-1].GetScore();
        if (canInitial)
        {
            highscoreTitle.sprite = sprInit;
            initial.Enable();
        }else
        {
            highscoreTitle.sprite = sprDNQ;
            continuePanel.Enable();
        }
    }
    private void UpdateText()
    {
        List<Score> scores = GameManager.Instance.data.GetData();
        //Clear text
        highscoreText.text = "";
        
        string queuedText = "";

        //Reorder scores with current player score
        bool currentAdded = false;
        int currentScore = GameManager.Instance.tempScore;

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
            {
                if (maxIndex >= 7)
                {
                    queuedText +=  "<color=#21b136>" + "DNQ " + UIScore.ScoreToDisplay(currentScore) + "\n";
                    queuedText += "-YOU" + "</color>" + "\n";
                }else
                {
                    queuedText +=  "<color=#21b136>" + scoreIndex + "] " + UIScore.ScoreToDisplay(currentScore) + "\n";
                    queuedText += "-YOU" + "</color>" + "\n";
                }
            }
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
}
