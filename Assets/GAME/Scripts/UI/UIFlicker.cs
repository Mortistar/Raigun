using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIFlicker : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float animSpeed;
    private Image image;
    private float timer;
    private int index;
    
    void Awake()
    {
        image = GetComponent<Image>();
        timer = animSpeed;
        index = 0;
    }
    void Update()
    {
        if (timer <= 0)
        {
            timer = animSpeed;
            index++;
            if (index >= sprites.Length)
            {
                index = 0;
            }
            image.sprite = sprites[index];
        }
        timer -= Time.deltaTime;
    }
}
