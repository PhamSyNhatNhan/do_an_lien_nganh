using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Info")] 
    [SerializeField] private string name = "Player";

    [Header("UI")] 
    [SerializeField] private Sprite AttackUi;
    [SerializeField] private Sprite SkillUi;
    [SerializeField] private Sprite BurstUi;
    [SerializeField] private Sprite DashUi;
    
    [Header("Stat")]
    [SerializeField] private float attackBase = 0.0f;
    [SerializeField] private float attackBonus = 0.0f; //%
    [SerializeField] private float dmgBonus = 0.0f;
    [SerializeField] private float defPierceBase = 0.0f;
    [SerializeField] private float defPierceBonus = 0.0f; //%
    
    
    [SerializeField] private float hpBase = 0.0f;
    [SerializeField] private float hpBonus = 0.0f; //%
    [SerializeField] private float defBase = 0.0f; 
    [SerializeField] private float defBonus = 0.0f; //%
    [SerializeField] private float dmgResistanceBase = 0.0f;
    [SerializeField] private float dmgResistanceBonus = 0.0f; //%
    
    
    
    
    [Header("Movement")] 
    [SerializeField]private float movementSpeed = 0.0f;
    
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 0.0f;
    [SerializeField] private float dashTime = 0.0f;
    [SerializeField] private int dashLeft = 0;
    [SerializeField] private float dashCD = 0.0f;
    [SerializeField] private float dashCDDecrease = 0.0f;
    [SerializeField] private int dashMultiple = 0;
    [SerializeField] private int dashBonus = 0;
    [SerializeField] private float dashDelay = 0.0f;
    
    [Header("Attack")]
    [SerializeField] private float attackDelay = 0.0f;
    [SerializeField] private int attackMultiple = 0;
    
    
    [Header("Skill")]
    [SerializeField] private float skillDelay = 0.0f;
    
    [Header("Burst")]
    [SerializeField] private float burstDelay = 0.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float MovementSpeed
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public float AttackBase
    {
        get => attackBase;
        set => attackBase = value;
    }

    public float DmgBonus
    {
        get => dmgBonus;
        set => dmgBonus = value;
    }

    public float DefPierceBase
    {
        get => defPierceBase;
        set => defPierceBase = value;
    }

    public float DefPierceBonus
    {
        get => defPierceBonus;
        set => defPierceBonus = value;
    }

    public float HpBase
    {
        get => hpBase;
        set => hpBase = value;
    }

    public float HpBonus
    {
        get => hpBonus;
        set => hpBonus = value;
    }

    public float DefBase
    {
        get => defBase;
        set => defBase = value;
    }

    public float DefBonus
    {
        get => defBonus;
        set => defBonus = value;
    }

    public float DmgResistanceBase
    {
        get => dmgResistanceBase;
        set => dmgResistanceBase = value;
    }

    public float DmgResistanceBonus
    {
        get => dmgResistanceBonus;
        set => dmgResistanceBonus = value;
    }

    public float MovementSpeed1
    {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public float DashSpeed
    {
        get => dashSpeed;
        set => dashSpeed = value;
    }

    public float DashTime
    {
        get => dashTime;
        set => dashTime = value;
    }

    public int DashLeft
    {
        get => dashLeft;
        set => dashLeft = value;
    }

    public int DashMultiple
    {
        get => dashMultiple;
        set => dashMultiple = value;
    }

    public float DashCd
    {
        get => dashCD;
        set => dashCD = value;
    }

    public int DashBonus
    {
        get => dashBonus;
        set => dashBonus = value;
    }

    public float DashDelay
    {
        get => dashDelay;
        set => dashDelay = value;
    }

    public float DashCdDecrease
    {
        get => dashCDDecrease;
        set => dashCDDecrease = value;
    }

    public float AttackDelay
    {
        get => attackDelay;
        set => attackDelay = value;
    }

    public float SkillDelay
    {
        get => skillDelay;
        set => skillDelay = value;
    }

    public float BurstDelay
    {
        get => burstDelay;
        set => burstDelay = value;
    }

    public int AttackMultiple
    {
        get => attackMultiple;
        set => attackMultiple = value;
    }

    public float AttackBonus
    {
        get => attackBonus;
        set => attackBonus = value;
    }
}
