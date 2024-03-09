using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerCombat : MonoBehaviour
{
    private Controller controller;
    private PlayerController pc;
    private PlayerStat ps;
    private PlayerHealth ph;
    private GameManager_ gm;
    private UiController uc;

    private bool canInput = true;

    [Header("Dash")] 
    private bool isDash = false;
    private bool canDash;
    private bool checkDash = false;
    private float lastDash = -1000.0f;
    private float dashTime = 0.0f;
    private bool inputDash = true;

    [Header("Attack")] 
    private Coroutine attackReset;
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
    [SerializeField] private GameObject skill1Object;
    [SerializeField] private GameObject skill2Object;
    [SerializeField] private float skillCd;
    [SerializeField] private float manaGain;
    private Coroutine skillTimer;
    
    
    [Header("Burst")]
    private bool isBurst = false;
    private bool canBurst = true;
    private float curMana = 0;
    [SerializeField] private float maxMana;
    [SerializeField] private GameObject burstObject;
    [SerializeField] private float burstCd;
    private Coroutine burstTimer;
    [SerializeField] private Vector2 maxOffsetMagnitude;
    [SerializeField] private float timeDisable;
    [SerializeField] private GameObject burstFinalObject;
    
    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        controller = pc.Controller;
        ps = GetComponent<PlayerStat>();
        ph = GetComponent<PlayerHealth>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager_>();
        uc = GameObject.Find("Canvas").GetComponent<UiController>();
        
        statAwake();
        StartCoroutine(statDashUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckValue();
        CheckDash();
        uc.updateNumberDash(ps.DashLeft);
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
        if(canInput == false) return;
        
        if (!isDash && pc.CanDash && ps.DashLeft > 0 && floatToBool(controller.Player.Dash.ReadValue<float>()))
        {
            pc.CanDash = false;
            //Debug.Log("input");
            isDash = true;
            dashTime = ps.DashTime;
            ps.DashLeft -= 1;
            checkDash = true;
            currentAttack = 3;
            SubAttackReset();
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
            //Debug.Log("Skill");
            isSkill = true;
            canSkill = false;
            curMana += manaGain;
            if (curMana > maxMana)
            {
                curMana = maxMana;
            }

            if (Application.isMobilePlatform)
            {
                uc.MobileManaUi.GetComponent<Slider>().maxValue = maxMana; 
                uc.MobileManaUi.GetComponent<Slider>().value = curMana;
            }
            else
            {
                uc.PCManaUi.GetComponent<Slider>().maxValue = maxMana; 
                uc.PCManaUi.GetComponent<Slider>().value = curMana;
            }
            
            Skill();
        }
        
        if (curMana == maxMana && !isDash && !isBurst && !isAttack && canBurst && floatToBool(controller.Player.Burst.ReadValue<float>()))
        {
            isBurst = true;
            curMana = 0;
            if (Application.isMobilePlatform)
            {
                uc.MobileManaUi.GetComponent<Slider>().maxValue = maxMana; 
                uc.MobileManaUi.GetComponent<Slider>().value = curMana;
            }
            else
            {
                uc.PCManaUi.GetComponent<Slider>().maxValue = maxMana; 
                uc.PCManaUi.GetComponent<Slider>().value = curMana;
            }
            //Debug.Log("Burst");
            pc.Rb.velocity = new Vector2(0.0f, 0.0f);
            canInput = false;
            pc.CanInput = false;
            pc.Rb.simulated = false;
            canBurst = false;
            
            Burst();
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
            SubAttackReset();
                
            if (Enemy.Count > 0)
            { 
                GameObject nearestEnemy = Enemy[0];
                
                if (transform.position.x < Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != 1)
                    {
                        pc.subFlipping(0.2f);
                    }
                }
                else if(transform.position.x > Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != -1)
                    {
                        pc.subFlipping(0.2f);
                    }
                }
                
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 

                ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);
                
                GameObject atkInstance = Instantiate(atk_1, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_1, attackPositionTmp, rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;


            }
            else
            {
                Vector3 attackPositionTmp = attackTransform.transform.position + (attackTransform.transform.rotation * offsetAttack); 
                ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);
                
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

            SubAttackReset();
            
            if (Enemy.Count > 0)
            { 
                GameObject nearestEnemy = Enemy[0];
                
                if (transform.position.x < Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != 1)
                    {
                        pc.subFlipping(0.2f);
                    }
                }
                else if(transform.position.x > Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != -1)
                    {
                        pc.subFlipping(0.2f);
                    }
                }
                
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
                ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

                GameObject atkInstance = Instantiate(atk_2, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_1, attackPositionTmp, rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;

            }
            else
            {
                Vector3 attackPositionTmp = attackTransform.transform.position + (attackTransform.transform.rotation * offsetAttack); 
                ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce,pc.Rb.transform.position, 0.0f, 0.0f);

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
                        pc.subFlipping(0.4f);
                    }
                }
                else if(transform.position.x > Enemy[0].transform.position.x)
                {
                    if (pc.FlipDirect != -1)
                    {
                        pc.subFlipping(0.4f);
                    }
                }
                
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
                ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

                GameObject atkInstance = Instantiate(atk_3, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_2, attackPositionTmp, rotation);

                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;
                StartCoroutine(subSlashAttack(0.0f, attackPositionTmp, rotation));

            }
            else
            {
                /*
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
                ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

                GameObject atkInstance = Instantiate(atk_3, attackPositionTmp, rotation);
                GameObject subAtkInstance = Instantiate(sub_atk_2, attackPositionTmp, rotation);
                
                atkInstance.transform.parent = attackTransform.transform;
                subAtkInstance.transform.parent = attackTransform.transform;
                
                StartCoroutine(subSlashAttack(0.0f, attackPositionTmp, rotation));
                */
                
                
                Vector2 direction = pc.MoveSnap.normalized; 
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
                ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

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
        
        ph.Parameters.setStat(ph.CurAttack * 2, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 1.0f, 0.2f, "SM_SlashAttack", 0.2f);
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
    
    public void SubAttackReset()
    {
        if (attackReset != null)
        {
            StopCoroutine(attackReset);
        }
        
        attackReset = StartCoroutine(attackReset_(3));
    }

    IEnumerator attackReset_(float time)
    {
        yield return new WaitForSeconds(time);

        //
        Debug.Log("ResetAttack");
        currentAttack = 1;
    }
    
    // SKILL
    private void Skill()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

        List<GameObject> Enemy = new List<GameObject>();

        foreach (Collider2D collider in DetectObject)
        {
            Enemy.Add(collider.gameObject);
        }

        Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
        
        if (Enemy.Count > 0)
        { 
            GameObject nearestEnemy = Enemy[0];
            
            if (transform.position.x < Enemy[0].transform.position.x)
            {
                if (pc.FlipDirect != 1)
                {
                    pc.subFlipping(0.4f);
                }
            }
            else if(transform.position.x > Enemy[0].transform.position.x)
            {
                if (pc.FlipDirect != -1)
                {
                    pc.subFlipping(0.4f);
                }
            }
            
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
            ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            GameObject skillInstance = Instantiate(skill1Object, attackPositionTmp, rotation);

            skillInstance.transform.parent = attackTransform.transform;
            
            StartCoroutine(subskill());
        }
        else
        {
            Vector2 direction = pc.MoveSnap.normalized; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
            ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            GameObject skillInstance = Instantiate(skill1Object, attackPositionTmp, rotation);

            skillInstance.transform.parent = attackTransform.transform;

            StartCoroutine(subskill());
        }
        
        skillTimer = StartCoroutine(skillReset());
    }
    
    IEnumerator statSkillDelay()
    {
        yield return new WaitForSeconds(ps.SkillDelay);

        canSkill = true;
    }

    IEnumerator skillReset()
    {
        yield return new WaitForSeconds(skillCd);

        canSkill = true;
    }

    IEnumerator subskill()
    {
        yield return new WaitForSeconds(0.15f);
        
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

        List<GameObject> Enemy = new List<GameObject>();

        foreach (Collider2D collider in DetectObject)
        {
            Enemy.Add(collider.gameObject);
        }

        Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
        
        if (Enemy.Count > 0)
        { 
            GameObject nearestEnemy = Enemy[0];
            
            if (transform.position.x < Enemy[0].transform.position.x)
            {
                if (pc.FlipDirect != 1)
                {
                    pc.subFlipping(0.4f);
                }
            }
            else if(transform.position.x > Enemy[0].transform.position.x)
            {
                if (pc.FlipDirect != -1)
                {
                    pc.subFlipping(0.4f);
                }
            }
            
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
            ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            GameObject skillInstance = Instantiate(skill2Object, attackPositionTmp, rotation);

            skillInstance.transform.parent = attackTransform.transform;
            
        }
        else
        {
            Vector2 direction = pc.MoveSnap.normalized; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
            ph.Parameters.setStat(ph.CurAttack, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            GameObject skillInstance = Instantiate(skill2Object, attackPositionTmp, rotation);

            skillInstance.transform.parent = attackTransform.transform;
        }

        currentAttack = 3;
        isSkill = false;
        SubAttackReset();
    }
    
    // BURST
    private void Burst()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

        List<GameObject> Enemy = new List<GameObject>();

        foreach (Collider2D collider in DetectObject)
        {
            Enemy.Add(collider.gameObject);
        }

        Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));

        if (Enemy.Count > 0)
        {
            GameObject nearestEnemy = Enemy[0];
            
            StartCoroutine(SpawnBurstObjects(nearestEnemy.transform.position, 10, 0.05f));
            StartCoroutine(burstActiveEnabled(transform.position, nearestEnemy.transform.position));
        }
        else
        {
            StartCoroutine(SpawnBurstObjects(transform.position, 10, 0.05f));
            StartCoroutine(burstActiveEnabled(transform.position, transform.position));
        }
        
        burstTimer = StartCoroutine(burstReset());
        gm.DisablePlayer(gameObject, timeDisable);
    }
    
    private IEnumerator SpawnBurstObjects(Vector2 nearestEnemy, int count, float delay)
    {
        float rotationAngle = 0f;
        float rotationIncrement = 36f;
    
        for (int i = 0; i < count; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
            rotationAngle += rotationIncrement;
        
            Vector2 randomOffset2D = Random.insideUnitCircle * maxOffsetMagnitude;
            Vector3 randomOffset = new Vector3(randomOffset2D.x, randomOffset2D.y, 0f);
        
            Vector2 burstPositionTmp = nearestEnemy + (Vector2)(rotation * (Vector2)randomOffset);
        
            GameObject burstInstance = Instantiate(burstObject, burstPositionTmp, rotation);

            ph.Parameters.setStat(ph.CurAttack / 2, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            if (i == count - 1)
            {
                StartCoroutine(burstFinalSpawnObject(0.2f));
            }
            yield return new WaitForSeconds(delay);
        }
    }

    
    IEnumerator statBurstDelay()
    {
        yield return new WaitForSeconds(ps.BurstDelay);

        canBurst = true;
    }
    IEnumerator burstReset()
    {
        yield return new WaitForSeconds(skillCd);

        canBurst = true;
    }

    IEnumerator burstActiveEnabled(Vector2 curPosition, Vector2 nearestEnemy)
    {
        yield return new WaitForSeconds(timeDisable);

        canInput = true;
        pc.CanInput = true;
        pc.Rb.simulated = true;
        
        transform.position = nearestEnemy;
        
    }

    IEnumerator burstFinalSpawnObject(float time)
    {
        yield return new WaitForSeconds(time);
        
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

        List<GameObject> Enemy = new List<GameObject>();

        foreach (Collider2D collider in DetectObject)
        {
            Enemy.Add(collider.gameObject);
        }

        Vector2 offsetFinalBurst = new Vector2(0.0f, 10.0f);

        Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
        
        if (Enemy.Count > 0)
        { 
            GameObject nearestEnemy = Enemy[0];
            
            ph.Parameters.setStat(ph.CurAttack * 3, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            Vector2 finalPosition = (Vector2)nearestEnemy.transform.position + offsetFinalBurst;
            GameObject finalInstance = Instantiate(burstFinalObject, finalPosition, Quaternion.identity);
            finalInstance.transform.parent = null;
        }
        else
        {
            ph.Parameters.setStat(ph.CurAttack * 4.5f, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            
            Vector2 finalPosition = (Vector2)transform.position + offsetFinalBurst;
            GameObject finalInstance = Instantiate(burstFinalObject, finalPosition, Quaternion.identity);
            finalInstance.transform.parent = null;
        }
        
        
        ps.DashLeft = ps.DashMultiple + ps.DashBonus;
        StartCoroutine(burstSlash());
        isBurst = false;
    }

    IEnumerator burstSlash()
    {
        yield return new WaitForSeconds(0.4f);
        
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(hitboxAttack.position, attackRadiusSnap, DamgeEnable);

        List<GameObject> Enemy = new List<GameObject>();

        foreach (Collider2D collider in DetectObject)
        {
            Enemy.Add(collider.gameObject);
        }

        Enemy.Sort((a, b) => Vector2.Distance(transform.position, a.transform.position).CompareTo(Vector2.Distance(transform.position, b.transform.position)));
        
        if (Enemy.Count > 0)
        { 
            GameObject nearestEnemy = Enemy[0];
            
            if (transform.position.x < Enemy[0].transform.position.x)
            {
                if (pc.FlipDirect != 1)
                {
                    pc.subFlipping(0.4f);
                }
            }
            else if(transform.position.x > Enemy[0].transform.position.x)
            {
                if (pc.FlipDirect != -1)
                {
                    pc.subFlipping(0.4f);
                }
            }
            
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
            ph.Parameters.setStat(ph.CurAttack * 0.7f, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);
            GameObject slashInstance = Instantiate(slashAttack, attackPositionTmp, rotation);
            
            float angleOffset = 15f;
            
            Quaternion leftRotation = Quaternion.AngleAxis(angle - angleOffset, Vector3.forward);
            Vector3 leftAttackPosition = attackTransform.transform.position + (leftRotation * offsetAttack);
            ph.Parameters.setStat(ph.CurAttack * 0.7f, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);
            GameObject leftSkillInstance = Instantiate(slashAttack, leftAttackPosition, leftRotation);
            
            Quaternion rightRotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);
            Vector3 rightAttackPosition = attackTransform.transform.position + (rightRotation * offsetAttack);
            ph.Parameters.setStat(ph.CurAttack * 0.7f, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);
            GameObject rightSkillInstance = Instantiate(slashAttack, rightAttackPosition, rightRotation);
        }
        else
        {
            Vector2 direction = pc.MoveSnap.normalized; 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Vector3 attackPositionTmp = attackTransform.transform.position + (rotation * offsetAttack); 
            ph.Parameters.setStat(ph.CurAttack * 0.7f, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);

            GameObject skillInstance = Instantiate(slashAttack, attackPositionTmp, rotation);

            float angleOffset = 15f;
            
            Quaternion leftRotation = Quaternion.AngleAxis(angle - angleOffset, Vector3.forward);
            Vector3 leftAttackPosition = attackTransform.transform.position + (leftRotation * offsetAttack);
            ph.Parameters.setStat(ph.CurAttack * 0.7f, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);
            GameObject leftSkillInstance = Instantiate(slashAttack, leftAttackPosition, leftRotation);
            
            Quaternion rightRotation = Quaternion.AngleAxis(angle + angleOffset, Vector3.forward);
            Vector3 rightAttackPosition = attackTransform.transform.position + (rightRotation * offsetAttack);
            ph.Parameters.setStat(ph.CurAttack * 0.7f, ph.DmgBonus, ph.CurDefPierce, pc.Rb.transform.position, 0.0f, 0.0f);
            GameObject rightSkillInstance = Instantiate(slashAttack, rightAttackPosition, rightRotation);
        }
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
