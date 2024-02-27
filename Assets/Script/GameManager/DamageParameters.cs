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

    public DamageParameters(float baseDmg, float dmgBonus, float defPierce, Vector2 playerDirect, float knockBack, float knockBackTime)
    {
        this.baseDmg = baseDmg;
        this.dmgBonus = dmgBonus;
        this.defPierce = defPierce;
        this.playerDirect = playerDirect;
        this.knockBack = knockBack;
        KnockBackTime = knockBackTime;
    }

    public DamageParameters()
    {
    }

    public void setStat(float baseDmg, float dmgBonus, float defPierce, Vector2 playerDirect, float knockBack, float knockBackTime)
    {
        this.baseDmg = baseDmg;
        this.dmgBonus = dmgBonus;
        this.defPierce = defPierce;
        this.playerDirect = playerDirect;
        this.knockBack = knockBack;
        KnockBackTime = knockBackTime;
    }

    public override string ToString()
    {
        return baseDmg + " " + dmgBonus + " " + defPierce + " " + playerDirect + " " + knockBack + " " + KnockBackTime;
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
    
}
