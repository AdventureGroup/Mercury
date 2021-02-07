using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureMagic : Skill
{
    public float Timer;
    /// <summary>
    /// 火焰的加成系数
    /// </summary>
    public float ArgumentFire = 0.8f;
    /// <summary>
    /// 寒冰的加成系数
    /// </summary>
    public float ArgumentIce = 0.2f;
    public float IceSpeedDown;
    public int FireLayers; //{ private set; get; }
    public int IceLayers; //{ private set; get; }
    public float FireDamage{ get => (IceLayers > 0) ? 0.3f :(FireLayers * ArgumentFire + 1); }
    public float IceDamage{ get => (FireLayers > 0) ? 0.7f : (IceLayers * ArgumentIce + 1); }
    public void FireCast(int CastCount)
    {
        for (int i = 1; i <= CastCount; i ++)
            if (IceLayers > 0)
                IceLayers--;
            else
                FireLayers++;
        Timer = 5;
    }
    public void IceCast(int CastCount)
    {
        for (int i = 1; i <= CastCount; i++)
            if (FireLayers > 0)
                FireLayers--;
            else
                IceLayers++;
        Timer = 5;
    }
    public void FireTo(int ChangeTo)
    {
        FireLayers = ChangeTo;
        IceLayers = 0;
        Timer = 5;
    }
    public void IceTo(int ChangeTo)
    {
        IceLayers = ChangeTo;
        FireLayers = 0;
        Timer = 5;
    }
    protected override void OnUpdate()
    {
        if (FireLayers > 0)
            role.HealthRecover(-role.Value.HealthMax * 0.02f * FireLayers * Time.deltaTime);
        if (IceLayers > 0)
            role.ManaRecover(role.Value.ManaMax * 0.02f * IceLayers * Time.deltaTime);

        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            if (FireLayers > 0)
                IceCast(1);
            if (IceLayers > 0)
                FireCast(1);
        }
    }
}
