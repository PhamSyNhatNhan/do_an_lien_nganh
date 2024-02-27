using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleHitbox : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;

    private PlayerCombat pc;
    
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerCombat>();
        Dmg();
    }
    

    private void Dmg()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(AttackHitBox.position, AtackRadius, WhatIsDamgeEnable);


        if (DetectObject.Length != 0)
        {
            foreach(Collider2D Colider in DetectObject)
            {
                DamageParameters parameters = pc.Parameters;
                
                Colider.transform.SendMessage("Damage", parameters);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}
