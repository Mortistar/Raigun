using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioHandler : MonoBehaviour
{
    public enum Song
    {
        Intro,
        Menu,
        StageOne,
        StageTwo,
        StageThree,
        Boss,
    }
    [SerializeField] private EventReference songIntro;
    [SerializeField] private EventReference songMenu;
    [SerializeField] private EventReference songStageOne;
    [SerializeField] private EventReference songStageTwo;
    [SerializeField] private EventReference songStageThree;
    [SerializeField] private EventReference songBossOne;

    public static AudioHandler Instance;

    public Dictionary<Song,EventReference> songs {get; private set;}

    public FMOD.Studio.EventInstance currentSong {get; private set;}
    public EventReference currentSongRef {get; private set;}

    private EventInstance music;

    void Awake()
    {
        PersistenceCheck();
    }
    private void PersistenceCheck()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            INIT();
        }else
        {
            Destroy(this.gameObject);
        }
    }
    private void INIT()
    {
        //Initialisation
        InitSongs();
    }
    private void InitSongs()
    {
        songs = new Dictionary<Song, EventReference>()
        {
            {Song.Intro, songIntro},
            {Song.Menu, songMenu},
            {Song.StageOne, songStageOne},
            {Song.StageTwo, songStageTwo},
            {Song.StageThree, songStageThree},
            {Song.Boss, songBossOne},
        };
    }
    public void PlayStageMusic(StageHandler.GameLayer layer)
    {
        Song song = Song.StageOne;
        switch (layer)
        {
            case StageHandler.GameLayer.Ground:
                song = Song.StageOne;
                break;
            case StageHandler.GameLayer.Sky:
                song = Song.StageTwo;
                break;
            case StageHandler.GameLayer.Space:
                song = Song.StageThree;
                break;
        }
        PlaySong(song, FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void QueueSong(Song songToPlay)
    {
        PlaySong(songToPlay, FMOD.Studio.STOP_MODE.IMMEDIATE);
        currentSong.setPaused(true);
    }
    public void PlaySong(Song songToPlay, FMOD.Studio.STOP_MODE stopMode)
    {
        //Convert enum to music reference
        EventReference musicQueued = songs[songToPlay];
        //Music state checks
        currentSong.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);

        //If current song paused
        currentSong.getPaused(out bool isPaused);
        if (isPaused)
        {
            //If queued music is the same as current music, do nothing
            if (musicQueued.Guid == currentSongRef.Guid)
            {
                currentSong.setPaused(false);
                return;
            }
        }

        //If current music playing
        if (currentSong.isValid() && state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            //If queued music is the same as current music, do nothing
            if (musicQueued.Guid == currentSongRef.Guid)
            {
                return;
            }
            currentSong.stop(stopMode);
        }
        currentSong = RuntimeManager.CreateInstance(musicQueued);
        currentSong.start();
        currentSongRef = musicQueued;
    }
    public void StopSong(FMOD.Studio.STOP_MODE stopMode)
    {
        currentSong.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE state);
        if (currentSong.isValid() && (state == FMOD.Studio.PLAYBACK_STATE.PLAYING))
        {
            currentSong.stop(stopMode);
        }
    }
    void OnDisable()
    {
        StopSong(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
