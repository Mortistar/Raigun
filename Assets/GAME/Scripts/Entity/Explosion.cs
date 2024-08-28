using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Vector2 speedRange;
    [SerializeField] private Sprite[] sprites;

    private SpriteRenderer ren;
    private EventReference explosionRef;

    private float animTimer = 0;

    private float speedInterval;
    private int animIndex;

    // Start is called before the first frame update
    void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        explosionRef = RuntimeManager.PathToEventReference("event:/SFX/Ship/sfx_explosion");
    }
    void Start()
    {
        explosionRef = RuntimeManager.PathToEventReference("event:/SFX/Ship/sfx_explosion");
        RuntimeManager.PlayOneShot(explosionRef);
        speedInterval = Random.Range(speedRange.x, speedRange.y);
        animTimer += speedInterval;
        animIndex = 0;
        ren.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (animTimer <= 0)
        {
            if (animIndex + 1 > 3)
            {
                Destroy(gameObject);
                return;
            }
            animIndex ++;
            ren.sprite = sprites[animIndex];
            animTimer = speedInterval;
        }
        animTimer -= Time.deltaTime;
    }
}
