using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerAlpha : MonoBehaviour
{
    private CanvasGroup group;
    private Transform camRef;

    private const float _DISTANCE = 5f;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        camRef = Camera.main.transform;
        UpdateAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAlpha();
    }
    void UpdateAlpha()
    {
        if (Vector3.Distance(transform.position, camRef.position) > _DISTANCE * 2)
        {
            group.alpha = 0f;
            return;
        }
        group.alpha = Mathf.Clamp(Vector3.Distance(transform.position, camRef.position) / _DISTANCE, 0, 0.8f);
    }
}
