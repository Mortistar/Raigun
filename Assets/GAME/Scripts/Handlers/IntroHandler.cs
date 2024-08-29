using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroHandler : MonoBehaviour
{
    private bool canSkip;

    void Awake()
    {
        canSkip = true;
        StartCoroutine(MenuTransition());
    }
    private IEnumerator MenuTransition()
    {
        yield return new WaitForSeconds(4f);
        canSkip = false;
        SceneManager.LoadScene((int)GameManager.Levels.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSkip && Input.GetKeyDown(KeyCode.Z))
        {
            StopAllCoroutines();
            SceneManager.LoadScene((int)GameManager.Levels.Menu);
        }
    }
}
