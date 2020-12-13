using KSGFK.Unsafe;
using UnityEngine;

/// <summary>
/// 矩形碰撞体
/// </summary>
public class ColliderQuadBox : MonoBehaviour
{
    /// <summary>
    /// 矩形长
    /// </summary>
    public float Width;

    /// <summary>
    /// 矩形宽
    /// </summary>
    public float Height;

    /// <summary>
    /// 中心点X
    /// </summary>
    public float OffsetX;

    /// <summary>
    /// 中心点Y
    /// </summary>
    public float OffsetY;

    /// <summary>
    /// 包围盒
    /// </summary>
    public Aabb2D Aabb
    {
        get
        {
            var pos = (Vector2) transform.position + new Vector2(OffsetX, OffsetY);
            var hw = Width / 2f;
            var hh = Height / 2f;
            return new Aabb2D(pos.x - hw, pos.y - hh, pos.x + hw, pos.y + hh);
        }
    }

    /// <summary>
    /// 精确检查两碰撞盒是否接触
    /// </summary>
    public bool IsTouch(ColliderQuadBox other) { return Aabb.IsCross(other.Aabb); }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var z = transform.position.z;
        var aabb = Aabb;
        Gizmos.DrawLine(new Vector3(aabb.Left, aabb.Down, z), new Vector3(aabb.Left, aabb.Up, z));
        Gizmos.DrawLine(new Vector3(aabb.Left, aabb.Down, z), new Vector3(aabb.Right, aabb.Down, z));
        Gizmos.DrawLine(new Vector3(aabb.Right, aabb.Up, z), new Vector3(aabb.Left, aabb.Up, z));
        Gizmos.DrawLine(new Vector3(aabb.Right, aabb.Up, z), new Vector3(aabb.Right, aabb.Down, z));
    }
}