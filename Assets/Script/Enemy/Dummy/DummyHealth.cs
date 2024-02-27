using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHealth : MonoBehaviour
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

    [Header("Dmg")] 
    [SerializeField] private GameObject strikeEffect;
    

    private DummyStat ds;
    private Dummy dmy;
    private DamageManager dma;
    
    void Start()
    {
        dmy = GetComponent<Dummy>();
        ds = GetComponent<DummyStat>();
        dma = GameObject.Find("GameManager").GetComponent<DamageManager>();

        getStats();
    }

    void Update()
    {
        checkKnockBack();
    }

    private void getStats()
    {
        attack = ds.AttackBase + ds.AttackBase * ds.AttackBonus / 100;
        dmgBonus = ds.DmgBonus;
        defPierce = ds.DefPierceBase + ds.DefPierceBase * ds.DefPierceBonus / 100;
        
        hp = ds.HpBase + ds.HpBase * ds.HpBonus / 100;
        def = ds.DefBase + ds.DefBase * ds.DefBonus / 100;
        dmgResistance = ds.DmgResistanceBase + ds.DmgResistanceBase * ds.DmgResistanceBonus / 100;

        
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
        attack = ds.AttackBase + ds.AttackBase * ds.AttackBonus / 100;
        dmgBonus = ds.DmgBonus;
        defPierce = ds.DefPierceBase + ds.DefPierceBase * ds.DefPierceBonus / 100;

        float tmpHp = hp - curHp;
        hp = ds.HpBase + ds.HpBase * ds.HpBonus / 100;
        def = ds.DefBase + ds.DefBase * ds.DefBonus / 100;
        dmgResistance = ds.DmgResistanceBase + ds.DmgResistanceBase * ds.DmgResistanceBonus / 100;
        
        curAttack = attack;
        curDamgBonus = dmgBonus;
        curDefPierce = defPierce;

        curHp = hp - tmpHp;
        curDef = def;
        curDmgResistance = dmgResistance;
    }

    public void Damage(DamageParameters parameters)
    {
        float baseDmg = parameters.BaseDmg;
        float dmgBonus = parameters.DmgBonus;
        float defPierce = parameters.DefPierce;
        Vector2 playerDirect = parameters.PlayerDirect;
        float knockBack = parameters.KnockBack;
        float KnockBackTime = parameters.KnockBackTime1;
        
        curHp -= dma.finalDamage(baseDmg, dmgBonus, defPierce, curDef, curDmgResistance);
        Instantiate(strikeEffect, transform);
        
        if (playerDirect.x <= transform.position.x && dmy.DummyDirect == 1)
        {
            dmy.Flipping();
        }

        if (playerDirect.x >= transform.position.x && dmy.DummyDirect == -1)
        {
            dmy.Flipping();
        }
        
        if(curHp <= 0) Destroy(gameObject);
        
        KnockBack(knockBack, KnockBackTime);
    }

    private void KnockBack(float knockBack, float KnockBackTime)
    {
        if(knockBack == 0.0f) return;
        
        KnockbackStartTime = Time.time;
        isKnockback = true;
        knockBackTime = KnockBackTime;
        dmy.CanFlip = false;
        dmy.Rb.velocity = new Vector2(ds.KnockbackSpeed.x * knockBack * dmy.DummyDirect, 0.0f);
    }

    private void checkKnockBack()
    {
        if (isKnockback && knockBackTime > 0.0f)
        {
            knockBackTime -= Time.deltaTime;
            if (knockBackTime <= 0.0f)
            {
                isKnockback = false;
                dmy.CanFlip = true;
                dmy.Rb.velocity = Vector2.zero;

            }
        }
    }
}
