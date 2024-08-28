using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image imgStage;
    [SerializeField] private Image imgName;
    [Header("Sprites")]
    [SerializeField] private Sprite[] spriteStages;
    [SerializeField] private Sprite[] spriteNames;

    void Start()
    {
        UpdateStage();
        StageHandler.Instance._UpdateStage += UpdateStage;
    }
    public void UpdateStage()
    {
        int layer = (int)StageHandler.Instance.currentLayer;
        imgStage.sprite = spriteStages[layer];
        imgName.sprite = spriteNames[layer];
    }
    void OnDisable()
    {
        StageHandler.Instance._UpdateStage -= UpdateStage;
    }
}
