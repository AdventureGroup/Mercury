using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Skill
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
        role.SkillCast = "FireBall";
        Casting = CastTime;
        Releaseing = ReleaseTime;
    }

}
