using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinSmallRef;
    [SerializeField] private GameObject coinBigRef;
    [SerializeField] private GameObject lifeRef;
    [SerializeField] private GameObject powerRef;
    

    [SerializeField] private float coinSmallInterval;
    [SerializeField] private float coinBigInterval;
    [SerializeField] private float lifeInterval;
    [SerializeField] private float powerInterval;


    private float timer;
    private float sinOffset;

    private float currentCoinSmall;
    private float currentCoinBig;
    private float currentLife;
    private float currentPower;

    void Awake()
    {
        timer = 0;
        currentCoinSmall = coinSmallInterval;
        currentCoinBig = coinBigInterval;
        currentLife = lifeInterval;
        currentPower = powerInterval;
    }

    void Update()
    {
        sinOffset = Mathf.Sin(timer) * 1.8f;
        transform.position = new Vector3(sinOffset, transform.position.y, StageHandler.Instance.LayerToOffset());

        if (timer >= currentCoinSmall)
        {
            GameObject coin = Instantiate(coinSmallRef, transform.position, Quaternion.identity);
            currentCoinSmall += coinSmallInterval;
        }
        if (timer >= currentCoinBig)
        {
            GameObject coinBig = Instantiate(coinBigRef, transform.position, Quaternion.identity);
            currentCoinBig += coinBigInterval;
        }
        if (timer >= currentLife)
        {
            GameObject life = Instantiate(lifeRef, transform.position, Quaternion.identity);
            currentLife += lifeInterval;
        }
        if (timer >= currentPower)
        {
            GameObject power = Instantiate(powerRef, transform.position, Quaternion.identity);
            currentPower += powerInterval;
        }
        timer += Time.deltaTime;
    }

}
