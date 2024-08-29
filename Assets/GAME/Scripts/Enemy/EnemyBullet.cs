using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public enum FollowType
    {
        Fixed,
        Homing
    }
    public List<Sprite> sprites;
    [SerializeField] private EventReference bulletRef;
    [SerializeField] private float animSpeed = 1f;
    [SerializeField] private float speed;
    [SerializeField] private bool randomSpeed = false;

    private SpriteRenderer ren;
    private Camera cam;

    private int spriteIndex;
    private float spriteTimer;

    private Vector3 moveDir;

    private EnemyGun gunRef;

    private float bulletSpeed;

    public void INIT(EnemyGun gun, Vector3 MoveDir)
    {
        if (randomSpeed)
        {
            bulletSpeed = Mathf.Clamp(Random.Range(speed - 2, speed + 2), 1f, 10f);
        }else
        {
            bulletSpeed = speed;
        }
        gunRef = gun;
        gunRef.shotList.Add(gameObject);
        moveDir = MoveDir;

        if (StageHandler.Instance.IsOnPlayerLayer(transform.position.z))
        {
            RuntimeManager.PlayOneShot(bulletRef);
        }
    }

    void Awake()
    {
        moveDir = Vector3.zero;
        //Rendering
        ren = GetComponentInChildren<SpriteRenderer>();
        ren.sprite = sprites[0];
        spriteIndex = 0;
        spriteTimer = animSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
    }
    private void Animate()
    {
        if (spriteTimer <= 0)
        {
            spriteIndex++;
            if (spriteIndex >= sprites.Count)
            {
                spriteIndex = 0;
            }
            ren.sprite = sprites[spriteIndex];
            spriteTimer = animSpeed;
        }
        spriteTimer -= Time.deltaTime;
    }
    private void Move()
    {
        transform.position += moveDir * bulletSpeed * Time.deltaTime;

        //Delete if out of bounds
        if (gunRef != null)
        {
            if(Vector2.Distance(transform.position, gunRef.transform.position) > 8f)
            {
                OnDeath();
            }
        }else
        {
            if(Vector2.Distance(transform.position, Vector3.zero) > 10f)
            {
                Destroy(gameObject);
            }
        }
        
    }
    void Update()
    {
        Animate();

        Move();
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.transform.GetComponent<IDamage>().Damage(1);
            OnDeath();
        }
    }
    private void OnDeath()
    {
        gunRef.shotList.Remove(gameObject);
        Destroy(gameObject);
    }

}
