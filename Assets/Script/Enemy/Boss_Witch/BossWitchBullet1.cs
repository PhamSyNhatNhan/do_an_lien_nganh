using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossWitchBullet1 : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;

    private DamageParameters parameters;
    [SerializeField] private float time;
    [SerializeField] private GameObject bulletObject;
    
    void Start()
    {
        StartCoroutine(destroyTime());
        StartCoroutine(subSpaenBullet());
    }

    public void onInstantiate(DamageParameters parameters, float scaleObject)
    {
        this.parameters = parameters;
        transform.localScale = new Vector3(scaleObject, scaleObject, 1.0f);
    }

    void Update()
    {
        Dmg();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }


    private void Dmg()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(AttackHitBox.position, AtackRadius, WhatIsDamgeEnable);

        if (DetectObject.Length != 0)
        {
            GameObject Player = DetectObject[0].gameObject;
            Player.transform.SendMessage("Damage", parameters);
            Destroy(gameObject);
        }
    }

    IEnumerator subSpaenBullet()
    {
        while (true)
        {
            spawnObject();
            
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    private void spawnObject()
    {
        Vector3 direction = transform.forward;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        float randomAngle = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle);

        Quaternion finalRotation = rotation * randomRotation; 
        
        Vector3 attackPositionTmp = transform.position + (finalRotation * Vector3.forward);

        DamageParameters tmpParameters = parameters.Clone();
        tmpParameters.BaseDmg *= 0.3f;
        GameObject newBullet = Instantiate(bulletObject, attackPositionTmp, finalRotation);
        newBullet.GetComponent<CircleHitboxDefaultTime>().onInstantiate(tmpParameters, 0.5f);
    }

    IEnumerator destroyTime()
    {
        yield return new WaitForSeconds(time);
        
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}
