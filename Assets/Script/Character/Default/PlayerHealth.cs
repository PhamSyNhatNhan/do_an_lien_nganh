using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class PlayerHealth : MonoBehaviour
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
    private float KnockbackStartTime;
    private bool isKnockback = false;
    private float knockBackTime = 0.0f;
    
    [Header("Attack")]
    private DamageParameters parameters;
    
    [Header("Dmg")] 
    [SerializeField] private GameObject strikeEffect;
    Dictionary<string, float> dmgCD = new Dictionary<string, float>();
    private bool canDamage = true;
    

    private PlayerStat ps;
    private PlayerController pc;
    private UiController uc;
    private DamageManager dma;
    
    
    [SerializeField] private bool Godmode = false;
    [SerializeField] private GameObject godModeElf;

    public void enterGodmode()
    {
        Godmode = !Godmode;
        godModeElf.SetActive(Godmode);
    }
    
    void Start()
    {
        ps = GetComponent<PlayerStat>();
        pc = GetComponent<PlayerController>();
        uc = GameObject.Find("Canvas").GetComponent<UiController>();
        dma = GameObject.Find("GameManager").GetComponent<DamageManager>();
        parameters = new DamageParameters();
        getStats();
        Debug.Log(hp + ", " + curHp);
    }

    void Update()
    {
        
    }

    public void getStats()
    {
        attack = ps.AttackBase + ps.AttackBase * ps.AttackBonus / 100;
        dmgBonus = ps.DmgBonus;
        defPierce = ps.DefPierceBase + ps.DefPierceBase * ps.DefPierceBonus / 100;

        hp = ps.HpBase + ps.HpBase * ps.HpBonus / 100;
        def = ps.DefBase + ps.DefBase * ps.DefBonus / 100;
        dmgResistance = ps.DmgResistanceBase + ps.DmgResistanceBase * ps.DmgResistanceBonus / 100;
        
        
        //
        curAttack = attack;
        curDamgBonus = dmgBonus;
        curDefPierce = defPierce;

        curHp = hp;
        if (Application.isMobilePlatform)
        {
            uc.MobileHpUi.GetComponent<Slider>().maxValue = hp;
            uc.MobileHpUi.GetComponent<Slider>().value = curHp;
        }
        else
        {
            uc.PCHpUi.GetComponent<Slider>().maxValue = hp; 
            uc.PCHpUi.GetComponent<Slider>().value = curHp;
        }
        curDef = def;
        curDmgResistance = dmgResistance;
    }

    public void ChangeStat()
    {
        attack = ps.AttackBase + ps.AttackBase * ps.AttackBonus / 100;
        dmgBonus = ps.DmgBonus;
        defPierce = ps.DefPierceBase + ps.DefPierceBase * ps.DefPierceBonus / 100;

        float tmpHp = hp - curHp;
        hp = ps.HpBase + ps.HpBase * ps.HpBonus / 100;
        def = ps.DefBase + ps.DefBase * ps.DefBonus / 100;
        dmgResistance = ps.DmgResistanceBase + ps.DmgResistanceBase * ps.DmgResistanceBonus / 100;
        
        
        //
        curAttack = attack;
        curDamgBonus = dmgBonus;
        curDefPierce = defPierce;

        curHp = hp - tmpHp;
        if (Application.isMobilePlatform)
        {
            uc.MobileHpUi.GetComponent<Slider>().maxValue = hp;
            uc.MobileHpUi.GetComponent<Slider>().value = curHp;
        }
        else
        {
            uc.PCHpUi.GetComponent<Slider>().maxValue = hp; 
            uc.PCHpUi.GetComponent<Slider>().value = curHp;
        }
        
        curDef = def;
        curDmgResistance = dmgResistance;
    }

    public void Damage(DamageParameters parameters)
    {
        if(!canDamage) return;
        
        float baseDmg = parameters.BaseDmg;
        float dmgBonus = parameters.DmgBonus;
        float defPierce = parameters.DefPierce;
        Vector2 playerDirect = parameters.PlayerDirect;
        float knockBack = parameters.KnockBack;
        float KnockBackTime = parameters.KnockBackTime1;


        if (!dmgCD.ContainsKey(parameters.DmgName))
        {
            curHp -= dma.finalDamage(baseDmg, dmgBonus, defPierce, curDef, curDmgResistance);
            if (Application.isMobilePlatform)
            {
                uc.MobileHpUi.GetComponent<Slider>().maxValue = hp;
                uc.MobileHpUi.GetComponent<Slider>().value = curHp;
            }
            else
            {
                uc.PCHpUi.GetComponent<Slider>().maxValue = hp;
                uc.PCHpUi.GetComponent<Slider>().value = curHp;
                Debug.Log(hp + ", " + curHp);
            }
            Instantiate(strikeEffect, transform);

            if (parameters.CdDamage != 0.0f)
            {
                dmgCD.Add(parameters.DmgName, parameters.CdDamage);
                StartCoroutine(removeCdDamage(parameters.DmgName, parameters.CdDamage));
            }
            
            if (playerDirect.x <= transform.position.x && pc.FlipDirect == 1)
            {
                pc.subFlipping();
            }

            if (playerDirect.x >= transform.position.x && pc.FlipDirect == -1)
            {
                pc.subFlipping();
            }

            if (curHp <= 0 && !Godmode)
            {
                Time.timeScale = 0.0f;
                uc.WinMenu.SetActive(true);
                Destroy(gameObject);
            }
            else if(curHp <= 0 && Godmode)
            {
                curHp = 1.0f;
            }
        
            //KnockBack(playerDirect, knockBack, KnockBackTime);
        }
    }
    
    
    IEnumerator removeCdDamage(string name, float time)
    {
        yield return new WaitForSeconds(time);
        
        dmgCD.Remove(name);
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

    public float DefPierce
    {
        get => defPierce;
        set => defPierce = value;
    }

    public float CurDefPierce
    {
        get => curDefPierce;
        set => curDefPierce = value;
    }

    public float Hp
    {
        get => hp;
        set
        { 
            hp = value;
            if (Application.isMobilePlatform)
            {
                uc.MobileHpUi.GetComponent<Slider>().maxValue = hp;
                uc.MobileHpUi.GetComponent<Slider>().value = curHp;
            }
            else
            {
                uc.PCHpUi.GetComponent<Slider>().maxValue = hp; 
                uc.PCHpUi.GetComponent<Slider>().value = curHp;
            }
        } 
    }

    public float CurHp
    {
        get => curHp;
        set
        {
            curHp = value;
            if (Application.isMobilePlatform)
            {
                uc.MobileHpUi.GetComponent<Slider>().maxValue = hp;
                uc.MobileHpUi.GetComponent<Slider>().value = curHp;
            }
            else
            {
                uc.PCHpUi.GetComponent<Slider>().maxValue = hp; 
                uc.PCHpUi.GetComponent<Slider>().value = curHp;
            }
        } 
    }

    public float Def
    {
        get => def;
        set => def = value;
    }

    public float CurDef
    {
        get => curDef;
        set => curDef = value;
    }

    public float DmgResistance
    {
        get => dmgResistance;
        set => dmgResistance = value;
    }

    public float CurDmgResistance
    {
        get => curDmgResistance;
        set => curDmgResistance = value;
    }

    public float KnockbackStartTime1
    {
        get => KnockbackStartTime;
        set => KnockbackStartTime = value;
    }

    public bool IsKnockback
    {
        get => isKnockback;
        set => isKnockback = value;
    }

    public float KnockBackTime
    {
        get => knockBackTime;
        set => knockBackTime = value;
    }

    public DamageParameters Parameters
    {
        get => parameters;
        set => parameters = value;
    }

    public bool CanDamage
    {
        get => canDamage;
        set => canDamage = value;
    }
}
