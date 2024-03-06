using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private EnemyHealth eh;
    
    [Header("Attack")]
    [SerializeField] private float attackBase = 0.0f;
    [SerializeField] private float attackBonus = 0.0f; //%
    [SerializeField] private float dmgBonus = 0.0f;
    [SerializeField] private float defPierceBase = 0.0f;
    [SerializeField] private float defPierceBonus = 0.0f; //%

    [Header("Crit")]
    [SerializeField] private float critRate;
    [SerializeField] private float critDamage;
    
    [Header("Health")]
    [SerializeField] private float hpBase = 0.0f;
    [SerializeField] private float hpBonus = 0.0f; //%
    [SerializeField] private float defBase = 0.0f; 
    [SerializeField] private float defBonus = 0.0f; //%
    [SerializeField] private float dmgResistanceBase = 0.0f;
    [SerializeField] private float dmgResistanceBonus = 0.0f;

    [SerializeField] private Vector2 knockbackSpeed;
    
    void Start()
    {
        eh = GetComponent<EnemyHealth>();
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
          eh.ChangeStat();
        }
    }

    public float AttackBonus
    {
        get => attackBonus;
        set
        {
            attackBonus = value;
            eh.ChangeStat();
        }
    }

    public float DmgBonus
    {
        get => dmgBonus;
        set
        {
           dmgBonus = value; 
           eh.ChangeStat();
        } 
    }

    public float DefPierceBase
    {
        get => defPierceBase;
        set
        { 
           defPierceBase = value; 
           eh.ChangeStat();
        } 
    }

    public float DefPierceBonus
    {
        get => defPierceBonus;
        set
        {
           defPierceBonus = value; 
           eh.ChangeStat();
        } 
    }

    public float HpBase
    {
        get => hpBase;
        set
        {
            hpBase = value;
            eh.ChangeStat();
        }
    }

    public float HpBonus
    {
        get => hpBonus;
        set
        {
            hpBonus = value;
            eh.ChangeStat();
        }
    }

    public float DefBase
    {
        get => defBase;
        set
        {
            defBase = value;
            eh.ChangeStat();
        }
    }

    public float DefBonus
    {
        get => defBonus;
        set
        {
            defBonus = value;
            eh.ChangeStat();
        }
    }

    public float DmgResistanceBase
    {
        get => dmgResistanceBase;
        set
        {
            dmgResistanceBase = value;
            eh.ChangeStat();
        }
    }

    public float DmgResistanceBonus
    {
        get => dmgResistanceBonus;
        set
        {
            dmgResistanceBonus = value;
            eh.ChangeStat();
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
        set
        {
            critRate = value;
            eh.ChangeStat();
        }
    }

    public float CritDamage
    {
        get => critDamage;
        set
        {
            critDamage = value;
            eh.ChangeStat();
        }
    }
}
