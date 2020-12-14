using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图自动实体生成器
/// </summary>
public abstract class EntityGenerator : MonoBehaviour
{
    public readonly struct Command : IComparable<Command>
    {
        public readonly GameObject EntityObject;
        public readonly float Time;
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public Command(GameObject entityObject, float time, Vector3 position, Quaternion rotation)
        {
            EntityObject = entityObject;
            Time = time;
            Position = position;
            Rotation = rotation;
        }

        public int CompareTo(Command other) { return Time.CompareTo(other.Time); }
    }

    public abstract IEnumerable<Command> GetCommands();
}