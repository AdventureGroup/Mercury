using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入事件消息
/// </summary>
public class KeyInputEventArgs
{
    public readonly KeyBindingInfo BindingInfo;

    /// <summary>
    /// 距离第一次输入经过的时间，如果是0说明是这一帧按下的
    /// </summary>
    public readonly float ElapsedTime;

    public KeyInputEventArgs(KeyBindingInfo bindingInfo, float elapsedTime)
    {
        BindingInfo = bindingInfo;
        ElapsedTime = elapsedTime;
    }
}

public delegate void KeyInputAction(KeyInputEventArgs info);

public class DelegateListKeyInput : List<KeyInputAction>
{
    public void Invoke(KeyInputEventArgs info)
    {
        foreach (var i in this)
        {
#if DEBUG
            try
            {
                i.Invoke(info);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
#else
            i.Invoke(info);
#endif
        }
    }
}

public enum KeyBindType
{
    None,
    Keyboard,
    Mouse
}

public enum MouseButton
{
    Left = 0,
    Middle = 2,
    Right = 1
}

public readonly struct KeyBindingInfo : IEquatable<KeyBindingInfo>
{
    public readonly KeyBindType Type;
    public readonly KeyCode Keyboard;
    public readonly MouseButton Mouse;

    public KeyBindingInfo(KeyCode keyboard)
    {
        Type = KeyBindType.Keyboard;
        Keyboard = keyboard;
        Mouse = default;
    }

    public KeyBindingInfo(MouseButton mouse)
    {
        Type = KeyBindType.Mouse;
        Keyboard = default;
        Mouse = mouse;
    }

    public bool Equals(KeyBindingInfo other)
    {
        return Type == other.Type && Keyboard == other.Keyboard && Mouse == other.Mouse;
    }

    public override bool Equals(object obj) { return obj is KeyBindingInfo other && Equals(other); }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int) Type;
            hashCode = (hashCode * 397) ^ (int) Keyboard;
            hashCode = (hashCode * 397) ^ (int) Mouse;
            return hashCode;
        }
    }
}

public class KeyBinding
{
    private readonly string _name;
    private readonly KeyBindingInfo _bindingInfo;
    private readonly DelegateListKeyInput _delegateList;

    public string Name => _name;
    public KeyBindingInfo BindingInfo => _bindingInfo;
    public DelegateListKeyInput DelegateList => _delegateList;

    public KeyBinding(string name, in KeyBindingInfo bindingInfo)
    {
        _name = name;
        _bindingInfo = bindingInfo;
        _delegateList = new DelegateListKeyInput();
    }
}

public class InputManager
{
    private readonly SortedDictionary<string, KeyBinding> _bindingMap;
    private readonly Dictionary<KeyBindingInfo, float> _keyElapsedTime;

    public InputManager()
    {
        _bindingMap = new SortedDictionary<string, KeyBinding>();
        _keyElapsedTime = new Dictionary<KeyBindingInfo, float>();
    }

    public void Update()
    {
        var deltaTime = Time.deltaTime;
        foreach (var pair in _bindingMap)
        {
            var binding = pair.Value;
            var list = binding.DelegateList;
            if (list.Count <= 0)
            {
                continue;
            }

            var info = binding.BindingInfo;
            switch (info.Type)
            {
                case KeyBindType.Keyboard:
                {
                    if (Input.GetKey(info.Keyboard))
                    {
                        var elapsed = _keyElapsedTime[info];
                        list.Invoke(new KeyInputEventArgs(info, elapsed));
                        _keyElapsedTime[info] = elapsed + deltaTime;
                    }
                    else if (Input.GetKeyUp(info.Keyboard))
                    {
                        _keyElapsedTime[info] = 0.0f;
                    }

                    break;
                }
                case KeyBindType.Mouse:
                {
                    var btnNum = (int) info.Mouse;
                    if (Input.GetMouseButton(btnNum))
                    {
                        var elapsed = _keyElapsedTime[info];
                        list.Invoke(new KeyInputEventArgs(info, elapsed));
                        _keyElapsedTime[info] = elapsed + deltaTime;
                    }
                    else if (Input.GetMouseButton(btnNum))
                    {
                        _keyElapsedTime[info] = 0.0f;
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public void AddBinding(string bindingName, in KeyBindingInfo bindingInfo)
    {
        if (bindingInfo.Type == KeyBindType.None)
        {
            throw new ArgumentException($"按键类型不可以是None");
        }

        if (HasBinding(bindingName))
        {
            throw new ArgumentException($"绑定名字应该是唯一的:{bindingName}");
        }

        var binding = new KeyBinding(bindingName, in bindingInfo);
        _bindingMap.Add(bindingName, binding);

        if (!_keyElapsedTime.ContainsKey(bindingInfo))
        {
            _keyElapsedTime.Add(bindingInfo, 0.0f);
        }
    }

    public bool HasBinding(string bindingName) { return _bindingMap.ContainsKey(bindingName); }

    public bool RemoveBinding(string bindingName) { return _bindingMap.Remove(bindingName); }

    public void AddDelegation(string bindingName, KeyInputAction action)
    {
        if (!_bindingMap.TryGetValue(bindingName, out var binding))
        {
            throw new ArgumentException($"未知的绑定:{bindingName}");
        }

        var list = binding.DelegateList;
        if (list.Contains(action))
        {
            Debug.LogWarning("尝试多次绑定同一个委托");
        }

        list.Add(action);
    }

    public bool RemoveDelegation(string bindingName, KeyInputAction action)
    {
        if (!_bindingMap.TryGetValue(bindingName, out var binding))
        {
            throw new ArgumentException($"未知的绑定:{bindingName}");
        }

        var list = binding.DelegateList;
        var index = list.IndexOf(action);
        if (index < 0)
        {
            return false;
        }

        list.RemoveAt(index);
        return true;
    }
}