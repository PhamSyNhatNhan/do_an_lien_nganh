using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_4 : MonoBehaviour
{
    private EnemyHealth eh;
        
    private Transform target;

    private NavMeshAgent agent;
    [SerializeField] private bool isEnabledNav = true;
    
    [Header("Attack")]
    private bool canAttack = false;
    [SerializeField] private float findRadius;
    [SerializeField] private float rangeAttackRadius;
    [SerializeField] private LayerMask whatIsDamageEnabled;
    [SerializeField] private Vector2 offsetAttack;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int bulletRow;
    [SerializeField] private int bulletColumn;
    private bool subAttack = true;
    
    [Header("Melee Attack")]
    [SerializeField] private float meleeAttackRadius;
    [SerializeField] private GameObject e4_meleeAttack_ef;
    
    [Header("Weapon")]
    [SerializeField] private GameObject meleeWeapon; 
    [SerializeField] private GameObject rangeWeapon;
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
        RotateWeaponToPlayer(meleeWeapon,-45f);
        RotateWeaponToPlayer(rangeWeapon);
        
        if (canAttack && subAttack)
        {
            Attack();
            canAttack = false;
            subAttack = false;
        }
        else if (isEnabledNav)
        {
            agent.SetDestination(target.position);
            eh.IsMove = true;
            checkFlip();

            if ((transform.position - target.position).magnitude <= rangeAttackRadius)
            {
                canAttack = true;
                agent.enabled = false;
                eh.IsMove = false;
                isEnabledNav = false;
            }
        }
        
        if ((transform.position - target.position).magnitude <= meleeAttackRadius)
        {
            meleeWeapon.SetActive(true);
            rangeWeapon.SetActive(false);
        }
        else
        {
            meleeWeapon.SetActive(false);
            rangeWeapon.SetActive(true);
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
    
    void RotateWeaponToPlayer(GameObject weapon)
    {
        Vector3 direction = target.position - weapon.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    void RotateWeaponToPlayer(GameObject weapon, float offsetAngle)
    {
        Vector3 direction = target.position - weapon.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        angle += offsetAngle;

        weapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void Attack()
    {
    
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(transform.position, findRadius, whatIsDamageEnabled);
        
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

            if ((transform.position - target.position).magnitude <= meleeAttackRadius)
            {
                StartCoroutine(MeleeAttack(nearestEnemy.transform));
            }
            else
            {
                StartCoroutine(SpawnBullets(nearestEnemy.transform));
            }
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
        StartCoroutine(endAttack(3.0f));
    }
    
    IEnumerator MeleeAttack(Transform nearestEnemy)
    {
        yield return new WaitForSeconds(0.5f);
    
        // Lấy góc quay của vũ khí melee
        Vector3 weaponDirection = target.position - meleeWeapon.transform.position;
        float weaponAngle = Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg;
    
        // Sử dụng góc quay để quyết định hướng spawn
        Quaternion spawnRotation = Quaternion.AngleAxis(weaponAngle, Vector3.forward);
    
        eh.Parameters.setStat(eh.CurAttack * 1.5f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
        GameObject e4_mA = Instantiate(e4_meleeAttack_ef, meleeWeapon.transform.position, spawnRotation);
        e4_mA.transform.parent = null;
        e4_mA.GetComponent<Enemy_4_meleeAttack>().onInstantiate(eh.Parameters, 2.3f);
    
        agent.enabled = true;
        isEnabledNav = true;
        StartCoroutine(endAttack(2.0f));
    }
    
    
    IEnumerator endAttack(float time)
    {
        //Debug.Log("Attack Reset");
        
        yield return new WaitForSeconds(time);

        subAttack = true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, findRadius);
        Gizmos.DrawWireSphere(transform.position, rangeAttackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRadius);
    }
}
