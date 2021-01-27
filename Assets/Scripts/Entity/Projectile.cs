using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : LivingEntity
{
    private float createTime;
    public DamageClass damage;
    private bool Quadratic = false;
    //public float LiveTime;

    public float deltaX;
    public float nowX;
    public float deltaY;
    public float nowY;

    public GameObject track;
    public void SetMoveX(float delta,float start)
    {
        Quadratic = true;
        deltaX = delta;
        nowX = start;
    }
    public void SetMoveY(float delta, float start)
    {
        Quadratic = true;
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
        tr.x += nowX * Time.deltaTime;
        tr.y += nowY * Time.deltaTime;
        nowX += Time.deltaTime * deltaX;
        nowY += Time.deltaTime * deltaY;
        transform.position = tr;
    }
}
