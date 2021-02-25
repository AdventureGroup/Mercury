using System;
using System.Collections.Generic;
using UnityEngine;

public readonly struct KeyInputEventArgs
{
    public readonly KeyBindingInfo Info;
    public readonly float ElapsedTime;
    public readonly Vector3 NowMousePos;
    public readonly Vector3 MousePosDelta;
    public readonly bool IsKeyRelease;

    public KeyInputEventArgs(in KeyBindingInfo info, float elapsedTime, bool isKeyRelease)
    {
        Info = info;
        ElapsedTime = elapsedTime;
        IsKeyRelease = isKeyRelease;
        NowMousePos = default;
        MousePosDelta = default;
    }

    public KeyInputEventArgs(in KeyBindingInfo info, Vector3 nowPos, Vector3 delta)
    {
        Info = info;
        ElapsedTime = 0;
        NowMousePos = nowPos;
        MousePosDelta = delta;
        IsKeyRelease = default;
    }
}

public delegate void KeyInputAction(in KeyInputEventArgs info);

public class DelegateListKeyInput : List<KeyInputAction>
{
    public void Invoke(in KeyInputEventArgs info)
    {
        foreach (var i in this)
        {
#if DEBUG
            try
            {
                i.Invoke(in info);
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
    MouseClick,
    MousePos
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
    public readonly bool IsMouseMoveCall;

    private KeyBindingInfo(KeyCode keyboard)
    {
        Type = KeyBindType.Keyboard;
        Keyboard = keyboard;
        Mouse = default;
        IsMouseMoveCall = false;
    }

    private KeyBindingInfo(MouseButton mouse)
    {
        Type = KeyBindType.MouseClick;
        Keyboard = default;
        Mouse = mouse;
        IsMouseMoveCall = false;
    }

    private KeyBindingInfo(bool isMouseMoveCall)
    {
        Type = KeyBindType.MousePos;
        Keyboard = default;
        Mouse = default;
        IsMouseMoveCall = isMouseMoveCall;
    }

    public static KeyBindingInfo CreateKeyboard(KeyCode keyboard) { return new KeyBindingInfo(keyboard); }

    public static KeyBindingInfo CreateMouseClick(MouseButton mouse) { return new KeyBindingInfo(mouse); }

    public static KeyBindingInfo CreateMousePos(bool isMouseMove) { return new KeyBindingInfo(isMouseMove); }
}

public struct KeyBindingData
{
    public float ElapsedTime;
    public Vector3 MousePosLastFrame;
}

public class KeyBinding
{
    private readonly string _name;
    private readonly KeyBindingInfo _bindingInfo;
    private readonly DelegateListKeyInput _delegateList;
    private readonly DelegateListKeyInput _releaseKeyList;
    private KeyBindingData _data;

    public string Name => _name;
    public KeyBindingInfo BindingInfo => _bindingInfo;
    public DelegateListKeyInput DelegateList => _delegateList;
    public DelegateListKeyInput ReleaseKeyList => _releaseKeyList;
    public ref KeyBindingData Data => ref _data;

    public KeyBinding(string name, in KeyBindingInfo bindingInfo)
    {
        _name = name;
        _bindingInfo = bindingInfo;
        _delegateList = new DelegateListKeyInput();
        _releaseKeyList = new DelegateListKeyInput();
        _data = new KeyBindingData
        {
            ElapsedTime = 0.0f
        };
    }
}

public class InputManager
{
    private readonly List<KeyBinding> _bindingList;

    public InputManager() { _bindingList = new List<KeyBinding>(); }

    public void Update()
    {
        var deltaTime = Time.deltaTime;
        foreach (var binding in _bindingList)
        {
            var list = binding.DelegateList;
            var releaseList = binding.ReleaseKeyList;
            if (list.Count <= 0 && releaseList.Count <= 0)
            {
                continue;
            }

            var info = binding.BindingInfo;
            ref var data = ref binding.Data;
            switch (info.Type)
            {
                case KeyBindType.Keyboard:
                {
                    if (Input.GetKey(info.Keyboard))
                    {
                        var args = new KeyInputEventArgs(in info, data.ElapsedTime, false);
                        list.Invoke(in args);
                        data.ElapsedTime += deltaTime;
                    }
                    else if (Input.GetKeyUp(info.Keyboard))
                    {
                        var args = new KeyInputEventArgs(in info, data.ElapsedTime, true);
                        releaseList.Invoke(in args);
                        data.ElapsedTime = 0.0f;
                    }

                    break;
                }
                case KeyBindType.MouseClick:
                {
                    var btnNum = (int) info.Mouse;
                    if (Input.GetMouseButton(btnNum))
                    {
                        var args = new KeyInputEventArgs(in info, data.ElapsedTime, false);
                        list.Invoke(in args);
                        data.ElapsedTime += deltaTime;
                    }
                    else if (Input.GetMouseButtonUp(btnNum))
                    {
                        var args = new KeyInputEventArgs(in info, data.ElapsedTime, true);
                        releaseList.Invoke(in args);
                        data.ElapsedTime = 0.0f;
                    }

                    break;
                }
                case KeyBindType.MousePos:
                {
                    var nowPos = Input.mousePosition;
                    var mouseMoveDelta = nowPos - data.MousePosLastFrame;
                    var args = new KeyInputEventArgs(in info, nowPos, mouseMoveDelta);
                    if (info.IsMouseMoveCall)
                    {
                        if (!MathExt.IsZero(mouseMoveDelta))
                        {
                            list.Invoke(in args);
                        }
                    }
                    else
                    {
                        list.Invoke(in args);
                    }

                    data.MousePosLastFrame = nowPos;
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
        _bindingList.Add(binding);
    }

    public bool HasBinding(string bindingName) { return _bindingList.Exists(bind => bind.Name == bindingName); }

    public bool RemoveBinding(string bindingName)
    {
        var index = _bindingList.FindIndex(bind => bind.Name == bindingName);
        if (index < 0)
        {
            return false;
        }

        _bindingList.RemoveAt(index);
        return true;
    }

    public void AddDelegation(string bindingName, KeyInputAction action, bool isReleaseAction = false)
    {
        var index = _bindingList.FindIndex(bind => bind.Name == bindingName);
        if (index < 0)
        {
            throw new ArgumentException($"未知的绑定:{bindingName}");
        }

        if (_bindingList[index].BindingInfo.Type == KeyBindType.MousePos && isReleaseAction)
        {
            throw new ArgumentException($"鼠标位置没有按键松开事件");
        }

        var list = isReleaseAction ? _bindingList[index].ReleaseKeyList : _bindingList[index].DelegateList;
        if (list.Contains(action))
        {
            Debug.LogWarning("尝试多次绑定同一个委托");
        }

        list.Add(action);
    }

    public bool RemoveDelegation(string bindingName, KeyInputAction action)
    {
        var index = _bindingList.FindIndex(bind => bind.Name == bindingName);
        if (index < 0)
        {
            throw new ArgumentException($"未知的绑定:{bindingName}");
        }

        var list = _bindingList[index].DelegateList;
        var delegateIndex = list.IndexOf(action);
        if (delegateIndex >= 0)
        {
            list.RemoveAt(delegateIndex);
        }

        var release = _bindingList[index].ReleaseKeyList;
        var releaseIndex = release.IndexOf(action);
        if (releaseIndex >= 0)
        {
            release.RemoveAt(releaseIndex);
        }

        return delegateIndex >= 0 || releaseIndex >= 0;
    }

    public void Clear() { _bindingList.Clear(); }
}