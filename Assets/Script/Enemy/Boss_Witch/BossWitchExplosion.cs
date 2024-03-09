using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWitchExplosion : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;

    private DamageParameters parameters;
    
    void Start()
    {
        StartCoroutine(Dmg());
    }

    public void onInstantiate(DamageParameters parameters, float scaleObject)
    {
        this.parameters = parameters;
        transform.localScale = new Vector3(scaleObject, scaleObject, 1.0f);
        AtackRadius *= scaleObject;
    }



    IEnumerator Dmg()
    {
        yield return new WaitForSeconds(0.15f);
        
        Debug.Log("1");
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(AttackHitBox.position, AtackRadius, WhatIsDamgeEnable);
        Debug.Log("2");
        if (DetectObject.Length != 0)
        {
            GameObject Player = DetectObject[0].gameObject;
            Player.transform.SendMessage("Damage", parameters);
        }
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}
