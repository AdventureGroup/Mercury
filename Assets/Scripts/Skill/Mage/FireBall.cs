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
        SkillState = "FireBall";
        TempMagic = GetComponent<TemperatureMagic>();
        CoolDownTime = 2;
        ReleaseTime = 1;

    }
    protected override void OnUsing()
    {
        Releaseing = 0;
        Vector2 pos1, pos2;
        pos1 = pos2 = transform.position;
        pos1.y += 0.6f;
        pos2.y -= 0.6f;
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
        if (rd.Length == 0)
        {
            pos1.x = 3 * role.GetFaceTo();
            pos2.x = 0;
        }
        else
        {
            Collider2D c2d = rd[0];
            float dis = 114514;
            for (int i = 0; i < rd.Length; i++)
            {
                if ((transform.position - rd[i].transform.position).magnitude < dis)
                {
                    dis = (transform.position - rd[i].transform.position).magnitude;
                    c2d = rd[i];
                }
            }
            float time,ky = c2d.transform.position.y - transform.position.y;
            if (dis < 3)
                time = 1;
        }

        

    }
    protected override void BeforeUsing()
    {
    }

}
