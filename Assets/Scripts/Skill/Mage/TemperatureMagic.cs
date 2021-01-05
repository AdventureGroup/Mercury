using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureMagic : MonoBehaviour
{
    /// <summary>
    /// 火焰的加成系数
    /// </summary>
    public float ArgumentFire = 0.7f;
    /// <summary>
    /// 寒冰的加成系数
    /// </summary>
    public float ArgumentIce = 0.3f;
    public int FireLayers; //{ private set; get; }
    public int IceLayers; //{ private set; get; }
    public float FireDamage{ get => FireLayers * ArgumentFire + 1; }
    public float IceDamage{ get => IceLayers * ArgumentIce + 1; }
    public void FireCast(int CastCount)
    {
        for (int i = 1; i <= CastCount; i ++)
            if (IceLayers > 0)
                IceLayers--;
            else
                FireLayers++;
    }
    public void IceCast(int CastCount)
    {
        for (int i = 1; i <= CastCount; i++)
            if (FireLayers > 0)
                FireLayers--;
            else
                IceLayers++;
    }
    public void FireTo(int ChangeTo)
    {
        FireLayers = ChangeTo;
        IceLayers = 0;
    }
    public void IceTo(int ChangeTo)
    {
        IceLayers = ChangeTo;
        FireLayers = 0;
    }
}
