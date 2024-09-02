using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private EventReference typeRef;
    private bool canSkip;

    private string queueText;
    private int typeIndex;
    private float typeTimer;
    private const float typeInterval = 0.06f;

    void Awake()
    {
        canSkip = true;

        queueText = text.text;
        text.text = "";
        typeTimer = typeInterval;
        typeIndex = 0;
    }
    private void TypeWriter()
    {
        // If Complete
        if (typeIndex >= queueText.Length)
        {
            Load();
            return;
        }
        //Play sound if letter
        if (queueText[typeIndex].ToString() != " ")
        {
            RuntimeManager.PlayOneShot(typeRef);
        }
        text.text += queueText[typeIndex].ToString();
        typeIndex++;
    }
    private void Load()
    {
        canSkip = false;
        StartCoroutine(ILoad());
    }
    private IEnumerator ILoad()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene((int)GameManager.Levels.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        if (canSkip)
        {
            if (typeTimer <= 0)
            {
                TypeWriter();
                typeTimer = typeInterval;
            }
            typeTimer -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Z))
            {
                canSkip = false;
                StopAllCoroutines();
                SceneManager.LoadScene((int)GameManager.Levels.Menu);
            }
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
}
