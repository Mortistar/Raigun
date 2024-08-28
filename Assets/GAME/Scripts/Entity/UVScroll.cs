using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UVScroll : MonoBehaviour
{
    [SerializeField] private StageHandler.GameLayer layer;
    private const float baseSpeed = 2f;

    private float currentSpeed;

    // Update is called once per frame
    private Image img;
    void Awake()
    {
        img = GetComponent<Image>();
        img.material.mainTextureOffset = Vector2.zero;

        switch(layer)
        {
            case StageHandler.GameLayer.Ground:
                currentSpeed = baseSpeed;
                break;
            case StageHandler.GameLayer.Sky:
                currentSpeed = baseSpeed * 2f;
                break;
            case StageHandler.GameLayer.Space:
                currentSpeed = baseSpeed * 4f;
                break;
        }
    }
    void Update()
    {
        img.material.mainTextureOffset += Vector2.up * currentSpeed * Time.deltaTime;
    }
    void OnDisable()
    {
        img.material.mainTextureOffset = Vector2.zero;
    }
}
