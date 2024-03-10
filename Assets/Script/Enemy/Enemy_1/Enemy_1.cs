using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_1 : MonoBehaviour
{
    private EnemyHealth eh;
    
    private Transform target;

    private NavMeshAgent agent;
    [SerializeField] private bool isEnabledNav = true;
    
    [Header("Attack")]
    private bool canAttack = false;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackRadius1;
    [SerializeField] private float attackRadius2;
    [SerializeField] private LayerMask whatIsDamageEnabled;
    [SerializeField] private Vector2 offsetAttack;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int bulletRow;
    [SerializeField] private int bulletColumn;
    private bool subAttack = true;
    
    
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
        if (canAttack && subAttack)
        {
            Attack();
            canAttack = false;
            subAttack = false;
        }
        else if (isEnabledNav && target != null)
        {
            agent.SetDestination(target.position);
            eh.IsMove = true;
            checkFlip();

            if ((transform.position - target.position).magnitude <= attackRadius2)
            {
                canAttack = true;
                agent.enabled = false;
                eh.IsMove = false;
                isEnabledNav = false;
            }
        }
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

    private void Attack()
    {
        //Debug.Log("Enemy Attack");
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(transform.position, attackRadius, whatIsDamageEnabled);

        List<GameObject> Player = new List<GameObject>();

        foreach (Collider2D collider in DetectObject)
        {
            Player.Add(collider.gameObject);
        }

        Player.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
        
        if (Player.Count > 0)
        { 
            GameObject nearestEnemy = Player[0];
            
            if (transform.position.x < Player[0].transform.position.x)
            {
                if (eh.EnemyDirect != 1)
                {
                    eh.subFlipping(0.4f);
                }
            }
            else if(transform.position.x > Player[0].transform.position.x)
            {
                if (eh.EnemyDirect != -1)
                {
                    eh.subFlipping(0.4f);
                }
            }
            
            StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,45f, 0.3f));
            
            
            if ((transform.position - target.position).magnitude <= attackRadius1)
            {
                StartCoroutine(SpawnBullets(nearestEnemy.transform, bulletRow + 2, bulletColumn - 5));
            }
            else
            {
                StartCoroutine(SpawnBullets(nearestEnemy.transform));
            }
        }
    }
    
    IEnumerator RotateOverTime(GameObject objectRotate, float targetAngle, float duration)
    {
        float elapsedTime = 0;
        Quaternion startRotation = objectRotate.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(
            objectRotate.transform.rotation.eulerAngles.x,
            objectRotate.transform.rotation.eulerAngles.y,
            objectRotate.transform.rotation.eulerAngles.z + targetAngle * -1.0f 
        );
    
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            objectRotate.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    IEnumerator SpawnBullets(Transform nearestEnemy)
    {
        float angleOffset = 15f;

        Vector3 direction = nearestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        for (int i = 0; i < bulletColumn; i++)
        { 
            for (int j = 0; j < bulletRow; j++)
            {
                Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);;
                if (j > 0 && j % 2 == 1)
                {
                    bulletRotation = Quaternion.AngleAxis(angle - angleOffset * (int)((j + 1) / 2), Vector3.forward);
                }
                else if (j > 0 && j % 2 == 0)
                {
                    bulletRotation = Quaternion.AngleAxis(angle + angleOffset * (int)((j + 1) / 2), Vector3.forward);
                }
                
                Vector3 leftAttackPosition = transform.position + (bulletRotation * offsetAttack);
                eh.Parameters.setStat(eh.CurAttack * 0.7f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
                GameObject newBullet1 = Instantiate(bullet, leftAttackPosition, bulletRotation);
                newBullet1.GetComponent<CircleHitboxDefaultTime>().onInstantiate(eh.Parameters, 0.5f);
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        
        agent.enabled = true;
        isEnabledNav = true;
        StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,-45f, 0.3f));
        StartCoroutine(endAttack(4.0f));
    }

    IEnumerator SpawnBullets(Transform nearestEnemy, int bulletRow_, int bulletColumn_)
    {
        float angleOffset = 15f;

        Vector3 direction = nearestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        for (int i = 0; i < bulletColumn_; i++)
        { 
            for (int j = 0; j < bulletRow_; j++)
            {
                Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);;
                if (j > 0 && j % 2 == 1)
                {
                    bulletRotation = Quaternion.AngleAxis(angle - angleOffset * (int)((j + 1) / 2), Vector3.forward);
                }
                else if (j > 0 && j % 2 == 0)
                {
                    bulletRotation = Quaternion.AngleAxis(angle + angleOffset * (int)((j + 1) / 2), Vector3.forward);
                }
                
                Vector3 leftAttackPosition = transform.position + (bulletRotation * offsetAttack);
                eh.Parameters.setStat(eh.CurAttack * 0.7f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
                GameObject newBullet1 = Instantiate(bullet, leftAttackPosition, bulletRotation);
                newBullet1.GetComponent<CircleHitboxDefaultTime>().onInstantiate(eh.Parameters, 0.5f);
            }
            
            yield return new WaitForSeconds(0.2f);
        }
        
        agent.enabled = true;
        isEnabledNav = true;
        StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,-45f, 0.3f));
        StartCoroutine(endAttack(4.0f));
    }

    
    IEnumerator endAttack(float time)
    {
        //Debug.Log("Attack Reset");
        
        yield return new WaitForSeconds(time);

        subAttack = true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius1);
        Gizmos.DrawWireSphere(transform.position, attackRadius2);
    }
}
