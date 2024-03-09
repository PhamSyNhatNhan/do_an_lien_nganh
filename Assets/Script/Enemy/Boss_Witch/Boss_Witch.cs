using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Witch : MonoBehaviour
{
    private EnemyHealth eh;
    private DamageManager dma;
    private EnemyStat es;
    private CameraShake cs;
    
    [Header("NavMesh")]
    private Transform target;
    private NavMeshAgent agent;
    [SerializeField] private bool isEnabledNav = true;
    
    [Header("Status")]
    private bool isRedWitch = false;
    private bool isDead = false;
    [SerializeField] private GameObject redWitchTransform;
    [SerializeField] private GameObject redWitchAnimatedTransform;
    
    [Header("Dmg")] 
    [SerializeField] private GameObject strikeEffect;
    Dictionary<string, float> dmgCD = new Dictionary<string, float>();
    private bool canDamage = true;

    [Header("Attack")] 
    [SerializeField] private int typeAttack;
    [SerializeField] private LayerMask whatIsDamageEnabled;
    [SerializeField] private bool canAttack = false;
    [SerializeField] private bool attack = false;
    [SerializeField] private float lastTimeAttack = -100.0f;
    private Coroutine coroutineAttack;
    
    [Header("Attack Bullet")]
    [SerializeField] private float attack1Radius;
    [SerializeField] private Transform attack1Transform;
    [SerializeField] private GameObject attack1Object;

    [Header("Attack Bullet 2")] 
    [SerializeField] private float attack2Radius;
    [SerializeField] private GameObject attack2Object;

    [Header("Attack Bullet 3")] 
    [SerializeField] private float attack3Radius;
    [SerializeField] private GameObject attack3Object;
    
    [Header("Attack Bullet 4")] 
    [SerializeField] private float attack4Radius;
    [SerializeField] private GameObject attack4Object;
    
    void Start()
    {
        eh = GetComponent<EnemyHealth>();
        es = GetComponent<EnemyStat>();
        cs = GameObject.Find("Virtual Camera").GetComponent<CameraShake>();
        
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
        dma = GameObject.Find("GameManager").GetComponent<DamageManager>();
        StartCoroutine(endAttack(3.0f));

    }


    void Update()
    {
        if (isEnabledNav)
        {
            agent.SetDestination(target.position);
            eh.IsMove = true;
            checkFlip();
            
            if ((transform.position - target.position).magnitude <= attack1Radius && canAttack)
            {
                agent.enabled = false;
                eh.IsMove = false;
                isEnabledNav = false;
                attack = true;
            }
        }

        if (attack && canAttack)
        {
            attack = false;
            canAttack = false;
            lastTimeAttack = Time.time;
            
            int randomNumber = Random.Range(1, typeAttack + 1);
            
            if (randomNumber == 1)
            {
                WitchBulletShot();
            }
            else if (randomNumber == 2)
            {
                WitchBulletShotLightningBall();
            }
            else if (randomNumber == 3)
            {
                WitchTree();
            }
            else if (randomNumber == 4)
            {
                WitchLightning();
                Debug.Log("witch 4");
                agent.enabled = true;
                eh.IsMove = true;
                if (coroutineAttack != null)
                {
                    StopCoroutine(coroutineAttack);
                    coroutineAttack = StartCoroutine(endAttack(5.0f));
                }
                else
                {
                    coroutineAttack = StartCoroutine(endAttack(5.0f));
                }
            }
        }
    }

    private void WitchLightning()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(transform.position, attack3Radius, whatIsDamageEnabled);

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

            StartCoroutine(subWitchLightning(nearestEnemy.transform, 50));

        }
    }

    IEnumerator subWitchLightning(Transform nearestEnemy, int numberLightning)
    {
        for (int i = 0; i < numberLightning; i++)
        {
            float randomX = Random.Range(-attack4Radius, attack4Radius);
            float randomY = Random.Range(-attack4Radius, attack4Radius);
            Vector2 tmpTransform = new Vector2(transform.position.x + randomX,transform.position.y + randomY);
            
            eh.Parameters.setStat(eh.CurAttack * 2.0f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f, "lightningBall", 0.2f);
            GameObject newBullet1 = Instantiate(attack4Object, tmpTransform, Quaternion.identity);
            newBullet1.GetComponent<EnemyCircleHitboxDefault>().onInstantiate(eh.Parameters, 1.0f);
            
            cs.ShakeCamera(6, 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        
        agent.enabled = true;
        isEnabledNav = true;
        //StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,-45f, 0.3f));
        if (coroutineAttack != null)
        {
            StopCoroutine(coroutineAttack);
            coroutineAttack = StartCoroutine(endAttack(5.0f));
        }
        else
        {
            coroutineAttack = StartCoroutine(endAttack(5.0f));
        }
    }
    
    private void WitchTree()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(transform.position, attack3Radius, whatIsDamageEnabled);

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

            if (!isRedWitch)
            {
                StartCoroutine(subWitchTree(15));
            }
            else if(isRedWitch)
            {
                StartCoroutine(subWitchTree(25));
            }

        }
    }

    IEnumerator subWitchTree(int numberOfTree)
    {
        for (int i = 0; i < numberOfTree; i++)
        {
            float randomX = Random.Range(-attack3Radius, attack3Radius);
            float randomY = Random.Range(-attack3Radius, attack3Radius);
            Vector2 tmpTransform = new Vector2(transform.position.x + randomX,transform.position.y + randomY);
            
            eh.Parameters.setStat(eh.CurAttack * 1.5f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
            GameObject newBullet1 = Instantiate(attack3Object, tmpTransform, Quaternion.identity);
            newBullet1.GetComponent<BossWitchTree>().onInstantiate(eh.Parameters, 1.0f);
            
            yield return new WaitForSeconds(0.05f);
        }
        
        agent.enabled = true;
        isEnabledNav = true;
        //StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,-45f, 0.3f));
        if (coroutineAttack != null)
        {
            StopCoroutine(coroutineAttack);
            coroutineAttack = StartCoroutine(endAttack(5.0f));
        }
        else
        {
            coroutineAttack = StartCoroutine(endAttack(5.0f));
        }
    }

    private void WitchBulletShotLightningBall()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(transform.position, attack2Radius, whatIsDamageEnabled);

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

            if (!isRedWitch)
            {
                StartCoroutine(subWitchBulletShotLightningBall(nearestEnemy.transform, 1));
            }
            else if(isRedWitch)
            {
                StartCoroutine(subWitchBulletShotLightningBall(nearestEnemy.transform, 2));
            }

        }
    }

    IEnumerator subWitchBulletShotLightningBall(Transform nearestEnemy, int circle)
    {
        float angleOffset = 36f;

        Vector2 offsetAttack = new Vector2(0.5f, 0.5f);
        Vector3 direction = nearestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        for (int i = 0; i < circle; i++)
        { 
            for (int j = 0; j < 10; j++)
            {
                Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward);;
                angle += angleOffset;
                
                Vector3 leftAttackPosition = transform.position + (bulletRotation * offsetAttack);
                eh.Parameters.setStat(eh.CurAttack * 1.5f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f, "lightningBall", 0.2f);
                GameObject newBullet1 = Instantiate(attack2Object, leftAttackPosition, bulletRotation);
                newBullet1.GetComponent<EnemyCircleDefaultHitBox2>().onInstantiate(eh.Parameters, 1.0f);
                
                yield return new WaitForSeconds(0.15f);
            }
            
            yield return new WaitForSeconds(0.7f);
        }
        
        agent.enabled = true;
        isEnabledNav = true;
        //StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,-45f, 0.3f));
        if (coroutineAttack != null)
        {
            StopCoroutine(coroutineAttack);
            coroutineAttack = StartCoroutine(endAttack(5.0f));
        }
        else
        {
            coroutineAttack = StartCoroutine(endAttack(5.0f));
        }
    }
    
    private void WitchBulletShot()
    {
        Collider2D[] DetectObject = Physics2D.OverlapCircleAll(transform.position, attack1Radius, whatIsDamageEnabled);

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

            if (!isRedWitch)
            {
                StartCoroutine(subWitchBulletShot(nearestEnemy.transform, 3));
            }
            else if(isRedWitch)
            {
                StartCoroutine(subWitchBulletShot(nearestEnemy.transform, 5));
            }

        }
    }

    IEnumerator subWitchBulletShot(Transform nearestEnemy, int numberBullets)
    {
        Vector2 offsetAttack = new Vector2(0.1f, 0.1f);
        
        for (int i = 0; i < numberBullets; i++)
        { 
            Vector3 direction = nearestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Vector3 AttackPosition = transform.position + (rotation * offsetAttack);
            eh.Parameters.setStat(eh.CurAttack * 0.7f, eh.DmgBonus, eh.CurDefPierce, eh.Rb.transform.position, 0.0f, 0.0f);
            GameObject newBullet1 = Instantiate(attack1Object, AttackPosition, rotation);
            newBullet1.GetComponent<BossWitchBullet1>().onInstantiate(eh.Parameters, 2.0f);
            
            yield return new WaitForSeconds(0.75f);
        }
        
        agent.enabled = true;
        isEnabledNav = true;
        eh.IsMove = true;
        //StartCoroutine(RotateOverTime(gameObject.transform.GetChild(0).gameObject ,-45f, 0.2f));
        if (coroutineAttack != null)
        {
            StopCoroutine(coroutineAttack);
            coroutineAttack = StartCoroutine(endAttack(5.0f));
        }
        else
        {
            coroutineAttack = StartCoroutine(endAttack(5.0f));
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
    
    IEnumerator endAttack(float time)
    {
        //Debug.Log("Attack Reset");
        
        yield return new WaitForSeconds(time);

        canAttack = true;
    }
    
    public void Damage(DamageParameters parameters)
    {
        if (!canDamage) return;
        
        float baseDmg = parameters.BaseDmg;
        float dmgBonus = parameters.DmgBonus;
        float defPierce = parameters.DefPierce;
        Vector2 playerDirect = parameters.PlayerDirect;
        
        if (!dmgCD.ContainsKey(parameters.DmgName))
        {
            eh.CurHp -= dma.finalDamage(baseDmg, dmgBonus, defPierce, eh.CurDef, eh.CurDmgResistance);
            Instantiate(strikeEffect, transform);

            if (parameters.CdDamage != 0.0f)
            {
                dmgCD.Add(parameters.DmgName, parameters.CdDamage);
                StartCoroutine(removeCdDamage(parameters.DmgName, parameters.CdDamage));
            }
            
            if (playerDirect.x <= transform.position.x && eh.EnemyDirect == 1)
            {
                eh.Flipping();
            }

            if (playerDirect.x >= transform.position.x && eh.EnemyDirect == -1)
            {
                eh.Flipping();
            }
        
            if (eh.CurHp <= 0 && !isRedWitch)
            {
                //Debug.Log("Red Witch");
                canDamage = false;
                es.HpBase *= 1.5f;
                eh.CurHp = eh.Hp;
                isRedWitch = true;
                es.AttackBase *= 1.5f;
                isDead = true;
                
                
                canAttack = false;
                agent.enabled = false;
                eh.IsMove = false;
                isEnabledNav = false;
                GameObject rwObject1 = Instantiate(redWitchTransform, transform);

                eh.Amt.SetBool("isDead", isDead);
                eh.Amt.SetBool("isRedWitch", isRedWitch);
                StartCoroutine(transformRedWitchAnimation());
                StartCoroutine(transformRedWitch(rwObject1));
            }
            else if (eh.CurHp <= 0 && isRedWitch)
            {
                Debug.Log("Red Witch Death");
                
                GameObject death = Instantiate(eh.DeathObject, transform);
                death.transform.parent = null;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator transformRedWitchAnimation()
    {
        yield return new WaitForSeconds(2.7f);

        Instantiate(redWitchAnimatedTransform, transform);
    }

    IEnumerator transformRedWitch(GameObject gm1)
    {
        yield return new WaitForSeconds(3.5f);
        
        Destroy(gm1);
        canDamage = true;
        canAttack = true;
        agent.enabled = true;
        eh.IsMove = true;
        isEnabledNav = true;
        
        isDead = false;
        typeAttack += 1;
        
        eh.Amt.SetBool("isDead", isDead);
        eh.Amt.SetBool("isRedWitch", isRedWitch);
    }
    
    IEnumerator removeCdDamage(string name, float time)
    {
        yield return new WaitForSeconds(time);
        
        dmgCD.Remove(name);
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
    
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attack1Radius);
        Gizmos.DrawWireSphere(transform.position, attack2Radius);
        Gizmos.DrawWireSphere(transform.position, attack3Radius);
        Gizmos.DrawWireSphere(transform.position, attack4Radius);
    }
}
