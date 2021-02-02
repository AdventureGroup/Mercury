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

    public bool debug;
    public bool IsMoving;


    public override void Healing()
    {
        Health += Value.HealthRec;
        if (Health > Value.HealthMax) Health = Value.HealthMax;
    }


    protected override void PerFrame()
    {
        debug = false;
        StiffTime -= Time.deltaTime;
        if (StiffTime <= 0 && State == "Stiff")
            State = "Noon";
        debug = false;
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
        var a = transform.localScale;
        a.x = -Mathf.Abs(a.x);
        transform.localScale = a;
        FaceTo = false;
    }
    public void SetFaceToRight()
    {
        var a = transform.localScale;
        a.x = Mathf.Abs(a.x);
        transform.localScale = a;
        FaceTo = true;
    }
    public void SetMoveState(bool isMoving)
    {
        IsMoving = isMoving;
        anim.SetBool("Move", IsMoving);//真的每个Role的动画都有Move吗?
    }
}
