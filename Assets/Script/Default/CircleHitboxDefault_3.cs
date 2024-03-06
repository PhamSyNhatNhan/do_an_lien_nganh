using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHitboxDefault_3 : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;

    private PlayerHealth ph;
    
    void Start()
    {
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        Dmg();
    }


    private void Dmg()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(AttackHitBox.position, AtackRadius, WhatIsDamgeEnable);


        if (DetectObject.Length != 0)
        {
            foreach(Collider2D Colider in DetectObject)
            {
                DamageParameters parameters = ph.Parameters;
                
                Colider.transform.SendMessage("Damage", parameters);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}
