using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class UIContinue : MonoBehaviour
{
    [SerializeField] private Image retryText;
    [SerializeField] private Image quitText;

    [SerializeField] private Sprite retrySelected;
    [SerializeField] private Sprite retryDefault;

    [SerializeField] private Sprite quitSelected;
    [SerializeField] private Sprite quitDefault;

    private CanvasGroup group;
    private EventReference submitRef;

    private bool isEnabled = false;
    private bool isRetry = false;
    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }
    void Start()
    {
        submitRef = RuntimeManager.PathToEventReference("event:/SFX/PICKUP/sfx_PU_life");
    }
    public void Enable()
    {
        group.alpha = 1;
        isEnabled = true;
        isRetry = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                isRetry = !isRetry;
                UpdateOptions();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                RuntimeManager.PlayOneShot(submitRef);
                if (isRetry)
                {
                    SceneManager.LoadScene((int)GameManager.Levels.Game);
                }else
                {
                    SceneManager.LoadScene((int)GameManager.Levels.Intro);
                }
            }
        }
    }
    void UpdateOptions()
    {
        if (isRetry)
        {
            retryText.sprite = retrySelected;
            quitText.sprite = quitDefault;
        }else
        {
            retryText.sprite = retryDefault;
            quitText.sprite = quitSelected;
        }
    }
}
