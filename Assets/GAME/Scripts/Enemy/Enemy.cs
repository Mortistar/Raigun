using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamage
{
    public enum MoveType
    {
        Scroll,
        Static,
        Homing
    }

    private Sprite sprMain;
    [SerializeField] private bool isDebug;
    [SerializeField] private Sprite sprFlash;
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private MoveType moveType;
    [SerializeField] private int scoring = 100;

    private bool dieOnCollision;

    private float currentHealth;
    
    private SpriteRenderer ren;
    private BoxCollider col;

    private bool canMove = true;

    void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider>();
        col.enabled = false;
        sprMain = ren.sprite;
        currentHealth = health;
        foreach(EnemyGun gun in GetComponentsInChildren<EnemyGun>())
        {
            gun.enabled = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (isDebug)
        {
            return;
        }
        canMove = false;
    }
    public void INIT(Vector3 targetPos)
    {
        Tween tw = transform.DOMove(targetPos, 1f);
        tw.onComplete += Begin;
    }
    private void Begin()
    {
        canMove = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (col.enabled == false)
        {
            if (transform.position.y < 2.5f)
            {
                col.enabled = true;
                foreach(EnemyGun gun in GetComponentsInChildren<EnemyGun>())
                {
                    gun.enabled = true;
                }
            } 
        }
        if (canMove)
        {
            Move();
        }
        
    }
    private void Move()
    {
        switch (moveType)
        {
            case MoveType.Scroll:
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y <= -4f)
                {
                    Death();
                }
                break;
            case MoveType.Static:
                if (moveType == MoveType.Static && !StageHandler.Instance.IsOnPlayerLayer(transform.position.z))
                {
                    Destroy(gameObject);
                    return;
                }
                break;
            case MoveType.Homing:
                Vector3 target = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position.y <= Player.Instance.transform.position.y + 0.2f)
                {
                    float newAngle = Mathf.Atan2(-1, 0) * Mathf.Rad2Deg;
                    newAngle -= 90f;
                    transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
                    moveType = MoveType.Scroll;
                    return;
                }
                if (Vector3.Distance(transform.position, target) <= 0.01f)
                {
                    Death();
                }
                
                Vector3 dir = Player.Instance.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                angle -= 90f;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                break;
        }
    }
    public void Damage(float damage)
    {
        StartCoroutine(DamageBlink());
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Killed();
        }
    }
    private void Death()
    {
        Destroy(gameObject);
    }
    private void Killed()
    {
        StageHandler.Instance.AddScore(scoring);
        CombatHandler.Instance.SpawnExplosion(CombatHandler.ExplosionType.normal, transform.position);
        Death();
    }
    private IEnumerator DamageBlink()
    {
        ren.sprite = sprFlash;
        yield return new WaitForSeconds(0.02f);
        ren.sprite = sprMain;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.transform.GetComponent<IDamage>().Damage(1);
            if (dieOnCollision)
            {
                Killed();
            }
        }
    }
    void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
