using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    public bool IsIgnorePlatform;
    public LayerMask PlatformLayer;
    public LayerMask OneWayPlatformLayer;
    public float SkinWidth = 0.01f;
    public int MaxIterations = 2;
    public float FloatEpsilon = (float) 1e-5;

    private Collider2D _coll;
    private Rigidbody2D _rigid;
    private Vector2 _deltaMove;
    private List<RaycastHit2D> _moveRaycast;
    private bool _isLastIgnore;

    public Vector2 Velocity => _rigid.velocity;
    public Collider2D HandledCollider => _coll;
    public Rigidbody2D HandledRigidBody => _rigid;
    public bool IsGrounded => HandledRigidBody.IsTouchingLayers(PlatformLayer | OneWayPlatformLayer);

    private void Start()
    {
        _coll = GetComponent<Collider2D>() ?? throw new ArgumentException();
        _rigid = GetComponent<Rigidbody2D>() ?? throw new ArgumentException();
        _moveRaycast = new List<RaycastHit2D>();
    }

    private void FixedUpdate()
    {
        if (_deltaMove.sqrMagnitude <= FloatEpsilon) return;
        var direction = _deltaMove.normalized;
        var distanceLen = _deltaMove.magnitude;
        var startPos = HandledRigidBody.position;
        var endPos = startPos;
        var ground = IsGrounded;
        var filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = IsIgnorePlatform || _isLastIgnore && ground
                ? PlatformLayer & ~OneWayPlatformLayer
                : PlatformLayer | OneWayPlatformLayer
        };
        var maxIterations = MaxIterations;
        while (maxIterations-- > 0 && distanceLen > FloatEpsilon && direction.sqrMagnitude > FloatEpsilon)
        {
            var distance = distanceLen;
            var hitCount = HandledRigidBody.Cast(direction, filter, _moveRaycast, distance);
            if (hitCount > 0)
            {
                var hit = _moveRaycast[0];
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

        HandledRigidBody.MovePosition(endPos);
        _deltaMove = Vector2.zero;
        _isLastIgnore = ground && _isLastIgnore || IsIgnorePlatform;
        IsIgnorePlatform = false;
    }

    public void Move(Vector2 deltaMove) { _deltaMove += deltaMove; }
}