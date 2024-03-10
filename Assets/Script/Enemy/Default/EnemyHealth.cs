using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
     private float attack = 0.0f;
    [SerializeField] private float curAttack = 0.0f;
    private float dmgBonus = 0.0f;
    [SerializeField] private float curDamgBonus = 0.0f;
    private float defPierce = 0.0f;
    [SerializeField] private float curDefPierce = 0.0f;
    
    private float hp = 0.0f;
    [SerializeField] private float curHp = 0.0f;
    private float def = 0.0f; 
    [SerializeField] private float curDef = 0.0f;
    private float dmgResistance = 0.0f;
    [SerializeField] private float curDmgResistance = 0.0f;

    [Header("KnockBack")]
    [SerializeField] private bool isKnockback = false;
    private float knockBackTime = 0.0f;

    [Header("Dmg")] 
    [SerializeField] private GameObject strikeEffect;
    Dictionary<string, float> dmgCD = new Dictionary<string, float>();

    [Header("Controller")]
    [SerializeField] private int enemyDirect = 1;
    private bool canFlip = true;
    [SerializeField] private bool isMove = false;
    private Coroutine flipCoroutine; 
    
    [Header("Attack")]
    private DamageParameters parameters;
    [SerializeField] private bool canDamage = true;
    [SerializeField] private bool canKnockBack = true;

    [SerializeField] private GameObject deathObject;

    private Rigidbody2D rb;
    private Animator amt;
    private EnemyStat es;
    private DamageManager dma;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        amt = GetComponent<Animator>();
        es = GetComponent<EnemyStat>();
        dma = GameObject.Find("GameManager").GetComponent<DamageManager>();
        parameters = new DamageParameters();

        getStats();
    }

    void Update()
    {
        checkKnockBack();
        animationController();
    }

    private void getStats()
    {
        attack = es.AttackBase + es.AttackBase * es.AttackBonus / 100;
        dmgBonus = es.DmgBonus;
        defPierce = es.DefPierceBase + es.DefPierceBase * es.DefPierceBonus / 100;
        
        hp = es.HpBase + es.HpBase * es.HpBonus / 100;
        def = es.DefBase + es.DefBase * es.DefBonus / 100;
        dmgResistance = es.DmgResistanceBase + es.DmgResistanceBase * es.DmgResistanceBonus / 100;

        
        //
        curAttack = attack;
        curDamgBonus = dmgBonus;
        curDefPierce = defPierce;

        curHp = hp;
        curDef = def;
        curDmgResistance = dmgResistance;
    }

    public void ChangeStat()
    {
        attack = es.AttackBase + es.AttackBase * es.AttackBonus / 100;
        dmgBonus = es.DmgBonus;
        defPierce = es.DefPierceBase + es.DefPierceBase * es.DefPierceBonus / 100;

        float tmpHp = hp - curHp;
        hp = es.HpBase + es.HpBase * es.HpBonus / 100;
        def = es.DefBase + es.DefBase * es.DefBonus / 100;
        dmgResistance = es.DmgResistanceBase + es.DmgResistanceBase * es.DmgResistanceBonus / 100;
        
        curAttack = attack;
        curDamgBonus = dmgBonus;
        curDefPierce = defPierce;

        curHp = hp - tmpHp;
        curDef = def;
        curDmgResistance = dmgResistance;
    }

    public void Damage(DamageParameters parameters)
    {
        if (!canDamage) return;
        
        float baseDmg = parameters.BaseDmg;
        float dmgBonus = parameters.DmgBonus;
        float defPierce = parameters.DefPierce;
        Vector2 playerDirect = parameters.PlayerDirect;
        float knockBack = parameters.KnockBack;
        float KnockBackTime = parameters.KnockBackTime1;


        if (!dmgCD.ContainsKey(parameters.DmgName))
        {
            curHp -= dma.finalDamage(baseDmg, dmgBonus, defPierce, curDef, curDmgResistance);
            Instantiate(strikeEffect, transform);

            if (parameters.CdDamage != 0.0f)
            {
                dmgCD.Add(parameters.DmgName, parameters.CdDamage);
                StartCoroutine(removeCdDamage(parameters.DmgName, parameters.CdDamage));
            }
            
            if (playerDirect.x <= transform.position.x && enemyDirect == 1)
            {
                Flipping();
            }

            if (playerDirect.x >= transform.position.x && enemyDirect == -1)
            {
                Flipping();
            }
        
            if (curHp <= 0)
            {
                GameObject death = Instantiate(deathObject, transform);
                death.transform.parent = null;
                Destroy(gameObject);
            }
        
            if(!canKnockBack) return;
            KnockBack(playerDirect, knockBack, KnockBackTime);
        }
    }

    IEnumerator removeCdDamage(string name, float time)
    {
        yield return new WaitForSeconds(time);
        
        dmgCD.Remove(name);
    }

    private void KnockBack(Vector2 playerDirect, float knockBack, float KnockBackTime)
    {
        if(knockBack == 0.0f) return;
        
        isKnockback = true;
        knockBackTime = KnockBackTime;
        canFlip = false;
        
        Vector2 knockbackDirection = ((Vector2)transform.position - playerDirect).normalized;
        
        rb.velocity = new Vector2(knockbackDirection.x * knockBack, knockbackDirection.y * knockBack);
    }

    private void checkKnockBack()
    {
        if (isKnockback && knockBackTime > 0.0f)
        {
            knockBackTime -= Time.deltaTime;
            if (knockBackTime <= 0.0f)
            {
                isKnockback = false;
                canFlip = true;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void animationController()
    {
        amt.SetBool("isKnockback", isKnockback);
        amt.SetBool("isMove", isMove);
    }
    
    public void Flipping()
    {
        if (canFlip)
        {
            enemyDirect *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    
    public void subFlipping(float time)
    {
        Flipping();
        canFlip = false;
        
        if (flipCoroutine != null)
        {
            StopCoroutine(flipCoroutine);
        }
        
        flipCoroutine = StartCoroutine(flipReset(time));
    }

    IEnumerator flipReset(float time)
    {
        yield return new WaitForSeconds(time);

        canFlip = true;
    }

    public float Attack
    {
        get => attack;
        set => attack = value;
    }

    public float CurAttack
    {
        get => curAttack;
        set => curAttack = value;
    }

    public float DmgBonus
    {
        get => dmgBonus;
        set => dmgBonus = value;
    }

    public float CurDamgBonus
    {
        get => curDamgBonus;
        set => curDamgBonus = value;
    }

    public float CurDefPierce
    {
        get => curDefPierce;
        set => curDefPierce = value;
    }

    public float CurHp
    {
        get => curHp;
        set => curHp = value;
    }

    public float CurDef
    {
        get => curDef;
        set => curDef = value;
    }

    public float CurDmgResistance
    {
        get => curDmgResistance;
        set => curDmgResistance = value;
    }

    public bool IsKnockback
    {
        get => isKnockback;
        set => isKnockback = value;
    }

    public int EnemyDirect
    {
        get => enemyDirect;
        set => enemyDirect = value;
    }

    public bool CanFlip
    {
        get => canFlip;
        set => canFlip = value;
    }

    public Rigidbody2D Rb
    {
        get => rb;
        set => rb = value;
    }

    public Animator Amt
    {
        get => amt;
        set => amt = value;
    }

    public bool IsMove
    {
        get => isMove;
        set => isMove = value;
    }

    public DamageParameters Parameters
    {
        get => parameters;
        set => parameters = value;
    }

    public GameObject DeathObject
    {
        get => deathObject;
        set => deathObject = value;
    }

    public float Hp
    {
        get => hp;
        set => hp = value;
    }
}
