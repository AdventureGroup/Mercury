using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role : LivingEntity
{

    /// <summary>
    /// 法力值
    /// </summary>
    public float Mana;
    public string SkillCast = "Noon";
    public string State = "Noon";
    public float StiffTime = 0;
    public ConstProperty Value, Property;
    public override void Healing()
    {
        Health += Value.HealthRec;
        if (Health > Value.HealthMax) Health = Value.HealthMax;
    }


    protected override void PerFrame()
    {

        StiffTime -= Time.deltaTime;
        if (StiffTime <= 0 && State == "Stiff")
            State = "Noon";
    }

    private bool FaceTo;
    public int GetFaceTo()
    {
        if (!FaceTo) return -1;
        return 1;
    }
    public float GetFaceAngle()
    {
        if (FaceTo) return 0;
        return 180;
    }
    public void SetFaceToLeft()
    {
        FaceTo = false;
    }
    public void SetFaceToRight()
    {
        FaceTo = true;
    }

}
