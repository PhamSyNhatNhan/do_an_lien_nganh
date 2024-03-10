using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2_Boom : MonoBehaviour
{
    private DamageParameters parameters;
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;
    [SerializeField] private GameObject death_enemy2;
    void Start()
    {
        StartCoroutine(Explode());
    }
    
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(0.2f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, AtackRadius, WhatIsDamgeEnable);

        if (hitColliders.Length != 0)
        {
            foreach (var player in hitColliders)
            {
                player.transform.SendMessage("Damage", parameters);
            }
        }

        StartCoroutine(Death_e2());
    }

    IEnumerator Death_e2()
    {
        yield return new WaitForSeconds(0.05f);
        GameObject e2_death = Instantiate(death_enemy2, transform);
        e2_death.transform.parent = null;
        Destroy(gameObject);
    }
    
    public void onInstantiate(DamageParameters parameters, float scaleObject)
    {
        this.parameters = parameters;
        transform.localScale = new Vector3(scaleObject, scaleObject, 1.0f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}
