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
    public GameObject[] Worlds;

    /// <summary>
    /// 实体
    /// </summary>
    public GameObject[] Entities;

    /// <summary>
    /// 特效
    /// </summary>
    public GameObject[] Effects;

    /// <summary>
    /// UI对象
    /// </summary>
    public GameObject[] UIPanel;
}