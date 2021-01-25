using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class Entity : MonoBehaviour
{
    private Collider2D _coll;
    private LinkedListNode<Entity> _node;
    public String Camp = "Noon";

    public Collider2D Collider => _coll;
    public LinkedListNode<Entity> InternalNode => _node;

    /// <summary>
    /// 实体被销毁时的事件
    /// </summary>
    public event Action Destroyed;

    private void Awake()
    {
        _node = new LinkedListNode<Entity>(this);
        _coll = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Entity")) OnTouchOtherEntity(other.collider.GetComponent<Entity>());
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("Entity")) OnTouchOtherEntity(other.collider.GetComponent<Entity>());
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Entity")) OnStopTouchOtherEntity(other.collider.GetComponent<Entity>());
    }

    /// <summary>
    /// 当接触到某个实体时触发方法
    /// </summary>
    protected virtual void OnTouchOtherEntity(Entity other) { }

    /// <summary>
    /// 当不再接触某个实体时触发方法
    /// </summary>
    protected virtual void OnStopTouchOtherEntity(Entity other) { }

    /// <summary>
    /// 当实体被创建时触发方法
    /// </summary>
    public virtual void OnCreated() { }

    /// <summary>
    /// 当实体被销毁时触发方法
    /// </summary>
    public virtual void OnDestroyed() { Destroyed?.Invoke(); }
}