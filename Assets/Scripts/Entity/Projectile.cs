using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : LivingEntity
{
    private float createTime;

    public override void Healing()
    {
        Health -= 1;
    }
    public override void Death()
    {
        Object.Destroy(this);
    }
    protected override void Create()
    {
        createTime = Time.time + 120;
    }

    protected override void PerFrame()
    {
        if (createTime < Time.time)
        {
            Death();
        }
    }
}
