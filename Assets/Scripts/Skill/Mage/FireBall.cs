﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
{
    TemperatureMagic TempMagic;
    public GameObject PreFireBall;
    public float Direction;
    public bool Trigger1 = false;
    public float Attack1 = 1.2f;
    protected override void OnSkillInit()
    {
        SkillState = "FireBall";
        TempMagic = GetComponent<TemperatureMagic>();

        CoolDownTime = 3;
        CastTime = 0;
        ReleaseTime = 0.5f;
        StiffTime = 0;
        Attack1 *= role.Value.MagicAtk;

        PreFireBall = GameManager.Instance.Db.Effects[1].gameObject;
    }
    protected override void BeforeUsing()
    {
        role.anim.SetBool("Attack", true);
        role.ManaRecover(-100 * TempMagic.FireCastMana);
    }
    protected override void OnUsing()
    {
        //Releaseing = 0;
        if (Releaseing >= 0.4f)
            return;
        if (Trigger1)
            return;
        Trigger1 = true;
        role.anim.SetBool("Attack", false);
        Vector2 pos1, pos2;
        pos1 = pos2 = transform.position;
        pos1.y += 1f;
        pos2.y -= 1;
        if (role.GetFaceTo() == 1)
        {
            pos1.x += 1;
            pos2.x += 6;
        }
        else
        {
            pos1.x -= 6;
            pos2.x -= 1;
        }
        Collider2D[] rd = Physics2D.OverlapAreaAll(pos1,pos2);
        pos1.y = 4;
        pos2.y = -8;
        pos2.x = 0;
        Collider2D c2d = rd[0];
        if (rd.Length == 0)
        {
            pos1.x = 3 * role.GetFaceTo();
        }
        else
        {
            float dis = 114514;
            for (int i = 0; i < rd.Length; i++)
            {
                if ((transform.position - rd[i].transform.position).magnitude < dis && rd[i].gameObject.TryGetComponent<Role>(out var r))
                {
                    if (!CampStatic.CompareCamp(role, r))
                        return;
                    dis = (transform.position - rd[i].transform.position).magnitude;
                    c2d = rd[i];
                }
            }
            float time,ky = c2d.transform.position.y - transform.position.y;
            if (ky > 1) dis = 114514;
            if (dis < 1)
                time = (-4 + Mathf.Sqrt(Mathf.Max(16 - 4 * (-4 * ky), 0))) / -8;
            else
                time = (-4 - Mathf.Sqrt(Mathf.Max(16 - 4 * (-4 * ky), 0))) / -8;
            pos1.x = (c2d.transform.position.x - transform.position.x) / time;

            if (dis == 114514)
            {
                pos1.x = 3 * role.GetFaceTo();
                c2d = role.GetComponent<Collider2D>();
            }
        }


        var to = GameManager.Instance.ActiveWorld.Value.CreateEntity(PreFireBall);
        to.transform.position = role.transform.position;
        to.Camp = role.Camp;

        Projectile pro = to.GetComponent<Projectile>();
        pro.SetOnlyOnce();
        var _dam = new DamageClass();
        _dam.SetMag();
        _dam.Damage = Attack1 * TempMagic.FireDamage;
        _dam.Element.Fire = true;
        pro.SetMoveX(0, pos1.x);
        pro.SetMoveY(pos2.y, pos1.y);
        pro.damage = _dam;
        
        pro.track = c2d.gameObject;


    }
    protected override void AfterUsing()
    {
        TempMagic.FireCast(1);
        Trigger1 = false;
    }
    protected override bool CanCast()
    {
        if (!IsRequestUse)
            return false;
        IsRequestUse = false;
        if (CoolDowning > 0)
            return false;
        if (role.State != "Noon")
            return false;
        if (role.Mana < 100 * TempMagic.FireCastMana)
            return false;

        return true;
    }

}
