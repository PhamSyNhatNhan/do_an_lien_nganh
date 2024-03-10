using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageParameters
{
    private float baseDmg;
    private float dmgBonus;
    private float defPierce;
    private Vector2 playerDirect;
    private float knockBack;
    private float KnockBackTime;

    private string dmgName;
    private float cdDamage;

    public DamageParameters(float baseDmg, float dmgBonus, float defPierce, Vector2 playerDirect, float knockBack, float knockBackTime, string dmgName, float cdDamage)
    {
        this.baseDmg = baseDmg;
        this.dmgBonus = dmgBonus;
        this.defPierce = defPierce;
        this.playerDirect = playerDirect;
        this.knockBack = knockBack;
        KnockBackTime = knockBackTime;
        this.dmgName = dmgName;
        this.cdDamage = cdDamage;
    }
    
    public DamageParameters Clone()
    {
        DamageParameters clonedParameters = new DamageParameters();
        
        clonedParameters.baseDmg = this.baseDmg;
        clonedParameters.dmgBonus = this.dmgBonus;
        clonedParameters.defPierce = this.defPierce;
        clonedParameters.playerDirect = this.playerDirect;
        clonedParameters.knockBack = this.knockBack;
        clonedParameters.KnockBackTime = this.KnockBackTime;
        clonedParameters.dmgName = this.dmgName;
        clonedParameters.cdDamage = this.cdDamage;
        
        return clonedParameters;
    }


    public DamageParameters()
    {
    }

    public void setStat(float baseDmg, float dmgBonus, float defPierce, Vector2 playerDirect, float knockBack, float knockBackTime, string dmgName, float cdDamage)
    {
        this.baseDmg = baseDmg;
        this.dmgBonus = dmgBonus;
        this.defPierce = defPierce;
        this.playerDirect = playerDirect;
        this.knockBack = knockBack;
        KnockBackTime = knockBackTime;
        this.dmgName = dmgName;
        this.cdDamage = cdDamage;
    }
    
    public void setStat(float baseDmg, float dmgBonus, float defPierce, Vector2 playerDirect, float knockBack, float knockBackTime)
    {
        this.baseDmg = baseDmg;
        this.dmgBonus = dmgBonus;
        this.defPierce = defPierce;
        this.playerDirect = playerDirect;
        this.knockBack = knockBack;
        KnockBackTime = knockBackTime;
        this.dmgName = "";
        this.cdDamage = 0.0f;
    }

    public override string ToString()
    {
        return "base damge: " + baseDmg + ", damage bonus" + dmgBonus + " " + defPierce + " " + playerDirect + " " + knockBack + " " + KnockBackTime;
    }

    public float BaseDmg
    {
        get => baseDmg;
        set => baseDmg = value;
    }

    public float DmgBonus
    {
        get => dmgBonus;
        set => dmgBonus = value;
    }

    public Vector2 PlayerDirect
    {
        get => playerDirect;
        set => playerDirect = value;
    }

    public float KnockBack
    {
        get => knockBack;
        set => knockBack = value;
    }

    public float KnockBackTime1
    {
        get => KnockBackTime;
        set => KnockBackTime = value;
    }

    public float DefPierce
    {
        get => defPierce;
        set => defPierce = value;
    }

    public string DmgName
    {
        get => dmgName;
        set => dmgName = value;
    }

    public float CdDamage
    {
        get => cdDamage;
        set => cdDamage = value;
    }
}
