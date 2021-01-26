using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string SkillState;
    protected Role role;
    /// <summary>
    /// 属性
    /// 技能持续时间，持续时间内角色无法被控制。
    /// </summary>
    protected float ReleaseTime = 0;
    /// <summary>
    /// 当前数值
    /// 当大于0时代表正在释放技能
    /// </summary>
    public float Releaseing = 0;

    /// <summary>
    /// 属性
    /// 技能咏唱时间，如果瞬发请设置为0
    /// </summary>
    protected float CastTime = 0;
    /// <summary>
    /// 当前数值
    /// 当大于0时代表正在咏唱！
    /// </summary>
    protected float Casting = 0;


    /// <summary>
    /// 属性
    /// 技能冷却时间
    /// </summary>
    protected float CoolDownTime = 0;
    /// <summary>
    /// 当前数值
    /// 当为大于0时，代表正在释放技能
    /// </summary>
    public float CoolDowning = 0;

    public float StiffTime = 0;


    /// <summary>
    /// 注意：为了防止重复处罚技能，请务必重写该函数。
    /// 技能释放前的会执行的事情
    /// 蓄力类型的请放在OnCasting里面
    /// </summary>
    protected virtual void BeforeUsing(){}
    /// <summary>
    /// 蓄力的时候会执行，不需要在写时间流逝了。
    /// </summary>
    protected virtual void OnCasting() { }
    /// <summary>
    /// 技能释放时会执行
    /// </summary>
    protected virtual void OnUsing(){}
    /// <summary>
    /// 技能释放结束之后会执行的事情
    /// </summary>
    protected virtual void AfterUsing(){}
    /// <summary>
    /// 写条件如果条件允许则技能会被释放
    /// </summary>
    protected virtual bool CanCast() { return false; }

    private void Start()
    {
        role = GetComponent<Role>() ?? throw new ArgumentException("？ 释放技能的不是role类型");
        OnSkillInit();
    }

    protected virtual void OnSkillInit(){}

    void Update()
    {
        OnUpdate();
        if (CanCast() == true)
        {
            CoolDowning = CoolDownTime;
            role.SkillCast = SkillState;
            Casting = CastTime;
            Releaseing = ReleaseTime;

            BeforeUsing();
        }
        if (CoolDowning > 0)
            CoolDowning -= Time.deltaTime;
        if (role.SkillCast == SkillState)
        {
            if (Casting > 0)
                Casting -= Time.deltaTime;
            if (Casting > 0)
            {
                OnCasting();
                return ;
            }
            if (Releaseing > 0 && Casting <= 0)
                Releaseing -= Time.deltaTime;
            if (Releaseing > 0)
            {
                OnUsing();
                return;
            }
            if (Releaseing <= 0)
            {
                role.SkillCast = "Noon";
                role.State = "Stiff";
                role.StiffTime = StiffTime;
                AfterUsing();
            }
        }
    }
    protected virtual void OnUpdate() { }
}
