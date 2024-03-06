using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private bool isKnockback = false;
    private float knockBackTime = 0.0f;

    [Header("Dmg")] 
    [SerializeField] private GameObject strikeEffect;
    Dictionary<string, float> dmgCD = new Dictionary<string, float>();


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


        if (!dmgCD.ContainsKey(parameters.DmgName))
        {
            curHp -= dma.finalDamage(baseDmg, dmgBonus, defPierce, curDef, curDmgResistance);
            Instantiate(strikeEffect, transform);

            if (parameters.CdDamage != 0.0f)
            {
                dmgCD.Add(parameters.DmgName, parameters.CdDamage);
                StartCoroutine(removeCdDamage(parameters.DmgName, parameters.CdDamage));
            }
            
            if (playerDirect.x <= transform.position.x && dmy.DummyDirect == 1)
            {
                dmy.Flipping();
            }

            if (playerDirect.x >= transform.position.x && dmy.DummyDirect == -1)
            {
                dmy.Flipping();
            }

            if (curHp <= 0)
            {
                Destroy(gameObject);
            }
        
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
        dmy.CanFlip = false;
        
        Vector2 knockbackDirection = ((Vector2)transform.position - playerDirect).normalized;
        
        dmy.Rb.velocity = new Vector2(knockbackDirection.x * knockBack, knockbackDirection.y * knockBack);
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
