using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpear : Skill
{
    TemperatureMagic TempMagic;
    public GameObject PreIceSpear;
    public bool Trigger1 = false;
    public float Attack1 = 0.9f;
    protected override void OnSkillInit()
    {
        PreIceSpear = GameManager.Instance.Db.Effects[0].gameObject;
        SkillState = "IceSpear";
        CoolDownTime = 3;
        CastTime = 0;
        ReleaseTime = 0.5f;
        StiffTime = 0.1f;
        Attack1 *= role.Value.MagicAtk;

        TempMagic = GetComponent<TemperatureMagic>();
    }
    protected override void BeforeUsing()
    {
        role.anim.SetBool("Attack", true);
        role.ManaRecover(-100);
    }

    protected override void OnUsing()
    {
        if (Releaseing >= 0.4f)
            return;
        if (Trigger1)
            return;
        Trigger1 = true;
        //Releaseing = 100;
        role.anim.SetBool("Attack", false);
        var to =  GameManager.Instance.ActiveWorld.Value.CreateEntity(PreIceSpear);
        to.transform.position = role.transform.position;
        to.Camp = role.Camp;

        var angle = to.transform.eulerAngles;
        angle.z = role.GetFaceAngle();
        
        to.transform.eulerAngles = angle;
        


        Projectile pro = to.GetComponent<Projectile>();
        pro.Velocity = 9;
        pro.Direction = role.GetFaceAngle();
        var _dam = new DamageClass();
        _dam.SetMag();
        _dam.Damage = Attack1 * TempMagic.IceDamage;

        //快速冻结 Flashfreeze
        if (TryGetComponent<Flashfreeze>(out Flashfreeze com))
        {
            _dam.Damage += Attack1 * TempMagic.IceDamage * 1.5f;
        }


        _dam.Element.Ice = true;
        pro.damage = _dam;

    }
    protected override bool CanCast()
    {
        if (!Input.GetMouseButtonDown(0))
            return false;
        if (CoolDowning > 0)
            return false;
        if (role.State != "Noon")
            return false;
        if (role.Mana < 100)
            return false;

        return true;
    }
    protected override void AfterUsing()
    {
        Trigger1 = false;
        TempMagic.IceCast(1);
    }
    protected override void OnUpdate()
    {
    }
}
