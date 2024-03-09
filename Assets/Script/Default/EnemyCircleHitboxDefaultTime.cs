using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHitboxDefaultTime : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;

    private DamageParameters parameters;
    [SerializeField] private float time;
    
    void Start()
    {
        StartCoroutine(destroyTime());
    }

    public void onInstantiate(DamageParameters parameters, float scaleObject)
    {
        this.parameters = parameters;
        transform.localScale = new Vector3(scaleObject, scaleObject, 1.0f);
        AtackRadius *= scaleObject;
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
