using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private GameObject bulletRef;
    [SerializeField] private Vector2 fireDirection;
    [SerializeField] private int levelRequired;

    private SpriteRenderer imageFlash;
    private bool isEnabled;

    // Start is called before the first frame update
    void Awake()
    {
        isEnabled = false;
        imageFlash = GetComponent<SpriteRenderer>();
        imageFlash.enabled = false;
    }

    // Update is called once per frame
    public void PowerUp()
    {
        isEnabled = true;
    }
    public void PowerDown()
    {
        isEnabled = false;
    }
    public void Shoot()
    {
        if (isEnabled)
        {
            GameObject bullet = Instantiate(bulletRef, transform.position, quaternion.identity);
            bullet.GetComponent<PlayerProjectile>().INIT(fireDirection.normalized, Player.Instance.damage);
            StartCoroutine(IShoot());
        }
    }
    public IEnumerator IShoot()
    {
        imageFlash.enabled = true;
        yield return new WaitForSeconds(0.05f);
        imageFlash.enabled = false;
    }
}
