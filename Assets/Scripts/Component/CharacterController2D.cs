using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    /// <summary>
    /// 下一帧是否可以穿过平台
    /// </summary>
    public bool IsIgnorePlatform;

    /// <summary>
    /// 碰撞层
    /// </summary>
    public LayerMask CollideLayer;

    /// <summary>
    /// 可穿越的平台碰撞层
    /// </summary>
    public LayerMask OneWayPlatformLayer;

    /// <summary>
    /// 预留空间
    /// </summary>
    public float SkinWidth = 0.01f;

    /// <summary>
    /// 碰撞迭代次数
    /// </summary>
    public int MaxIterations = 2;

    /// <summary>
    /// 最小的float值
    /// </summary>
    public float FloatEpsilon = (float) 1e-5;

    private Collider2D _coll;
    private Rigidbody2D _rigid;
    private Vector2 _deltaMove;
    private List<RaycastHit2D> _hitBuffer;
    private bool _isLastIgnore;
    private Vector2 _velocity;

    /// <summary>
    /// 当前速度
    /// </summary>
    public Vector2 Velocity => _velocity;

    /// <summary>
    /// 碰撞体
    /// </summary>
    public Collider2D AttachedCollider => _coll;

    /// <summary>
    /// 刚体
    /// </summary>
    public Rigidbody2D AttachedRigidBody => _rigid;

    /// <summary>
    /// 是否和地面碰撞
    /// </summary>
    public bool IsGrounded => AttachedRigidBody.IsTouchingLayers(CollideLayer | OneWayPlatformLayer);

    /// <summary>
    /// 是否和可穿过平台碰撞
    /// </summary>
    public bool IsTouchingOneWayPlatform => AttachedRigidBody.IsTouchingLayers(OneWayPlatformLayer);

    private void Start()
    {
        _coll = GetComponent<Collider2D>() ?? throw new ArgumentException();
        _rigid = GetComponent<Rigidbody2D>() ?? throw new ArgumentException();
        _hitBuffer = new List<RaycastHit2D>();
    }

    private void FixedUpdate()
    {
        if (_deltaMove.sqrMagnitude <= FloatEpsilon) return;
        var direction = _deltaMove.normalized;
        var distanceLen = _deltaMove.magnitude;
        var startPos = AttachedRigidBody.position;
        var endPos = startPos;
        var ground = IsTouchingOneWayPlatform;
        var filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = IsIgnorePlatform || _isLastIgnore && ground
                ? CollideLayer & ~OneWayPlatformLayer
                : CollideLayer | OneWayPlatformLayer
        };
        var maxIterations = MaxIterations;
        while (maxIterations-- > 0 && distanceLen > FloatEpsilon && direction.sqrMagnitude > FloatEpsilon)
        {
            var distance = distanceLen;
            var hitCount = AttachedRigidBody.Cast(direction, filter, _hitBuffer, distance);
            if (hitCount > 0)
            {
                var hit = _hitBuffer[0];
                if (hit.distance > SkinWidth)
                {
                    distance = hit.distance - SkinWidth;
                    endPos += direction * distance;
                }
                else
                {
                    distance = 0f;
                }

                direction -= hit.normal * Vector2.Dot(direction, hit.normal);
            }
            else
            {
                endPos += direction * distance;
            }

            distanceLen -= distance;
        }

        AttachedRigidBody.MovePosition(endPos);
        _velocity = (endPos - startPos) / Time.fixedDeltaTime;
        _deltaMove = Vector2.zero;
        _isLastIgnore = ground && _isLastIgnore || IsIgnorePlatform;
        IsIgnorePlatform = false;
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="deltaMove">路程</param>
    public void Move(Vector2 deltaMove) { _deltaMove += deltaMove; }
}