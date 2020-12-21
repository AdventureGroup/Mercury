using System;
using System.Collections.Generic;
using KSGFK.Collections;
using UnityEngine;

/// <summary>
/// 世界
/// </summary>
public class World : MonoBehaviour
{
    /// <summary>
    /// 唯一名字
    /// </summary>
    public string WorldName;

    /// <summary>
    /// 世界中的实体生成器
    /// </summary>
    public EntityGenerator[] Generator;

    private LinkedList<Entity> _activeEntity;
    private PriorityQueue<EntityGenerator.Command> _cmdQueue;
    private float _worldTime;

    /// <summary>
    /// 世界中活动的实体
    /// </summary>
    public IReadOnlyCollection<Entity> ActiveEntity => _activeEntity;

    /// <summary>
    /// 世界生成后经过的时间
    /// </summary>
    public float WorldTime => _worldTime;

    private void Awake()
    {
        _activeEntity = new LinkedList<Entity>();
        _cmdQueue = new PriorityQueue<EntityGenerator.Command>();
    }

    private void Update()
    {
        _worldTime += Time.deltaTime;
        EntityGenerator.Command cmd;
        while (!_cmdQueue.IsEmpty && (cmd = _cmdQueue.Peek()).Time >= WorldTime)
        {
            _cmdQueue.Dequeue();
            var entity = CreateEntity(cmd.EntityObject);
            var trans = entity.transform;
            trans.position = cmd.Position;
            trans.rotation = cmd.Rotation;
        }
    }

    /// <summary>
    /// 当世界被加载时调用
    /// </summary>
    public virtual void OnLoad()
    {
        foreach (var gen in Generator)
        {
            foreach (var cmd in gen.GetCommands())
            {
                _cmdQueue.Enqueue(cmd);
            }
        }

        _worldTime = 0;
    }

    /// <summary>
    /// 当世界被卸载时调用
    /// </summary>
    public virtual void OnUnload() { }

    /// <summary>
    /// 创建一个实体
    /// </summary>
    /// <param name="entityObject">预制体</param>
    /// <returns>实体实例</returns>
    public Entity CreateEntity(GameObject entityObject)
    {
        var ins = Instantiate(entityObject);
        if (!ins.TryGetComponent(out Entity e))
        {
            Destroy(ins);
            throw new ArgumentException(nameof(entityObject));
        }

        e.OnCreated();
        _activeEntity.AddLast(e.InternalNode);
        return e;
    }

    /// <summary>
    /// 延时创建一个实体
    /// </summary>
    /// <param name="entityObject">预制体</param>
    /// <param name="delay">延迟时间</param>
    public void CreateEntity(GameObject entityObject, float delay)
    {
        _cmdQueue.Enqueue(new EntityGenerator.Command(entityObject,
            delay + WorldTime,
            Vector3.zero,
            Quaternion.identity));
    }

    /// <summary>
    /// 销毁一个实体
    /// </summary>
    /// <param name="entity">实体实例</param>
    public void DestroyEntity(Entity entity)
    {
        entity.OnDestroyed();
        _activeEntity.Remove(entity.InternalNode);
        Destroy(entity.gameObject);
    }
}