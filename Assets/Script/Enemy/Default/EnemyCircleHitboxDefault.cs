using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCircleHitboxDefault : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;

    private DamageParameters parameters;
    
    void Start()
    {
        Dmg();
    }
    
    public void onInstantiate(DamageParameters parameters, float scaleObject)
    {
        this.parameters = parameters;
        transform.localScale = new Vector3(scaleObject, scaleObject, 1.0f);
        AtackRadius *= scaleObject;
    }

    private void Dmg()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(AttackHitBox.position, AtackRadius, WhatIsDamgeEnable);


        if (DetectObject.Length != 0)
        {
            foreach(Collider2D Colider in DetectObject)
            {
                DamageParameters parameters = this.parameters;
                
                Colider.transform.SendMessage("Damage", parameters);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}
