using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : LivingEntity
{
    /// <summary>
    /// 法力值
    /// </summary>
    public float Mana;
    public string SkillCast = "NOON"; 

    public ConstProperty Value, Property;
    public override void Healing()
    {
        Health += Value.HealthRec;
        if (Health > Value.HealthMax) Health = Value.HealthMax;
    }



}
