/*
 * MIT License
 * 
 * Copyright (c) 2020 ksgfk https://github.com/ksgfk/KSGFK.Unsafe
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
#if UNITY_2019_4_OR_NEWER
using UnityEngine;
using MathF = System.Math;

#else
using System.Numerics;
#endif

namespace KSGFK.Unsafe
{
    public readonly struct Aabb2D : IEquatable<Aabb2D>
    {
        public readonly float Left;
        public readonly float Down;
        public readonly float Right;
        public readonly float Up;

        public float Width => Right - Left;
        public float Height => Up - Down;

        public Vector2 RightUp => new Vector2(Right, Up);
        public Vector2 LeftDown => new Vector2(Left, Down);

        public Aabb2D(float left, float down, float right, float up)
        {
            Left = left;
            Down = down;
            Right = right;
            Up = up;
        }

        public bool IsCross(Aabb2D o)
        {
            var left = MathF.Max(Left, o.Left);
            var down = MathF.Max(Down, o.Down);
            var right = MathF.Min(Right, o.Right);
            var up = MathF.Min(Up, o.Up);
            return left < right && down < up;
        }

        public bool Contains(Aabb2D o) { return o.Left >= Left && o.Right <= Right && o.Up <= Up && o.Down >= Down; }

        public override string ToString() { return $"[{new Vector2(Left, Down)},{new Vector2(Right, Up)}]"; }

        public bool Equals(Aabb2D other)
        {
            return MathF.Abs(Left - other.Left) < 0.000001f &&
                   MathF.Abs(Down - other.Down) < 0.000001f &&
                   MathF.Abs(Right - other.Right) < 0.000001f &&
                   MathF.Abs(Up - other.Up) < 0.000001f;
        }

        public override bool Equals(object obj) { return obj is Aabb2D other && Equals(other); }

#if UNITY_2019_4_OR_NEWER
        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Down.GetHashCode() << 2 ^ Right.GetHashCode() >> 2 ^ Up.GetHashCode() >> 1;
        }
#else
        public override int GetHashCode() { return HashCode.Combine(Left, Down, Right, Up); }
#endif

        public static bool operator ==(Aabb2D l, Aabb2D r) { return l.Equals(r); }

        public static bool operator !=(Aabb2D l, Aabb2D r) { return !l.Equals(r); }
    }
}