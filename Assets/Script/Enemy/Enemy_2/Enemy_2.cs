using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_2 : MonoBehaviour
{
    private EnemyHealth eh;
    
    private Transform target;

    private NavMeshAgent agent;
    [SerializeField] private bool isEnabledNav = true;

    [Header("Attack")] 
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject e2_boom;
    [SerializeField] private float time_to_boom;

    
    
    void Start()
    {
        eh = GetComponent<EnemyHealth>();
        
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        if (isEnabledNav)
        {
            agent.SetDestination(target.position);
            eh.IsMove = true;
            checkFlip();

            if ((transform.position - target.position).magnitude <= explosionRadius)
            {
                agent.enabled = false;
                eh.IsMove = false;
                isEnabledNav = false;
                StartCoroutine(Boom());
            }
        }
        
        
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(time_to_boom);
        
        eh.Parameters.setStat(eh.CurAttack * 3.0f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
        GameObject e2_death = Instantiate(e2_boom, transform);
        e2_death.transform.parent = null;
        e2_death.GetComponent<Enemy2_Boom>().onInstantiate(eh.Parameters, 1.0f);
        Destroy(gameObject);
    }
    private void checkFlip()
    {
        if (eh.EnemyDirect > 0 && agent.velocity.x < 0.0f)
        {
            eh.Flipping();
        }
        else if (eh.EnemyDirect < 0 && agent.velocity.x > 0.0f)
        {
            eh.Flipping();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
