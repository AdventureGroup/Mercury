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

        if (Input.GetKeyDown(KeyCode.D))
        {
            var a = transform.localScale;
            a.x = Mathf.Abs(a.x);
            transform.localScale = a;
            SetFaceToRight();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            var a = transform.localScale;
            a.x = -Mathf.Abs(a.x);
            transform.localScale = a;
            SetFaceToLeft();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }

        //anim.SetBool("Move", false);
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
        FaceTo = false;
    }
    public void SetFaceToRight()
    {
        FaceTo = true;
    }

}
