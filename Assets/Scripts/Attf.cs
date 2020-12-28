using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attf
{
    public static void DealDamage(LivingEntity from,LivingEntity to,Dam damage)
    {
        to.UnderAttack(from, damage);
    }
}
