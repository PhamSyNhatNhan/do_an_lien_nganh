using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_4_meleeAttack : MonoBehaviour
{
    private DamageParameters parameters;
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox;
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackWidth;
    
    void Start()
    {
        StartCoroutine(Melee_Attack());
    }
    
    IEnumerator Melee_Attack()
    {
        yield return new WaitForSeconds(0.2f);

        Vector2 boxSize = new Vector2(AttackWidth, AttackRange);
        Vector2 boxCenter = AttackHitBox.position;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, AttackHitBox.eulerAngles.z, WhatIsDamgeEnable);

        if (hitColliders.Length != 0)
        {
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
        AttackRange *= scaleObject;
        AttackWidth *= scaleObject;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 boxSize = new Vector2(AttackWidth, AttackRange);
        Vector2 boxCenter = (AttackHitBox.position);

        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}