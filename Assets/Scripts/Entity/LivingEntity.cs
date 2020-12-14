using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : Entity
{
    public float Health;
    public float HealthRec;
    public float Velocity;
    public float Direction;

    public virtual void Healing()
    {
        Health += HealthRec;
    }
    public virtual void Death()
    {
        
    }
    private void Update()
    {
        Healing();
        if (Health <= 0)
            Death();
    }
   

}
