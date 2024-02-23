using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerCombat : MonoBehaviour
{
    private Controller controller;
    private PlayerController pc;
    private PlayerStat ps;

    [Header("Dash")] 
    private bool isDash = false;
    private bool canDash;
    private bool checkDash = false;
    private float lastDash = -1000.0f;
    private float dashTime = 0.0f;
    private bool inputDash = true;

    [Header("Attack")]
    private bool isAttack = false;
    private bool canAttack = true;
    private int numberAttack;
    private int currentAttack = 1;
    [SerializeField] private GameObject atk_1;
    [SerializeField] private GameObject atk_2;
    [SerializeField] private GameObject atk_3;
    [SerializeField] private GameObject sub_atk_1;
    [SerializeField] private GameObject sub_atk_2;
    [SerializeField] private GameObject attackTransform;
    [SerializeField] private GameObject slashAttack;
    
    [SerializeField] private Transform hitboxAttack;
    [SerializeField] private LayerMask DamgeEnable;
    [SerializeField] private float attackRadiusSnap;
    [SerializeField] private Vector3 offsetAttack;
    
    

    [Header("Skill")]
    private bool isSkill = false;
    private bool canSkill = true;
    
    [Header("Burst")]
    private bool isBurst = false;
    private bool canBurst = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        controller = pc.Controller;
        ps = GetComponent<PlayerStat>();
        
        statAwake();
        StartCoroutine(statDashUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckValue();
        CheckDash();
    }

    private void statAwake()
    {
        ps.DashLeft = ps.DashMultiple + ps.DashBonus;
        numberAttack = ps.AttackMultiple;
    }
    
    
    IEnumerator statDashUpdate()
    {
        while (true)
        {
            if (ps.DashLeft < ps.DashMultiple + ps.DashBonus)
            {
                ps.DashLeft += 1;
                Debug.Log("Dash + 1");
            }
            
            yield return new WaitForSeconds(ps.DashCd - ps.DashCdDecrease);
        }
    }
    
    private void CheckInput()
    {
        if (!isDash && pc.CanDash && ps.DashLeft > 0 && floatToBool(controller.Player.Dash.ReadValue<float>()))
        {
            pc.CanDash = false;
            //Debug.Log("input");
            isDash = true;
            dashTime = ps.DashTime;
            ps.DashLeft -= 1;
            checkDash = true;
        }

        if (!isDash && !isAttack && canAttack && floatToBool(controller.Player.Attack.ReadValue<float>()))
        {
            //Debug.Log("Attack " + currentAttack);
            isAttack = true;
            canAttack = false;
            //Debug.Log(isAttack + " " + canAttack);

            if (currentAttack == 1 || currentAttack == 2)
            {
                StartCoroutine(statAttackDelay(0.3f));  
            }
            else
            {
                StartCoroutine(statAttackDelay(0.3f)); 
            }
            
            Attack();
        }
        
        if (!isDash && !isSkill && !isAttack && canSkill && floatToBool(controller.Player.Skill.ReadValue<float>()))
        {
            Debug.Log("Skill");
            canSkill = false;
            Skill();
            StartCoroutine(statSkillDelay());
        }
        
        if (!isDash && !isBurst && !isAttack && canBurst && floatToBool(controller.Player.Burst.ReadValue<float>()))
        {
            Debug.Log("Burst");
            canBurst = false;
            Burst();
            StartCoroutine(statBurstDelay());
        }
    }

    private void CheckValue()
    {
        canDash = pc.CanDash;
    }

    // DASH
    private void CheckDash()
    {
        if (isDash && dashTime > 0.0f)
        {
            if (checkDash)
            {
                lastDash = Time.time;
                pc.IsDash = true;
                pc.CanFlip = false;
                pc.CanMove = false;
                checkDash = false;
            }
            
            dashTime = -(Time.time - (ps.DashTime + lastDash));
            if (dashTime <= 0.0f)
            {
                isDash = false;
                pc.IsDash = false;
                pc.CanFlip = true;
                pc.CanMove = true;
                pc.CanDash = false;
                StartCoroutine(statDashDelay());
            }
            
            pc.Rb.velocity = new Vector2(pc.MoveSnap.x * ps.DashSpeed, pc.MoveSnap.y * ps.DashSpeed);
            //Debug.Log("Time " + Time.time + "\nDashTime" + ps.DashTime + "\nlastDash" + lastDash);
            //Debug.Log("DashTimeLeft: " + dashTime);
        }
    }
    
    IEnumerator statDashDelay()
    {
        yield return new WaitForSeconds(ps.DashDelay);

        pc.CanDash = true;
    }
    
    // ATTACK
    private void Attack()
    {
        if (currentAttack == 1)
        {
            currentAttack += 1;
            if (currentAttack > numberAttack)
            {
                currentAttack = 1;
            }
            
            Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

            List<GameObject> Enemy = new List<GameObject>();

            foreach (Collider2D collider in DetectObject)
            {
                Enemy.Add(collider.gameObject);
            }

            Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
            //Debug.Log("Enemy find: " + Enemy.Count);
            
            if (Enemy.Count > 0)
            { 
                GameObject nearestEnemy = Enemy[0];
                
                if (transform.position.x < Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != 1)
                    {
                        pc.subFlipping();
                    }
                }
                else if(transform.position.x > Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != -1)
                    {
                        pc.subFlipping();
                    }
                }
                
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 

                GameObject atkInstance = Instantiate(atk_1, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_1, attackPositionTmp, rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;


            }
            else
            {
                Vector3 attackPositionTmp = attackTransform.transform.position + (attackTransform.transform.rotation * offsetAttack); 
                
                GameObject atkInstance = Instantiate(atk_1, attackPositionTmp, attackTransform.transform.rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_1, attackPositionTmp, attackTransform.transform.rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;
            }
            
        }
        else if (currentAttack == 2)
        {
            currentAttack += 1;
            if (currentAttack > numberAttack)
            {
                currentAttack = 1;
            }
            
            Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

            List<GameObject> Enemy = new List<GameObject>();

            foreach (Collider2D collider in DetectObject)
            {
                Enemy.Add(collider.gameObject);
            }

            Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
            //Debug.Log("Enemy find: " + Enemy.Count);
            
            if (Enemy.Count > 0)
            { 
                GameObject nearestEnemy = Enemy[0];
                
                if (transform.position.x < Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != 1)
                    {
                        pc.subFlipping();
                    }
                }
                else if(transform.position.x > Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != -1)
                    {
                        pc.subFlipping();
                    }
                }
                
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
                
                GameObject atkInstance = Instantiate(atk_2, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_1, attackPositionTmp, rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;

            }
            else
            {
                Vector3 attackPositionTmp = attackTransform.transform.position + (attackTransform.transform.rotation * offsetAttack); 
                
                GameObject atkInstance = Instantiate(atk_2, attackPositionTmp, attackTransform.transform.rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_1, attackPositionTmp, attackTransform.transform.rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;
            }
            
        }
        else if (currentAttack == 3)
        {
            currentAttack += 1;
            if (currentAttack > numberAttack)
            {
                currentAttack = 1;
            }
            
            Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

            List<GameObject> Enemy = new List<GameObject>();

            foreach (Collider2D collider in DetectObject)
            {
                Enemy.Add(collider.gameObject);
            }

            Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
            //Debug.Log("Enemy find: " + Enemy.Count);
            
            if (Enemy.Count > 0)
            { 
                GameObject nearestEnemy = Enemy[0];
                
                if (transform.position.x < Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != 1)
                    {
                        pc.subFlipping();
                    }
                }
                else if(transform.position.x > Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != -1)
                    {
                        pc.subFlipping();
                    }
                }
                
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
                
                GameObject atkInstance = Instantiate(atk_3, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_2, attackPositionTmp, rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;
                StartCoroutine(subSlashAttack(0.0f, attackPositionTmp, rotation));

            }
            else
            {
                Vector2 playerDirection; 
                
                if (pc.FlipDirect == 1)
                {
                    playerDirection = Vector2.right; 
                }
                else
                {
                    playerDirection = Vector2.left; 
                } 
                
                float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
                
                GameObject atkInstance = Instantiate(atk_3, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_2, attackPositionTmp, rotation);
                
                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;
                
                StartCoroutine(subSlashAttack(0.0f, attackPositionTmp, rotation));
            }
        }
    }

    IEnumerator subSlashAttack(float time, Vector3 vec, Quaternion rotation)
    {
        yield return new WaitForSeconds(time);
        
        GameObject instance = (GameObject)Instantiate(slashAttack, vec, rotation);
    }
    
    IEnumerator statAttackDelay()
    {
        yield return new WaitForSeconds(ps.AttackDelay);

        canAttack = true;
    }
    IEnumerator statAttackDelay(float time)
    {
        yield return new WaitForSeconds(time);

        isAttack = false;
        canAttack = true;
    }
    
    // SKILL
    private void Skill()
    {
        
    }
    
    IEnumerator statSkillDelay()
    {
        yield return new WaitForSeconds(ps.SkillDelay);

        canSkill = true;
    }
    
    // BURST
    private void Burst()
    {
        
    }
    
    IEnumerator statBurstDelay()
    {
        yield return new WaitForSeconds(ps.BurstDelay);

        canBurst = true;
    }

    private bool floatToBool(float value)
    {
        if (value == 0.0f)
        {
            return false;
        }
        return true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackTransform.transform.position, attackRadiusSnap);
    }
}
