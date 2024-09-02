using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class IntroMusic : MonoBehaviour
{
    [SerializeField] private EventReference musicToPlay;

    private EventInstance inst;
    void Start()
    {
        inst = RuntimeManager.CreateInstance(musicToPlay);
        inst.start();
    }
    void OnDisable()
    {
        inst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
