using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private EventReference bulletRef;
    [SerializeField] private float speed;

    private Camera cam;

    private float damage;
    private Vector3 dir;

    public void INIT(Vector2 Dir, float Damage)
    {
        damage = Damage;
        dir = Dir;
    }
    void Start()
    {
        cam = Camera.main;
        Player.Instance.shotList.Add(gameObject);
        RuntimeManager.PlayOneShot(bulletRef);
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        //Delete if out of bounds
        if(transform.position.y >= 3f)
        {
            OnDeath();
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            col.transform.GetComponent<IDamage>().Damage(damage);
            OnDeath();
        }
    }
    private void OnDeath()
    {
        Player.Instance.shotList.Remove(gameObject);
        Destroy(gameObject);
    }
}
