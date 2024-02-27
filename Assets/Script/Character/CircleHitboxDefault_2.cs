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
    [SerializeField] private int[] timeAttack;

    private PlayerCombat pc;
    

    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerCombat>();
        StartDmgCoroutine();
    }
    

    private IEnumerator DmgCoroutine()
    {
        for (int i = 0; i < numberAttack; i++)
        {
            Collider2D[] DetectObject = Physics2D.OverlapCircleAll(attackHitBoxes[i].position, attackRadiuses[i], WhatIsDamgeEnable);

            if (DetectObject.Length != 0)
            {
                foreach(Collider2D Colider in DetectObject)
                {
                    DamageParameters parameters = pc.Parameters;
                
                    Colider.transform.SendMessage("Damage", parameters);
                }
            }

            if (i < numberAttack - 1)
            {
                yield return new WaitForSeconds(timeAttack[i]); 
            }
        }
    }

    private void StartDmgCoroutine()
    {
        StartCoroutine(DmgCoroutine());
    }

    
    private void OnDrawGizmos()
    {
        for (int i = 0; i < numberAttack; i++)
        {
            Gizmos.DrawWireSphere(attackHitBoxes[i].position, attackRadiuses[i]);
        }
    }
}
