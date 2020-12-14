using System;

/// <summary>
/// 包装可能是null的引用类型
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct MayNull<T> where T : class
{
    private readonly T _ins;

    public T Value
    {
        get
        {
            if (_ins == null)
            {
                throw new ArgumentNullException();
            }

            return _ins;
        }
    }

    public bool HasValue => _ins != null;

    public MayNull(T instance) { _ins = instance; }

    public static implicit operator T(MayNull<T> value) { return value.Value; }
}