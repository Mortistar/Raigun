using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBig : MonoBehaviour
{
    [SerializeField] private GameObject explosionRef;
    [SerializeField] private float delay;
    
    private int boomAmount;
    private float boomRadius;

    private float currentDelay;
    void Start()
    {
        currentDelay = 0;
        boomAmount = Random.Range(10,20);
        for (int i = 0; i <= boomAmount; i++)
        {
            Vector3 newPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), StageHandler.Instance.LayerToOffset());
            StartCoroutine(IExplode(currentDelay, newPos));
            currentDelay += delay;
        }
        StartCoroutine(IKill());
    }
    private IEnumerator IExplode(float delay, Vector3 position)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(explosionRef, position, Quaternion.identity);
    }
    private IEnumerator IKill()
    {
        yield return new WaitForSeconds(currentDelay + 1f);
        Destroy(gameObject);
    }
}
