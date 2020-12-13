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
using System.Collections.Generic;

namespace KSGFK.Unsafe
{
    public static unsafe class Unsafe
    {
        public static void PushHeap<T>(IList<T> list, IComparer<T> pred)
        {
            var count = list.Count;
            if (count < 0 || count >= int.MaxValue) throw new IndexOutOfRangeException();
            if (count >= 2)
            {
                var uLast = count - 1;
                var val = list[uLast];
                PushHeapByIndex(list, uLast, 0, val, pred);
            }
        }

        private static void PushHeapByIndex<T>(IList<T> list, int hole, int top, T value, IComparer<T> pred)
        {
            for (var idx = (hole - 1) >> 1;
                top < hole && pred.Compare(value, list[idx]) < 0;
                idx = (hole - 1) >> 1)
            {
                list[hole] = list[idx];
                hole = idx;
            }

            list[hole] = value;
        }

        public static void PopHeap<T>(IList<T> list, IComparer<T> pred)
        {
            var count = list.Count;
            if (count < 0 || count >= int.MaxValue) throw new IndexOutOfRangeException();
            if (count >= 2)
            {
                var uLast = count - 1;
                var val = list[uLast];
                list[uLast] = list[0];
                var hole = 0;
                var bottom = uLast;
                var top = hole;
                var idx = hole;
                var maxSequenceNonLeaf = (bottom - 1) >> 1;
                while (idx < maxSequenceNonLeaf)
                {
                    idx = 2 * idx + 2;
                    if (pred.Compare(list[idx - 1], list[idx]) < 0)
                    {
                        --idx;
                    }

                    list[hole] = list[idx];
                    hole = idx;
                }

                if (idx == maxSequenceNonLeaf && bottom % 2 == 0)
                {
                    list[hole] = list[bottom - 1];
                    hole = bottom - 1;
                }

                PushHeapByIndex(list, hole, top, val, pred);
            }
        }
    }
}