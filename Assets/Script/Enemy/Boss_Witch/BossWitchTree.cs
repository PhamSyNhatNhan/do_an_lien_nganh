using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWitchTree : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private GameObject explosion;
    private DamageParameters parameters;
    
    void Start()
    {
        StartCoroutine(destroyTime());
    }

    public void onInstantiate(DamageParameters parameters, float scaleObject)
    {
        this.parameters = parameters;
        transform.localScale = new Vector3(scaleObject, scaleObject, 1.0f);
    }
    
    IEnumerator destroyTime()
    {
        yield return new WaitForSeconds(time);

        GameObject explosion_ = Instantiate(explosion, transform);
        explosion_.GetComponent<BossWitchExplosion>().onInstantiate(parameters, 1.0f);
        explosion_.transform.parent = null;
        Destroy(gameObject);
    }
}
