﻿public class DamageClass
{
    /// <summary>
    /// 伤害
    /// </summary>
    public float Damage;
    public bool RandomDamage = true;
    public struct elem
    {
        public bool Fire;
        public bool Ice;
        public bool Thunder;
        public bool Water;
        public bool Wind;

        public bool Light;
        public bool Dark;

        public override string ToString()
        {
            string s = "";
            if (Fire) s += "Fire";
            if (Ice) s += "Ice";
            if (Thunder) s += "Thunder";
            if (Water) s += "Water";
            if (Wind) s += "Wind";
            if (Light) s += "Light";
            if (Dark) s += "Dark";

            return s;
        }
    };
    /// <summary>
    /// 包含的属性，可以点出来
    /// </summary>
    public elem Element;
    public int Type;
    public void SetPhy()
    {
        Type = 1;
    }
    public void SetMag()
    {
        Type = 2;
    }
    public void SetTrue()
    {
        Type = 0;
    }
    public static int PhyAttackCap = 1;
    public static int MagAttackCap = 2;
    public static int TrueAttackCap = 0;
}
