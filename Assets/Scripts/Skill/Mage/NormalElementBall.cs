using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalElementBall : Skill
{
    TemperatureMagic TempMagic;
    public GameObject PreIceBall;
    public GameObject PreFireBall;
    public GameObject PreElementBall;
    private GameObject ball;
    public bool Trigger1 = false;
    public bool Trigger2 = false;
    public bool Trigger3 = false;
    public bool Trigger4 = false;
    public bool Trigger5 = false;
    public float Attack1 = 0.3f;
    public float Attack2 = 0.4f;
    public float Attack3 = 0.5f;
    public float Attack;


    
    protected override void OnSkillInit()
    {
        SkillState = "NormalElementBall";
        TempMagic = GetComponent<TemperatureMagic>();

        CoolDownTime = 0;
        CastTime = 0;
        ReleaseTime = 0.4f;
        StiffTime = 0.3f;
        Attack1 *= role.Value.MagicAtk;
        Attack2 *= role.Value.MagicAtk;
        Attack3 *= role.Value.MagicAtk;

        PreFireBall = GameManager.Instance.Db.Effects[1].gameObject;
        PreElementBall = GameManager.Instance.Db.Effects[2].gameObject;
        PreIceBall = GameManager.Instance.Db.Effects[3].gameObject;
    }
    protected override void BeforeUsing()
    {
        role.anim.SetBool("Attack", true); 
    }
    protected override void AfterUsing()
    {
        Trigger5 = false;
        if (Trigger1 == true && Trigger2 == true && Trigger3 == true)
        {
            Trigger1 = false;
            Trigger2 = false;
            Trigger3 = false;
        }
    }
    protected override void OnUsing()
    {
        if (Releaseing >= 0.4f)
            return;
        if (!Trigger3 && Trigger1 && Trigger2 && !Trigger5)
        {
            Trigger3 = true;
            Trigger4 = true;
            Trigger5 = true;    
            Attack = Attack3;
            AfterState = "Stiff";
        }

        if (!Trigger2 && Trigger1 && !Trigger5)
        {
            Trigger2 = true;
            Trigger4 = true;
            Trigger5 = true;
            Attack = Attack2;
            AfterState = "EB";
        }

        if (!Trigger1 && !Trigger5)
        {
            Trigger1 = true;
            Trigger4 = true;
            Trigger5 = true;
            Attack = Attack1;
            AfterState = "EB";
        }
        if (Trigger4)
        {
            Trigger4 = false;
            role.anim.SetBool("Attack", false);
            var to = GameManager.Instance.ActiveWorld.Value.CreateEntity(ball);
            to.transform.position = role.transform.position;
            to.Camp = role.Camp;

            var angle = to.transform.eulerAngles;
            angle.z = role.GetFaceAngle();
            to.transform.eulerAngles = angle;

            Projectile pro = to.GetComponent<Projectile>();
            pro.SetOnlyOnce();
            pro.Velocity = 9;
            pro.Direction = role.GetFaceAngle();

            var _dam = new DamageClass();
            _dam.SetMag();
            _dam.Damage = Attack;
            if (TempMagic.FireLayers > 0)
            {
                _dam.Element.Fire = true;
                _dam.Damage *= TempMagic.FireDamage;
            }
            if (TempMagic.IceLayers > 0)
            {
                _dam.Element.Ice = true;
                _dam.Damage *= TempMagic.IceDamage;
            }
            pro.damage = _dam;
        }
    }   
    protected override void OnUpdate()
    {
        if (TempMagic.FireLayers > 0)
            ball = PreFireBall;
        if (TempMagic.IceLayers > 0)
            ball = PreIceBall;
        if (TempMagic.IceLayers == 0 && TempMagic.FireLayers == 0)
            ball = PreElementBall;
        if (role.State == "EB" && role.StiffTime <= 0)
        {
            Trigger1 = false;
            Trigger2 = false;
            Trigger3 = false;
            Trigger4 = false;
            Trigger5 = false;
            role.State = "Noon";
        }
    }
    protected override bool CanCast()
    {
        if (!Input.GetKeyDown(KeyCode.X))
            return false;
        if (role.State == "EB")
            return true;
        if (role.State != "Noon")
            return false;

        return true;
    }
}
