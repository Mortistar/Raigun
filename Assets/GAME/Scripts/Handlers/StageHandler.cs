using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using FMODUnity;

public class StageHandler : MonoBehaviour
{
    [SerializeField] private bool debugControls = false;
    [SerializeField] private HighscoreHandler scoreHandler;
    [SerializeField] private CanvasGroup winFade;
    public delegate void gameEvent();

    public gameEvent _UpdateLives;
    public gameEvent _UpdateScore;
    public gameEvent _UpdateStage;

    public static StageHandler Instance;
    private Camera cam;

    private Tween camTween;

    public GameLayer currentLayer {get; private set;}
    public bool canSwitchLayer {get; private set;}

    private const int _GROUND = 0;
    private const int _SKY = -20;
    private const int _SPACE = -200;

    public int lives {get; private set;}

    public int score {get; private set;}
    public int scoreMult {get; private set;}
    public int multProgress {get; private set;}

    private EventReference multiplierRef;


    public enum GameLayer
    {
        Ground = 0,
        Sky = 1,
        Space = 2 
    }
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
        canSwitchLayer = true;
        currentLayer = GameLayer.Ground;

        //Init stats;
        ResetScore();
        ResetLives();
    }
    void Start()
    {
        multiplierRef = RuntimeManager.PathToEventReference("event:/SFX/UI/sfx_multiplier");
        cam = Camera.main;
        AudioHandler.Instance.PlaySong(AudioHandler.Song.StageOne, FMOD.Studio.STOP_MODE.IMMEDIATE);
        GameManager.Instance.SetTempScore(0, false);
    }
    public void Ascend()
    {
        if (!canSwitchLayer || (int)currentLayer + 1 > 2)
        {
            return;
        }
        currentLayer++;
        ChangeLayer(currentLayer);
    }
    public void Descend()
    {
        if (!canSwitchLayer || (int)currentLayer - 1 < 0)
        {
            return;
        }
        currentLayer--;
        ChangeLayer(currentLayer);
    }
    public void ChangeLayer(GameLayer layer)
    {
        if (!canSwitchLayer)
        {
            return;
        }
        switch (layer)
        {
            case GameLayer.Ground:
                camTween = cam.transform.DOMove(new Vector3(0, 0, _GROUND - 5), 2f);
                Player.Instance.SetLayerOrder(0);
                break;
            case GameLayer.Sky:
                CombatHandler.Instance.UnpauseSky();
                camTween = cam.transform.DOMove(new Vector3(0, 0, _SKY - 5), 2f);
                Player.Instance.SetLayerOrder(20);
                break;
            case GameLayer.Space:
                CombatHandler.Instance.UnpauseSpace();
                camTween = cam.transform.DOMove(new Vector3(0, 0, _SPACE - 5), 2f);
                Player.Instance.SetLayerOrder(200);
                break;
        }
        canSwitchLayer = false;
        Tween playerTween = Player.Instance.StartChangeLayer();
        playerTween.onComplete += TweenOnComplete;
        camTween.SetEase(Ease.InOutSine);
        AudioHandler.Instance.StopSong(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    private void TweenOnComplete()
    {
        canSwitchLayer = true;
        AudioHandler.Instance.PlayStageMusic(currentLayer);
        _UpdateStage?.Invoke();
    }
    public float LayerToOffset()
    {
        switch(currentLayer)
        {
            case GameLayer.Ground:
                return 0f;
            case GameLayer.Sky:
                return -20f;
            case GameLayer.Space:
                return -200f;
        }
        return 0f;
    }
    public void AddScore(int value)
    {
        int currentMult = scoreMult;
        multProgress ++;
        if (multProgress >= 10)
        {
            multProgress = 0;
            scoreMult = Mathf.Clamp(scoreMult + 1, 1, 5);
        }
        if (currentMult != scoreMult)
        {
            RuntimeManager.PlayOneShot(multiplierRef);
        }
        score += value * scoreMult;
        score = Mathf.Clamp(score, 0, 999999999);
        _UpdateScore?.Invoke();
    }
    public void ResetMult()
    {
        scoreMult = 1;
        multProgress = 0;
        _UpdateScore?.Invoke();
    }
    public void ResetScore()
    {
        score = 0;
        ResetMult();
    }
    public void ResetLives()
    {
        lives = 3;
        _UpdateLives?.Invoke();
    }
    public void AddLife()
    {
        lives++;
        lives = Mathf.Clamp(lives, 0, 5);
        _UpdateLives?.Invoke();
    }
    public void RemoveLife()
    {
        lives--;
        lives = Mathf.Clamp(lives, 0, 5);
        _UpdateLives?.Invoke();

        if (lives == 0)
        {
            Fail();
        }
    }
    public void Fail()
    {
        GameManager.Instance.SetTempScore(score, false);
        CombatHandler.Instance.Pause();
        AudioHandler.Instance.StopSong(FMOD.Studio.STOP_MODE.IMMEDIATE);
        scoreHandler.Enable();
    }
    public void Win()
    {
        GameManager.Instance.SetTempScore(score, true);
        Player.Instance.Win();
        CombatHandler.Instance.Pause();
        AudioHandler.Instance.StopSong(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        winFade.DOFade(1, 3f);
        StartCoroutine(IEndGame());
    }
    private IEnumerator IEndGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene((int)GameManager.Levels.Highscore);
    }
    public bool IsOnPlayerLayer(float zOffset)
    {
        GameLayer offsetLayer = GameLayer.Ground;
        switch (zOffset)
        {
            case 0:
                offsetLayer = GameLayer.Ground;
                break;
            case -20:
                offsetLayer = GameLayer.Sky;
                break;
            case -200:
                offsetLayer = GameLayer.Space;
                break;
        }
        return offsetLayer == currentLayer;
    }
    
    void Update()
    {
        if (debugControls)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ascend();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Descend();
            }
        }
    }
}
