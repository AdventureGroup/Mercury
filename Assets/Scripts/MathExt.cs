using UnityEngine;

public static class MathExt
{
    public static int GetBinaryLayer(this GameObject gameObject) { return 1 << gameObject.layer; }
}