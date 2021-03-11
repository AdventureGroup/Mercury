using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : LivingEntity
{
    private float createTime;
    public DamageClass damage;
    private bool Quadratic = false;
    private bool OnlyOnce = false;
    //public float LiveTime;

    public float deltaX;
    public float nowX;
    public float deltaY;
    public float nowY;

    public GameObject track;
    public List<Entity> list;
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
            if (list.Contains(other) == false)
            {
                list.Add(other);
                Attf.DealDamage(this, e, damage);
            }
        }
        if (OnlyOnce)
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
        if (Health <= 0)
        {
            Death();
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

        var angle = transform.eulerAngles;

        Vector2 v2d;
        v2d.x = nowX; v2d.y = nowY;
        angle.z = Vector2.SignedAngle(Vector2.left, v2d)+180;
        //angle.z = Mathf.Asin(nowY / nowX) * Mathf.Rad2Deg;
        //if (nowY > 0)
        //    angle.z += 180;
        transform.eulerAngles = angle;

        nowX += Time.deltaTime * deltaX;
        nowY += Time.deltaTime * deltaY;
        transform.position = tr;
    }


    public void SetOnlyOnce()
    {
        OnlyOnce = true;
    }
    public void SetManyTimes()
    {
        OnlyOnce = false;
    }
}
