using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public float finalDamage(
        float baseDmg,
        float dmgBonus, 
        float defPierce,
        
        float def,
        float dmgResistance
    )
    {
        float calculatedDamage = baseDmg;
        
        if (dmgBonus != 0)
            calculatedDamage *= dmgBonus;
        
        if (defPierce != 0 && def != 0)
            calculatedDamage *= (1.0f - defCalculate(def) * defPierce / 100.0f);
        
        if (dmgResistance != 0)
            calculatedDamage *= (100.0f - dmgResistance) / 100.0f;

        return calculatedDamage;
    }

    private float defCalculate(float def)
    {
        return def / (def + 500.0f);
    }
}
