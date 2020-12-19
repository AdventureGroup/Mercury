using System;

[Serializable]
public class ConstProperty
{
    /// <summary>
    /// 最大生命值
    /// </summary>
    public float HealthMax = 600;
    /// <summary>
    /// 生命恢复速度
    /// </summary>
    public float HealthRec = 0;

    /// <summary>
    /// 最大法力值
    /// </summary>
    public float ManaMax = 100;
    /// <summary>
    /// 每秒法力回复
    /// </summary>
    public float ManaRec = 0;


    /// <summary>
    /// 物理攻击力
    /// </summary>
    public float PhysicAtk = 0;

    /// <summary>
    /// 魔法攻击力
    /// </summary>
    public float MagicAtk = 0;

    /// <summary>
    /// 物理防御
    /// </summary>
    public float PhysicDef = 0;

    /// <summary>
    /// 魔法防御
    /// </summary>
    public float MagicDef = 0;

    /// <summary>
    /// 耐力
    /// </summary>
    public int PointEndurance = 0;

    /// <summary>
    /// 精神力
    /// </summary>
    public int PointSpirit = 0;

    /// <summary>
    /// 智力
    /// </summary>
    public int PointIntelligence = 0;

    /// <summary>
    /// 力量
    /// </summary>
    public int PointStrength = 0;

    /// <summary>
    /// 暴击
    /// </summary>
    public int Critical = 0;

    /// <summary>
    /// 暴击系数
    /// </summary>
    public float CriticalCoe = 1.5f;

    /// <summary>
    /// 攻击速度
    /// </summary>
    public float SpeedAtk = 1;

    /// <summary>
    /// 移动
    /// </summary>
    public float SpeedMov = 1;

    /// <summary>
    /// 技能急速
    /// </summary>
    public float SpeedSkl = 1;

    
}
