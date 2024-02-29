using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    private PlayerStat ps;
    private PlayerController pc;
    
    void Start()
    {
        ps = GetComponent<PlayerStat>();
        pc = GetComponent<PlayerController>();
        parameters = new DamageParameters();
        getStats();
    }

    void Update()
    {
        
    }

    private void getStats()
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
        curDef = def;
        curDmgResistance = dmgResistance;
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
        set => hp = value;
    }

    public float CurHp
    {
        get => curHp;
        set => curHp = value;
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
}
