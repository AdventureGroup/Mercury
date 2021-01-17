using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : LivingEntity
{
    private float createTime;
    public Dam damage;
    protected override void OnTouchOtherEntity(Entity other)
    {
        base.OnTouchOtherEntity(other);
        Death();
        if (!(other is Role e))
            return;
        Attf.DealDamage(this, e, damage);
        if (CampStatic.CompareCamp(this, other))
        {
            Attf.DealDamage(this, e, damage);
            Death();
        }
    }
    public override void Healing()
    {
        Health -= 1 * Time.deltaTime;
    }
    public override void Death()
    {
        GameManager.Instance.ActiveWorld.Value.DestroyEntity(this);
    }
    protected override void Create()
    {
        createTime = Time.time + 120;
    }

    protected override void PerFrame()
    {
        if (createTime < Time.time)
        {
            //Death();
        }
    }
}
