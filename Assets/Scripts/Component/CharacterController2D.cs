using UnityEngine;

/// <summary>
/// 基于射线检测的2D角色控制器
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    /// <summary>
    /// 这一帧是否可以穿过 <c>OneWayPlatformLayer</c> 设定的碰撞层
    /// </summary>
    public bool IsIgnorePlatform;

    /// <summary>
    /// 不可穿越平台物理碰撞层
    /// </summary>
    public LayerMask PlatformLayer;

    /// <summary>
    /// 可穿过物理碰撞层
    /// <para>一般来说可穿越的平台的Collider2D的类型都是EdgeCollider2D，否则会出现不可预料的结果</para>
    /// </summary>
    public LayerMask OneWayPlatformLayer;

    /// <summary>
    /// 外层预留宽度，该值如果小于等于0可能出现穿墙问题
    /// </summary>
    public float SkinWidth = (float) 1e-5;

    /// <summary>
    /// 水平方向射线发射数量
    /// </summary>
    [Range(2, 16)] public int HorizontalRay = 3;

    /// <summary>
    /// 垂直方向射线发射数量
    /// </summary>
    [Range(2, 16)] public int VerticalRay = 3;

    /// <summary>
    /// float精度阈值，过小会导致物体抖动
    /// </summary>
    public float FloatThreshold = (float) 1e-6;

    [SerializeField] private Vector2 _velocity;

    private Collider2D _coll;
    private Transform _trans;
    private Vector2 _deltaMove;

    private bool _leftHit;
    private bool _rightHit;
    private bool _upHit;
    private bool _downHit;

    /// <summary>
    /// 是否站在平台上
    /// </summary>
    public bool IsBottomOnPlatform => _downHit;

    /// <summary>
    /// 当 <c>IsBottomOnPlatform</c> 属性为true时该属性为顶部碰撞到的 <c>Collider2D</c>，否则该属性为null
    /// </summary>
    public Collider2D BottomPlatform { get; private set; }

    /// <summary>
    /// 顶部是否碰到平台
    /// </summary>
    public bool IsTopUnderPlatform => _upHit;

    /// <summary>
    /// 当 <c>IsTopUnderPlatform</c> 属性为true时该属性为底部碰撞到的 <c>Collider2D</c>，否则该属性为null
    /// </summary>
    public Collider2D TopPlatform { get; private set; }

    /// <summary>
    /// 任意方向是否碰到平台
    /// </summary>
    public bool IsCollidePlatform => _leftHit || _rightHit || _upHit || _downHit;

    /// <summary>
    /// 当前速度
    /// </summary>
    public Vector2 Velocity => _velocity;

    private void Awake()
    {
        _coll = GetComponent<Collider2D>();
        _trans = transform;
    }

    private void LateUpdate()
    {
        ResetHitState();
        if (Mathf.Abs(_deltaMove.x) - FloatThreshold > 0)
        {
            MoveHorizontal();
        }

        if (Mathf.Abs(_deltaMove.y) - FloatThreshold > 0)
        {
            MoveVertical();
        }

        _trans.position += new Vector3(_deltaMove.x, _deltaMove.y);
        _velocity = _deltaMove / Time.deltaTime;
        _deltaMove = Vector2.zero;
        IsIgnorePlatform = false;
    }

    [System.Diagnostics.Conditional("DEBUG")]
    private void DrawRay(Vector3 start, Vector3 direction, Color color) { Debug.DrawRay(start, direction, color); }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="deltaMove">位置的变化量</param>
    public void Move(Vector2 deltaMove) { _deltaMove += deltaMove; }

    private void MoveHorizontal()
    {
        var isRight = _deltaMove.x > 0;
        var rayDistance = Mathf.Abs(_deltaMove.x) + SkinWidth;
        var rayDirection = isRight ? Vector2.right : Vector2.left;
        var aabb = _coll.bounds;
        var min = aabb.min;
        var max = aabb.max;
        var startPos = new Vector2(isRight ? max.x : min.x, min.y);
        var step = (max.y - min.y) / (HorizontalRay - 1);
        for (var i = 0; i < HorizontalRay; i++)
        {
            DrawRay(startPos, rayDirection, Color.green);
            RaycastHit2D castResult;
            if (IsIgnorePlatform)
            {
                castResult = Physics2D.Raycast(startPos,
                    rayDirection,
                    rayDistance,
                    PlatformLayer & ~OneWayPlatformLayer);
            }
            else
            {
                castResult = Physics2D.Raycast(startPos,
                    rayDirection,
                    rayDistance,
                    PlatformLayer | OneWayPlatformLayer);
            }

            startPos.y += step;
            if (!castResult) continue;
            var hitDis = castResult.distance;
            var newMove = hitDis - SkinWidth;
            if (isRight)
            {
                _deltaMove.x = newMove;
                _rightHit = true;
            }
            else
            {
                _deltaMove.x = -newMove;
                _leftHit = true;
            }

            if (Mathf.Abs(_deltaMove.x) - FloatThreshold <= 0) _deltaMove.x = 0;
            break;
        }
    }

    private void MoveVertical()
    {
        var isUp = _deltaMove.y > 0;
        var rayDistance = Mathf.Abs(_deltaMove.y) + SkinWidth;
        var rayDirection = isUp ? Vector2.up : Vector2.down;
        var aabb = _coll.bounds;
        var min = aabb.min;
        var max = aabb.max;
        var startPos = new Vector2(min.x, isUp ? max.y : min.y);
        var step = (max.x - min.x) / (VerticalRay - 1);
        for (var i = 0; i < VerticalRay; i++)
        {
            DrawRay(startPos, rayDirection, Color.green);
            RaycastHit2D castResult;
            if (IsIgnorePlatform)
            {
                castResult = Physics2D.Raycast(startPos,
                    rayDirection,
                    rayDistance,
                    PlatformLayer & ~OneWayPlatformLayer);
            }
            else
            {
                castResult = Physics2D.Raycast(startPos,
                    rayDirection,
                    rayDistance,
                    PlatformLayer | OneWayPlatformLayer);
            }

            startPos.x += step;
            if (!castResult) continue;
            var hitDis = castResult.distance;
            var newMove = hitDis - SkinWidth;
            if (isUp)
            {
                _deltaMove.y = newMove;
                _upHit = true;
                TopPlatform = castResult.collider;
            }
            else
            {
                _deltaMove.y = -newMove;
                _downHit = true;
                BottomPlatform = castResult.collider;
            }

            if (Mathf.Abs(_deltaMove.y) - FloatThreshold <= 0) _deltaMove.y = 0;
            break;
        }
    }

    private void ResetHitState()
    {
        _leftHit = false;
        _rightHit = false;
        _upHit = false;
        _downHit = false;
        TopPlatform = null;
        BottomPlatform = null;
    }
}