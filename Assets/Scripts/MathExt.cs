using System;
using UnityEngine;

public static class MathExt
{
    public const float FloatEpsinon = 0.0000001f;

    public static int GetBinaryLayer(this GameObject gameObject) { return 1 << gameObject.layer; }

    public static bool IsZero(float value) { return Math.Abs(value) < FloatEpsinon; }

    public static bool IsZero(Vector3 vec3) { return IsZero(vec3.x) && IsZero(vec3.y) && IsZero(vec3.z); }
}