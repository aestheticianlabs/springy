using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Springy.Editor.Util
{
    public class ListPref<T> : IEnumerable<T>
    {
        private readonly string key;
        private readonly Func<string, T> getter;
        private readonly Action<string, T> setter;

        public T this[int i]
        {
            get => ElementAt(i);
            set => Set(i, value);
        }

        private readonly Pref<int> count;

        public ListPref(
            string key,
            Func<string, T> getter,
            Action<string, T> setter
        )
        {
            this.key = key;
            this.getter = getter;
            this.setter = setter;

            count = new Pref<int>(
                key + "/Count",
                EditorPrefs.GetInt, EditorPrefs.SetInt
            );
        }

        public T ElementAt(int i)
        {
            if (i >= count || i < 0) throw new IndexOutOfRangeException();
            return getter(GetIndexKey(i));
        }

        public void Add(T value)
        {
            setter(GetIndexKey(count), value);
            count.Value++;
        }

        public void Set(int i, T value)
        {
            if (i >= count || i < 0) throw new IndexOutOfRangeException();
            setter(GetIndexKey(i), value);
        }

        public void RemoveAt(int i)
        {
            if (i >= count || i < 0) throw new IndexOutOfRangeException();

            // shift all elements ahead of index back by one
            for (int j = i; j < count - 1; j++)
            {
                Set(i, ElementAt(i + 1));
            }

            count.Value--;
        }

        public void Remove(T value)
        {
            var index = IndexOf(value);
            if (index >= 0)
            {
                RemoveAt(index);
            }
            else
            {
                throw new Exception($"No element {value} in list");
            }
        }

        public int IndexOf(T value)
        {
            for (int i = 0; i < count; i++)
            {
                if (ElementAt(i).Equals(value))
                {
                    return i;
                }
            }

            return -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return getter(GetIndexKey(i));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string GetIndexKey(int i) => key + "/" + i;
    }
}