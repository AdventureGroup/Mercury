using System;
using UnityEngine;

public class LivingEntity : Entity
{
    /// <summary>
    /// <para>当前生命值，如果为0 则会调用Death函数 这里Death函数提供重写方案</para>
    /// </summary>
    public float Health;
    public float Velocity;
    public float Direction;
    
    public event Action<LivingEntity, DamageClass> Attacked;
    
    /// <summary>
    /// 每帧会调用的函数，用于修改生命值的函数。
    /// </summary>
    public virtual void Healing(){}
    /// <summary>
    /// 当生命值小于等于0的时候系统会自动调用该函数。
    /// </summary>
    public virtual void Death(){}
    /// <summary>
    /// 受到攻击的时候调用，可以被复写
    /// </summary>
    /// <param name="from"></param>
    /// <param name="dam"></param>
    public virtual void UnderAttack(LivingEntity from,DamageClass dam)
    {
        Attacked?.Invoke(from, dam);
        Health -= dam.Damage;
    }
    protected virtual void Create() { }
    protected virtual void PerFrame() { }
    public virtual void Move() 
    {
        var tr = transform.position;
        tr.x += Mathf.Cos(Direction * Mathf.PI / 180) * Velocity * Time.deltaTime;
        tr.y += Mathf.Sin(Direction * Mathf.PI / 180) * Velocity * Time.deltaTime;
        transform.position = tr;
    }
    void Update()
    {
        Move();
        PerFrame();
        Healing();
        if (Health <= 0)
            Death();
    }
    public override void OnCreated()
    {
        base.OnCreated();
        Create();
    }


}
