using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyStat : MonoBehaviour
{
    private DummyHealth dh;
    
    [SerializeField] private float attackBase = 0.0f;
    [SerializeField] private float attackBonus = 0.0f; //%
    [SerializeField] private float dmgBonus = 0.0f;
    [SerializeField] private float defPierceBase = 0.0f;
    [SerializeField] private float defPierceBonus = 0.0f; //%

    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;
    
    [SerializeField] private float hpBase = 0.0f;
    [SerializeField] private float hpBonus = 0.0f; //%
    [SerializeField] private float defBase = 0.0f; 
    [SerializeField] private float defBonus = 0.0f; //%
    [SerializeField] private float dmgResistanceBase = 0.0f;
    [SerializeField] private float dmgResistanceBonus = 0.0f;

    [SerializeField] private Vector2 knockbackSpeed;
    
    void Start()
    {
        dh = GetComponent<DummyHealth>();
    }

    
    void Update()
    {
        
    }


    public float AttackBase
    {
        get => attackBase;
        set
        {
          attackBase = value;  
          dh.ChangeStat();
        }
    }

    public float AttackBonus
    {
        get => attackBonus;
        set
        {
            attackBonus = value;
            dh.ChangeStat();
        }
    }

    public float DmgBonus
    {
        get => dmgBonus;
        set
        {
           dmgBonus = value; 
           dh.ChangeStat();
        } 
    }

    public float DefPierceBase
    {
        get => defPierceBase;
        set
        { 
           defPierceBase = value; 
           dh.ChangeStat();
        } 
    }

    public float DefPierceBonus
    {
        get => defPierceBonus;
        set
        {
           defPierceBonus = value; 
           dh.ChangeStat();
        } 
    }

    public float HpBase
    {
        get => hpBase;
        set
        {
            hpBase = value;
            dh.ChangeStat();
        }
    }

    public float HpBonus
    {
        get => hpBonus;
        set
        {
            hpBonus = value;
            dh.ChangeStat();
        }
    }

    public float DefBase
    {
        get => defBase;
        set
        {
            defBase = value;
            dh.ChangeStat();
        }
    }

    public float DefBonus
    {
        get => defBonus;
        set
        {
            defBonus = value;
            dh.ChangeStat();
        }
    }

    public float DmgResistanceBase
    {
        get => dmgResistanceBase;
        set
        {
            dmgResistanceBase = value;
            dh.ChangeStat();
        }
    }

    public float DmgResistanceBonus
    {
        get => dmgResistanceBonus;
        set
        {
            dmgResistanceBonus = value;
            dh.ChangeStat();
        }
    }

    public Vector2 KnockbackSpeed
    {
        get => knockbackSpeed;
        set => knockbackSpeed = value;
    }

    public float CritRate
    {
        get => critRate;
        set => critRate = value;
    }

    public float CritDamage
    {
        get => critDamage;
        set => critDamage = value;
    }
}
