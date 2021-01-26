using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : LivingEntity
{
    private float createTime;
    public DamageClass damage;
    private bool Quadratic;
    //public float LiveTime;

    private float deltaX;
    private float nowX;
    private float deltaY;
    private float nowY;
    public void SetMoveX(float delta,float start)
    {
        deltaX = delta;
        nowX = start;
    }
    public void SetMoveY(float delta, float start)
    {
        deltaY = delta;
        nowY = start;
    }
    protected override void OnTouchOtherEntity(Entity other)
    {
        base.OnTouchOtherEntity(other);
        if (!(other is LivingEntity e))
            return;
        //Attf.DealDamage(this, e, damage);
        if (CampStatic.CompareCamp(this, other))
        {
            Attf.DealDamage(this, e, damage);
        }
        Death();
    }
    
    public override void Healing()
    {
        Health -= Time.deltaTime;
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
    public override void Move()
    {
        if (!Quadratic)
        {
            base.Move();
            return;
        }
        var tr = transform.position;
        tr.x += nowX;
        tr.y += nowY;
        nowX -= Time.deltaTime * deltaX;
        nowY -= Time.deltaTime * deltaY;

    }
}
