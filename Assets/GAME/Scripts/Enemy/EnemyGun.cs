using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    private float fireTimer = 0f;

    [SerializeField] private EnemyGunData gunData;
    [SerializeField] private float delay = 0f;
    [SerializeField] private bool killBulletsOnDestroy = false;

    public List<GameObject> shotList {get; private set;}

    void Awake()
    {
        shotList = new List<GameObject>();
    }
    // Start is called before the first frame update
    void Start()
    {
        fireTimer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimer <= 0)
        {
            float currentShotDelay = 0f;
            fireTimer = gunData.reload;
            float currentMagSpread = gunData.spreadMag ? -gunData.spread / 2 : 0;
            float spreadMagInterval = gunData.spreadMag ? gunData.spread / gunData.magAmount : 0;
            float currentShellSpread = gunData.spreadShells ? -gunData.spread / 2: 0;
            float spreadShellInterval = gunData.spreadShells ? gunData.spread / gunData.bulletAmount: 0;
            
            for (int i = 0; i < gunData.magAmount; i++)
            {
                for (int j = 0; j < gunData.bulletAmount; j++)
                {
                    StartCoroutine(IFireBullet(currentShotDelay, currentMagSpread + currentShellSpread));
                    if (gunData.spreadShells)
                    {
                        currentShellSpread += spreadShellInterval;
                    }
                }
                currentShotDelay += gunData.fireRate;
                if (gunData.spreadMag)
                {
                    currentMagSpread += spreadMagInterval;
                }
            }
        }
        fireTimer -= Time.deltaTime;
    }
    private IEnumerator IFireBullet(float delay, float spread)
    {
        Vector3 direction;
        if (gunData.fireMode == EnemyGunData.FireMode.Aimed)
        {
            if (transform.position.y < Player.Instance.transform.position.y)
            {
                yield break;
            }
            Vector3 dir = Player.Instance.transform.position - transform.position;
            direction = new Vector3(dir.x, dir.y, 0).normalized;
        }else
        {
            direction = Vector3.down;
        }
        Vector3 spreadDir = Quaternion.AngleAxis(spread, Vector3.forward) * direction;

        yield return new WaitForSeconds(delay);

        GameObject bullet = Instantiate(gunData.bulletRef);
        bullet.transform.position = transform.position;
        bullet.GetComponent<EnemyBullet>().INIT(this, spreadDir);
    }

    void OnDisable()
    {
        if(!this.gameObject.scene.isLoaded) return;
        if (killBulletsOnDestroy)
        {
            float explodeDelay = 0;
            for (int i = 0; i < shotList.Count; i++)
            {
                CombatHandler.Instance.SpawnExplosionWithDelay(CombatHandler.ExplosionType.normal, shotList[i].transform.position, explodeDelay);
                explodeDelay += 0.02f;
                Destroy(shotList[i]);
                StageHandler.Instance.AddScore(50);
            }
        }
        shotList.Clear();
        StopAllCoroutines();
    }
}
