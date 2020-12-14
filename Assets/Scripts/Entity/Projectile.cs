using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : LivingEntity
{
    private float createTime;
    public override void Death()
    {
        Object.Destroy(this);
    }
    void Init()
    {
        createTime = Time.time + 120;
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (createTime < Time.time)
        {
            Death();
        }
    }
}
