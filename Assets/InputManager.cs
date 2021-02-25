using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputEventArgs
{
    public readonly KeyBindingInfo Info;
    public readonly KeyBindingData Data;

    public KeyInputEventArgs(in KeyBindingInfo info, KeyBindingData data)
    {
        Info = info;
        Data = data;
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

public readonly struct KeyBindingInfo
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
}

public struct KeyBindingData
{
    public float ElapsedTime;
}

public class KeyBinding
{
    private readonly string _name;
    private readonly KeyBindingInfo _bindingInfo;
    private readonly DelegateListKeyInput _delegateList;
    private KeyBindingData _data;

    public string Name => _name;
    public KeyBindingInfo BindingInfo => _bindingInfo;
    public DelegateListKeyInput DelegateList => _delegateList;
    public ref KeyBindingData Data => ref _data;

    public KeyBinding(string name, in KeyBindingInfo bindingInfo)
    {
        _name = name;
        _bindingInfo = bindingInfo;
        _delegateList = new DelegateListKeyInput();
        _data = new KeyBindingData
        {
            ElapsedTime = 0.0f
        };
    }
}

public class InputManager
{
    private readonly SortedDictionary<string, KeyBinding> _bindingMap;

    public InputManager() { _bindingMap = new SortedDictionary<string, KeyBinding>(); }

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
                        ref var data = ref binding.Data;
                        list.Invoke(new KeyInputEventArgs(in info, data));
                        data.ElapsedTime += deltaTime;
                    }
                    else if (Input.GetKeyUp(info.Keyboard))
                    {
                        binding.Data.ElapsedTime = 0.0f;
                    }

                    break;
                }
                case KeyBindType.Mouse:
                {
                    var btnNum = (int) info.Mouse;
                    if (Input.GetMouseButton(btnNum))
                    {
                        ref var data = ref binding.Data;
                        list.Invoke(new KeyInputEventArgs(in info, data));
                        data.ElapsedTime += deltaTime;
                    }
                    else if (Input.GetMouseButton(btnNum))
                    {
                        binding.Data.ElapsedTime = 0.0f;
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