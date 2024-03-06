using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMaster_FinalBurst : MonoBehaviour
{
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform AttackHitBox; 
    [SerializeField] private float AtackRadius;
    [SerializeField] private float Speed;
    [SerializeField] private float shakeIntesity;
    [SerializeField] private float shakeTime;

    private PlayerHealth ph;
    private CameraShake cs;
    private Vector2 currentPosition;
    private Vector2 targetPosition;
    
    void Start()
    {
        currentPosition = transform.position;
        targetPosition = new Vector2(currentPosition.x, currentPosition.y - 9.6f); 

        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
        cs = GameObject.Find("Virtual Camera").GetComponent<CameraShake>();
        StartCoroutine(MoveAndDestroy());
    }
    
    IEnumerator MoveAndDestroy()
    {
        while (transform.position.y > targetPosition.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed);
            yield return null;
        }

        cs.ShakeCamera(shakeIntesity, shakeTime);
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(AttackHitBox.position, AtackRadius, WhatIsDamgeEnable);


        if (DetectObject.Length != 0)
        {
            foreach(Collider2D Colider in DetectObject)
            {
                DamageParameters parameters = ph.Parameters;
                
                Colider.transform.SendMessage("Damage", parameters);
            }
        }
        
        StartCoroutine(delayDestoy());
    }

    IEnumerator delayDestoy()
    {
        yield return new WaitForSeconds(0.2f);
        
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackHitBox.position, AtackRadius);
    }
}
