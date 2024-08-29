using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IDamage
{
    public static Player Instance;

    [Header("Debug")]
    [SerializeField] private bool isGodmode;

    [Header("Settings")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float invulnPeriod = 3f;
    [SerializeField] private int shotCount = 8;
    [SerializeField] private float thrustSpeed;

    [Header("References")]
    [SerializeField] private PlayerGun[] guns;
    [SerializeField] private Sprite[] mainSprites;
    [SerializeField] private Sprite[] flashSprites;

    [SerializeField] private SpriteRenderer thruster;
    
    [SerializeField] private Sprite[] thrustSprites;
    [SerializeField] private Sprite[] thrustMinSprites;

    [SerializeField] private Sprite spriteLeftFull;
    [SerializeField] private Sprite spriteRightFull;

    private Camera cam;
    private SpriteRenderer ren;
    private BoxCollider col;

    private float borderPadding;

    private int thrustIndex;
    private float thrustTimer;

    private bool canMove = true;
    private bool canShoot = true;
    private bool isInvuln = false;
    public bool canFly {get; private set;}

    private float fireTimer;
    private float invulnTimer;

    public List<GameObject> shotList;
    private int currentShotCount;

    private EventReference thrusterRef;
    private EventInstance thrusterInst;
    
    private EventReference maxPowerRef;
    private EventReference powerRef;

    //Stats
    public int currentLevel {get; private set;}
    public float damage {get; private set;}
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            INIT();
        }else
        {
            Destroy(gameObject);
        }
    }
    private void INIT()
    {
        damage = 1f;
        shotList = new List<GameObject>();
        currentShotCount = shotCount;
        ren = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider>();
        thrustTimer = thrustSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {   
        thrusterRef = RuntimeManager.PathToEventReference("event:/SFX/Ship/sfx_thrust_low");

        powerRef = RuntimeManager.PathToEventReference("event:/SFX/UI/sfx_Power");
        maxPowerRef = RuntimeManager.PathToEventReference("event:/SFX/UI/sfx_PowerMax");
        
        thrusterInst = RuntimeManager.CreateInstance(thrusterRef);
        borderPadding = Screen.height / 20;
        cam = Camera.main;

        Reset();
    }
    private void Reset()
    {
        StageHandler.Instance.ResetMult();
        ResetLevel();
        SetMove(false);
        SetInvulnerable();
        transform.position = new Vector3(0, -6, StageHandler.Instance.LayerToOffset());
        Tween tw = transform.DOMove(new Vector3 (0, -1.5f, StageHandler.Instance.LayerToOffset()), 1.5f);
        tw.SetEase(Ease.OutQuint);
        tw.onComplete += Begin;
    }
    private void Begin()
    {
        SetMove(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateThruster();
        if (canMove)
        {
            Movement();
        }
        if (canShoot)
        {
            Shoot();
        }
        if (isInvuln)
        {
            Invulnerable();
        }else
        {
            ren.sprite = mainSprites[MoveToSpriteIndex()];
        }

        if (invulnTimer >= 0)
        {
            invulnTimer -= Time.deltaTime;
        }
        if (fireTimer >= 0)
        {
            fireTimer -= Time.deltaTime;
        }
    }
    private void UpdateThruster()
    {
        if (thrustTimer <= 0)
        {
            thrustIndex ++;
            if (thrustIndex > 3)
            {
                thrustIndex = 0;
            }
            thruster.sprite = Input.GetKey(KeyCode.UpArrow) ? thrustSprites[thrustIndex] : thrustMinSprites[thrustIndex];
            thrustTimer = thrustSpeed;
        }
        thrustTimer -= Time.deltaTime;
    }
    private void Movement()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(xMove, yMove, 0).normalized;

        Vector3 movement = direction * speed * Time.deltaTime;

        movement = Collision(movement);

        transform.position += movement;
    }
    private void Shoot()
    {
        if (shotList.Count > currentShotCount)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            if (fireTimer <= 0)
            {
                foreach(PlayerGun gun in guns)
                {
                    gun.Shoot();
                }
                fireTimer = fireRate;
            }
        }
    }
    private void Invulnerable()
    {
        if (invulnTimer % 0.2 <= 0.1f)
        {
            if (canMove)
            {
                ren.sprite = flashSprites[MoveToSpriteIndex()];
            }else
            {
                ren.sprite = flashSprites[1];
            }
            
        }else
        {
            if (canMove)
            {
                ren.sprite = mainSprites[MoveToSpriteIndex()];
            }else
            {
                ren.sprite = mainSprites[1];
            }
        }
        if (invulnTimer <= 0)
        {
            col.enabled = true;
            isInvuln = false;
            ren.sprite = mainSprites[MoveToSpriteIndex()];
        }
    }
    private int MoveToSpriteIndex()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            return 1;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            return 0;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            return 2;
        }
        return 1;
    }
    private Vector3 Collision(Vector3 movement)
    {
        //World to screen point
        Vector3 newPos = transform.position + movement;

        //If outside X (horizontal) bounds
        if (newPos.x < -1.8f || newPos.x > 1.8f)
        {
            movement = new Vector3(0, movement.y, 0);
        }
        //If outside Z (horizontal) bounds
        if (newPos.y < -2.5f || newPos.y > 2.5f)
        {
            movement = new Vector3(movement.x, 0, 0);
        }
        return movement;
    }
    public void SetInvulnerable()
    {
        col.enabled = false;
        invulnTimer = invulnPeriod;
        isInvuln = true;
    }
    public void SetMove(bool CanMove)
    {
        canMove = CanMove;
        canShoot = canMove;
        col.enabled = canMove;
    }
    public void Damage(float damage)
    {
        if (isInvuln || isGodmode)
        {
            return;
        }
        CombatHandler.Instance.SpawnExplosion(CombatHandler.ExplosionType.normal, transform.position);
        StageHandler.Instance.RemoveLife();
        if (StageHandler.Instance.lives > 0)
        {
            Reset();
        }else
        {
            Kill();
        }
    }
    private void Kill()
    {
        SetMove(false);
        col.enabled = false;
        transform.position = new Vector3(0, -6, StageHandler.Instance.LayerToOffset());
    }
    public void SetCanFly(bool CanFly)
    {
        canFly = CanFly;
    }
    public void ClearBullets()
    {
        foreach(GameObject obj in shotList)
        {
            Destroy(obj);
        }
        shotList.Clear();
    }
    public void SetLayerOrder(int order)
    {
        ren.sortingOrder = order;
    }
    public void ClearStage()
    {
        col.enabled = false;
        SetMove(false);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y, -350f), 3f);
    }
    public void LevelUp()
    {
        if (currentLevel == 4)
        {
            RuntimeManager.PlayOneShot(maxPowerRef);
            return;
        }
        RuntimeManager.PlayOneShot(powerRef);
        currentLevel = Mathf.Clamp(currentLevel + 1, 1, 4);
        switch (currentLevel)
        {
            case 2:
                currentShotCount = shotCount * 3;
                guns[1].PowerUp();
                guns[2].PowerUp();
                damage = 0.5f;
                break;
            case 3:
                currentShotCount = shotCount * 4;
                guns[3].PowerUp();
                damage = 0.4f;
                break;
            case 4:
                currentShotCount = shotCount * 6;
                guns[4].PowerUp();
                guns[5].PowerUp();
                damage = 0.3f;
                break;
        }
    }
    private void ResetLevel()
    {
        foreach(PlayerGun gun in guns)
        {
            gun.PowerDown();
        }
        //Base Level
        currentLevel = 1;
        damage = 1;
        currentShotCount = shotCount;
        guns[0].PowerUp();
    }
    public void StartThruster()
    {
        thrusterInst.start();
    }
    public Tween StartChangeLayer()
    {
        SetMove(false);
        ClearBullets();
        thrusterInst.start();

        Tween tw = transform.DOMove(new Vector3(transform.position.x, transform.position.y, StageHandler.Instance.LayerToOffset()), 2f);
        tw.SetEase(Ease.InOutQuad);
        tw.onComplete += StopChangeLayer;
        return tw;
    }
    public void StopChangeLayer()
    {
        SetMove(true);
        SetInvulnerable();
        thrusterInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    public void Win()
    {
        SetMove(false);
        Tween tw = transform.DOMove(transform.position + Vector3.up * 3, 2f);
        tw.SetEase(Ease.InBack);
    }
}
