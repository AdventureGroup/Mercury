using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureMagic : Skill
{
    public float Timer;
    private float MaxTimer = 10;
    /// <summary>
    /// 火焰的加成系数
    /// </summary>
    private float ArgumentFire = 2f;
    /// <summary>
    /// 寒冰的加成系数
    /// </summary>
    private float ArgumentIce = 0.2f;
    public float IceSpeedDown;
    public int FireLayers; //{ private set; get; }
    public int IceLayers; //{ private set; get; }
    public float FireDamage{ get => (IceLayers > 0) ? 0.3f :(FireLayers * ArgumentFire + 1); }
    public float FireCastMana { get => ((IceLayers >= 3) ? 0 : Mathf.Pow(2, FireLayers)); }
    public float IceDamage{ get => (FireLayers > 0) ? 0.7f : (IceLayers * ArgumentIce + 1); }
    public void FireCast(int CastCount)
    {
        for (int i = 1; i <= CastCount; i ++)
            if (IceLayers > 0)
                IceLayers--;
            else
                FireLayers++;
        Timer = MaxTimer;
    }
    public void IceCast(int CastCount)
    {
        for (int i = 1; i <= CastCount; i++)
            if (FireLayers > 0)
                FireLayers--;
            else
                IceLayers++;
        Timer = MaxTimer;
    }
    public void FireTo(int ChangeTo)
    {
        FireLayers = ChangeTo;
        IceLayers = 0;
        Timer = MaxTimer;
    }
    public void IceTo(int ChangeTo)
    {
        IceLayers = ChangeTo;
        FireLayers = 0;
        Timer = MaxTimer;
    }
    protected override void OnUpdate()
    {
        if (FireLayers > 0)
        {
            role.ManaRecover(role.Value.ManaRec * -1 * Time.deltaTime);
            role.HealthRecover(role.Value.HealthMax * -0.02f * FireLayers * Time.deltaTime);
        }
            //role.HealthRecover(-10);
        if (IceLayers > 0)
            role.ManaRecover(role.Value.ManaRec * 0.3f * IceLayers * Time.deltaTime);

        Timer -= Time.deltaTime * (1 +  Mathf.Max(FireLayers,IceLayers) * 0.2f);
        if (Timer <= 0)
        {
            if (FireLayers > 0)
                IceCast(1);
            if (IceLayers > 0)
                FireCast(1);
        }
    }
}
