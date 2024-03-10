using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3_MeleeAttack : MonoBehaviour
{
    private DamageParameters parameters;
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;

    void Start()
    {
        StartCoroutine(Melee_Attack());
    }
    
    IEnumerator Melee_Attack()
    {
        yield return new WaitForSeconds(0.2f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, AtackRadius, WhatIsDamgeEnable);

        if (hitColliders.Length != 0)
        {
            Debug.Log(parameters.ToString());
            foreach (var player in hitColliders)
            {
                player.transform.SendMessage("Damage", parameters);
            }
        }
    }
    
    public void onInstantiate(DamageParameters parameters, float scaleObject)
    {
        this.parameters = parameters;
        transform.localScale = new Vector3(scaleObject, scaleObject, 1.0f);
        AtackRadius *= scaleObject;
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}