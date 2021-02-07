using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attf
{
    public static void DealDamage(LivingEntity from,LivingEntity to,DamageClass damage)
    {
        if (damage.RandomDamage)
            damage.Damage += Random.Range(-0.1f * damage.Damage, 0.1f * damage.Damage);
        to.UnderAttack(from, damage);
    }
}
