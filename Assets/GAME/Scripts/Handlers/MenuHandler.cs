using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private EventReference musicRef;
    private EventInstance musicInst;
    // Start is called before the first frame update
    void Start()
    {
        musicInst = RuntimeManager.CreateInstance(musicRef);
        musicInst.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            musicInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            SceneManager.LoadScene((int)GameManager.Levels.Game);
        }
    }
}
