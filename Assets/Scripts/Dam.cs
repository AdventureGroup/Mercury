public class DamageClass
{
    /// <summary>
    /// 伤害
    /// </summary>
    public float Damage;
    public struct elem
    {
        public bool Fire;
        public bool Ice;

        public override string ToString()
        {
            return $"火:{Fire} 冰:{Ice}";
        }
    };
    /// <summary>
    /// 包含的属性，可以点出来
    /// </summary>
    public elem Element;
    public int Type;
}
