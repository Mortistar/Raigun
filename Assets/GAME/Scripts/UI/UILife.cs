using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILife : MonoBehaviour
{
    [SerializeField] private GameObject lifeRef;
    [SerializeField] private RectTransform lifeParent;

    private List<GameObject> lives;
    void Awake()
    {
        //Destroy all child objects
        while (lifeParent.childCount > 0)
        {
            DestroyImmediate(lifeParent.GetChild(0).gameObject);
        }
    }
    void Start()
    {
        lives = new List<GameObject>();
        StageHandler.Instance._UpdateLives += UpdateUI;
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        int currentLives = StageHandler.Instance.lives;

        if (currentLives == lives.Count)
        {
            return;
        }
        if (currentLives > lives.Count)
        {
            int livesToAdd = currentLives - lives.Count;
            for (int i = 0; i < livesToAdd ; i++)
            {
                GameObject newLife = Instantiate(lifeRef);
                newLife.transform.SetParent(lifeParent, false);
                lives.Add(newLife);
            }
        }
        if (currentLives < lives.Count)
        {
            int livesToRemove = lives.Count - currentLives;
            for (int i = 0; i < livesToRemove; i++)
            {
                int index = lives.Count - 1;
                GameObject deadLife = lives[index];
                lives.RemoveAt(index);
                Destroy(deadLife);
            }
        }
    }
    void OnDisable()
    {
        StageHandler.Instance._UpdateLives -= UpdateUI;
    }
}
