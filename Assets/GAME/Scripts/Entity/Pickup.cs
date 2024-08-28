using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        Life,
        Canister,
        Coin,
        CoinBig,
        Powerup
    }
    [Header("References")]
    [SerializeField] private PickupType pickupType;
    [SerializeField] private EventReference pickupRef;

    private float speed;

    // Start is called before the first frame update
    void Awake()
    {
        speed = Random.Range(0.5f,2);
    }
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (transform.position.y <= -4f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            RuntimeManager.PlayOneShot(pickupRef);
            switch (pickupType)
            {
                case PickupType.Life:
                    StageHandler.Instance.AddLife();
                    break;
                case PickupType.Canister:
                    //play audio
                    StageHandler.Instance.AddScore(1000);
                    StageHandler.Instance.AddLife();
                    if (Player.Instance.canFly)
                    {
                        StageHandler.Instance.Ascend();
                    }
                    break;
                case PickupType.Coin:
                    StageHandler.Instance.AddScore(100);
                    break;
                case PickupType.CoinBig:
                    StageHandler.Instance.AddScore(500);
                    break;
                case PickupType.Powerup:
                    StageHandler.Instance.AddScore(100);
                    Player.Instance.LevelUp();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
