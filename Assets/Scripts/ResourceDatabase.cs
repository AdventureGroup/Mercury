using UnityEngine;

/// <summary>
/// 数据类
/// </summary>
// [CreateAssetMenu(fileName = "ResourceDatabase")]
public class ResourceDatabase : ScriptableObject
{
    /// <summary>
    /// 地图
    /// </summary>
    public World[] Worlds;

    /// <summary>
    /// 实体
    /// </summary>
    public Entity[] Entities;
    /// <summary>
    /// 特效
    /// </summary>
    public Entity[] Effects;
}