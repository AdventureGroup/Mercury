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

namespace KSGFK.Collections
{
    public class PriorityQueue<T>
    {
        private readonly List<T> _data;
        private readonly IComparer<T> _cmp;

        public int Count => _data.Count;
        public int Capacity => _data.Capacity;
        public bool IsEmpty => _data.Count <= 0;

        public PriorityQueue() : this(8, Comparer<T>.Default) { }

        public PriorityQueue(int size) : this(size, Comparer<T>.Default) { }

        public PriorityQueue(int size, IComparer<T> cmp)
        {
            _data = new List<T>(size);
            _cmp = cmp;
        }

        public void Enqueue(T item)
        {
            _data.Add(item);
            Unsafe.Unsafe.PushHeap(_data, _cmp);
        }

        public void Dequeue()
        {
            if (_data.Count <= 0) throw new IndexOutOfRangeException();
            Unsafe.Unsafe.PopHeap(_data, _cmp);
            _data.RemoveAt(_data.Count - 1);
        }

        public T Peek()
        {
            if (_data.Count <= 0) throw new IndexOutOfRangeException();
            return _data[0];
        }

        public void Clear() { _data.Clear(); }

        public void TrimExcess() { _data.TrimExcess(); }
    }
}