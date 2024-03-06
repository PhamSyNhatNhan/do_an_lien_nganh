using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CircleHitboxDefault_2 : MonoBehaviour
{
    [SerializeField] private int numberAttack;
    [SerializeField] private LayerMask WhatIsDamgeEnable;
    [SerializeField] private Transform[] attackHitBoxes;
    [SerializeField] private float[] attackRadiuses;
    [SerializeField] private float[] timeAttack;

    private PlayerHealth ph;
    

    void Start()
    {
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
        StartCoroutine(DmgCoroutine());
    }
    

    private IEnumerator DmgCoroutine()
    {
        for (int i = 0; i < numberAttack; i++)
        {
            //Debug.Log("attack " + i);
            Collider2D[] DetectObject = Physics2D.OverlapCircleAll(attackHitBoxes[i].position, attackRadiuses[i], WhatIsDamgeEnable);

            if (DetectObject.Length != 0)
            {
                //Debug.Log("attack: " + i);
                foreach (Collider2D Colider in DetectObject)
                {
                    DamageParameters parameters = ph.Parameters;
                    Colider.transform.SendMessage("Damage", parameters);
                }
            }

            if(i < numberAttack -1)
                yield return new WaitForSeconds(timeAttack[i]); 
        }
    }

    
    private void OnDrawGizmos()
    {
        for (int i = 0; i < numberAttack; i++)
        {
            Gizmos.DrawWireSphere(attackHitBoxes[i].position, attackRadiuses[i]);
        }
    }
}
