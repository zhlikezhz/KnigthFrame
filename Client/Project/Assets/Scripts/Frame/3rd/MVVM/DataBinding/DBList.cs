using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

namespace Huge.MVVM
{
    public class DBList<T> : DBType where T : DBType
    {
        List<T> m_Value;
        public Action<DBList<T>> indexOrCountChanged = null;

        public DBList()
        {
            m_Value = new List<T>();
        }

        public DBList(int capacity)
        {
            m_Value = new List<T>(capacity);
        }

        public DBList(IEnumerable<T> collection)
        {
            m_Value = new List<T>(collection);
        }

        internal override void InvokeChange()
        {
            IsDirty = false;
            indexOrCountChanged?.Invoke(this);
        }

        public T this[int index] 
        { 
            get { return m_Value[index]; } 
            set
            {
                m_Value[index] = value;
                IsDirty = true;
            }
        }
        public int Count => m_Value.Count;
        public int Capacity => m_Value.Capacity;

        public void Add(T item)
        {
            m_Value.Add(item);
            IsDirty = true;
        }
        public void AddRange(IEnumerable<T> collection)
        {
            m_Value.AddRange(collection);
            IsDirty = true;
        }

        //不支持
        // public ReadOnlyCollection<T> AsReadOnly();

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer) => m_Value.BinarySearch(index, count, item, comparer);
        public int BinarySearch(T item) => m_Value.BinarySearch(item);
        public int BinarySearch(T item, IComparer<T> comparer) => m_Value.BinarySearch(item, comparer);

        public void Clear()
        {
            if (m_Value.Count > 0)
            {
                m_Value.Clear();
                IsDirty = true;
            }
        }

        public bool Contains(T item) => m_Value.Contains(item);

        //不支持
        // public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter);

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Value.CopyTo(array, arrayIndex);
            IsDirty = true;
        }
        public void CopyTo(T[] array)
        {
            m_Value.CopyTo(array);
            IsDirty = true;
        }
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            m_Value.CopyTo(index, array, arrayIndex, count);
            IsDirty = true;
        }
        public bool Exists(Predicate<T> match) => m_Value.Exists(match);
        public T Find(Predicate<T> match) => m_Value.Find(match);
        public List<T> FindAll(Predicate<T> match) => m_Value.FindAll(match);
        public int FindIndex(int startIndex, int count, Predicate<T> match) => m_Value.FindIndex(startIndex, count, match);
        public int FindIndex(int startIndex, Predicate<T> match) => m_Value.FindIndex(startIndex, match);
        public int FindIndex(Predicate<T> match) => m_Value.FindIndex(match);
        public T FindLast(Predicate<T> match) => m_Value.FindLast(match);
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => m_Value.FindLastIndex(startIndex, count, match);
        public int FindLastIndex(int startIndex, Predicate<T> match) => m_Value.FindLastIndex(startIndex, match);
        public int FindLastIndex(Predicate<T> match) => m_Value.FindLastIndex(match);
        public void ForEach(Action<T> action) => m_Value.ForEach(action);
        public List<T>.Enumerator GetEnumerator() => m_Value.GetEnumerator();
        public List<T> GetRange(int index, int count) => m_Value.GetRange(index, count);
        public int IndexOf(T item, int index, int count) => m_Value.IndexOf(item, index, count);
        public int IndexOf(T item, int index) => m_Value.IndexOf(item, index);
        public int IndexOf(T item) => m_Value.IndexOf(item);
        public void Insert(int index, T item)
        {
            m_Value.Insert(index, item);
            IsDirty = true;
        }
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            m_Value.InsertRange(index, collection);
            IsDirty = true;
        }
        public int LastIndexOf(T item) => m_Value.LastIndexOf(item);
        public int LastIndexOf(T item, int index) => m_Value.LastIndexOf(item, index);
        public int LastIndexOf(T item, int index, int count) => m_Value.LastIndexOf(item, index, count);
        public bool Remove(T item)
        {
            var ret = m_Value.Remove(item);
            IsDirty = true;
            return ret;
        }
        public int RemoveAll(Predicate<T> match)
        {
            var ret = m_Value.RemoveAll(match);
            IsDirty = true;
            return ret;
        }
        public void RemoveAt(int index)
        {
            if (index < m_Value.Count)
            {
                m_Value.RemoveAt(index);
                IsDirty = true;
            }
        }
        public void RemoveRange(int index, int count)
        {
            if (index + count <= m_Value.Count)
            {
                m_Value.RemoveRange(index, count);
                IsDirty = true;
            }
        }
        public void Reverse(int index, int count)
        {
            if (index + count <= m_Value.Count)
            {
                m_Value.Reverse(index, count);
                IsDirty = true;
            }
        }
        public void Reverse()
        {
            m_Value.Reverse();
            IsDirty = true;
        }
        public void Sort(Comparison<T> comparison)
        {
            m_Value.Sort(comparison);
            IsDirty = true;
        }
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            m_Value.Sort(index, count, comparer);
            IsDirty = true;
        }
        public void Sort()
        {
            m_Value.Sort();
            IsDirty = true;
        }
        public void Sort(IComparer<T> comparer)
        {
            m_Value.Sort(comparer);
            IsDirty = true;
        }

        public T[] ToArray() => m_Value.ToArray();
        public void TrimExcess() => m_Value.TrimExcess();
        public bool TrueForAll(Predicate<T> match) => m_Value.TrueForAll(match);
    }
}