using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpear : Skill
{
    TemperatureMagic TempMagic;
    public GameObject PreIceSpear;
    public float Direction;
    public bool Trigger1 = false;
    protected override void OnSkillInit()
    {
        PreIceSpear = GameManager.Instance.Db.Effects[0].gameObject;
        SkillState = "IceSpear";
        CoolDownTime = 7;
        CastTime = 0;
        ReleaseTime = 0.7f;
        StiffTime = 0.1f;

        TempMagic = GetComponent<TemperatureMagic>();
    }
    protected override void BeforeUsing()
    {
        role.anim.SetBool("Attack", true);
        role.ManaRecover(-100);
    }

    protected override void OnUsing()
    {
        if (Releaseing >= 0.5f)
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
        _dam.Damage = 60f * TempMagic.IceDamage;
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
        Vector2 role2 = role.transform.position;
        Vector2 mouse2 = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Direction = Vector2.SignedAngle(Vector2.right, mouse2 - role2);
    }
}
