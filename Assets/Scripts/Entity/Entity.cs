using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class Entity : MonoBehaviour
{
    private Collider2D _coll;
    private LinkedListNode<Entity> _node;

    public Collider2D Collider => _coll;
    public LinkedListNode<Entity> InternalNode => _node;

    /// <summary>
    /// 实体被销毁时的事件
    /// </summary>
    public event Action Destroyed;

    private void Start()
    {
        _node = new LinkedListNode<Entity>(this);
        _coll = GetComponent<Collider2D>();
        _coll.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Entity")) OnTouchOtherEntity(other.GetComponent<Entity>());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Entity")) OnTouchOtherEntity(other.GetComponent<Entity>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Entity")) OnStopTouchOtherEntity(other.GetComponent<Entity>());
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