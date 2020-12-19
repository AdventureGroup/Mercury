using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpear : Skill
{
    TemperatureMagic TempMagic;
    public GameObject PreIceSpear;
    public float Direction;
    protected override void OnSkillInit()
    {
        TempMagic = GetComponent<TemperatureMagic>();
        CoolDownTime = 2;
        ReleaseTime = 1;
    }
    protected override void BeforeUsing()
    {
        CoolDowning = CoolDownTime;
        role.SkillCast = "IceSpear";
        Casting = CastTime;
        Releaseing = ReleaseTime;
    }
    protected override void OnUsing()
    {
        Releaseing = 0;
        var to =  GameManager.Instance.ActiveWorld.Value.CreateEntity(PreIceSpear);
        to.transform.position = role.transform.position;
        Projectile pro = to.GetComponent<Projectile>();
        pro.Velocity = 3;
        pro.Direction = Direction;
    }
    protected override bool CanCast()
    {
        if (!Input.GetMouseButtonDown(0))
            return false;
        if (CoolDowning > 0)
            return false;
        return true;
    }
    protected override void AfterUsing()
    {
        TempMagic.FireCast(1);
        role.SkillCast = "Stiff";
    }
    protected override void OnUpdate()
    {
        Vector2 role2 = role.transform.position;
        Vector2 mouse2 = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        Direction = Vector2.SignedAngle(Vector2.right, mouse2 - role2);
    }
}
