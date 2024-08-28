using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAlpha : MonoBehaviour
{
    private Transform camRef;
    private Renderer ren;
    [SerializeField] private Vector3 fixedPos;

    private const float _DISTANCE = 5f;

    void Awake()
    {
        ren = GetComponent<Renderer>();
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
        ren.material.color = new Color(ren.material.color.r, ren.material.color.g, ren.material.color.b, Mathf.Clamp(Vector3.Distance(fixedPos, camRef.position) / _DISTANCE, 0, 0.8f));
    }
}
