using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Springy.Editor.Util
{
    /// <summary>
    /// Manages a list of items stored in Editor/Player prefs
    /// </summary>
    /// <typeparam name="T">The list type</typeparam>
    public class PrefList<T> : IList<T>
    {
        /// <summary>
        /// The name of the key to read/write the list
        /// </summary>
        private readonly string key;

        /// <summary>
        /// The method used to get the preference items
        /// (i.e. <see cref="EditorPrefs.GetInt(string)"/>)
        /// </summary>
        private readonly Func<string, T> getter;

        /// <summary>
        /// The method used to set the preference items
        /// (i.e. <see cref="EditorPrefs.SetInt"/>)
        /// </summary>
        private readonly Action<string, T> setter;

        /// <summary>
        /// Stores the number of items in the list
        /// </summary>
        private readonly Pref<int> count;
        
        public int Count
        {
            get => count.Value;
            private set => count.Value = value;
        }

        /// <summary>
        /// Gets or sets the element at the specified index
        /// </summary>
        /// <param name="i">The index of the element</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="i"/> is out of range</exception>
        public T this[int i]
        {
            get
            {
                if (i >= Count || i < 0) throw new IndexOutOfRangeException();
                return getter(GetIndexKey(i));
            }
            set
            {
                if (i >= Count || i < 0) throw new IndexOutOfRangeException();
                setter(GetIndexKey(i), value);
            }
        }

        public bool IsReadOnly => false;

        /// <summary>
        /// Creates a new PrefList
        /// </summary>
        /// <param name="key">The name of the key to read/write the list</param>
        /// <param name="getter">
        /// The method used to get the preference items
        /// (i.e. <see cref="EditorPrefs.GetInt(string)"/>)
        /// </param>
        /// <param name="setter">
        /// The method used to set the preference items
        /// (i.e. <see cref="EditorPrefs.SetInt"/>)
        /// </param>
        public PrefList(
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

        public void Add(T item)
        {
            setter(GetIndexKey(Count), item);
            Count++;
        }

        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                EditorPrefs.DeleteKey(GetIndexKey(i));
            }

            Count = 0;
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Equals(item)) return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex + i] = this[i];
            }
        }

        public void Insert(int index, T item)
        {
            if (index > Count || index < 0)
                throw new IndexOutOfRangeException();
            
            Count++;

            // shift all elements ahead by one
            for (int i = Count - 1; i > index; i++)
            {
                this[i] = this[i - 1];
            }

            this[index] = item;
        }

        public void RemoveAt(int index)
        {
            if (index >= Count || index < 0)
                throw new IndexOutOfRangeException();

            // shift all elements ahead of index back by one
            for (int j = index; j < Count - 1; j++)
            {
                this[index] = this[index + 1];
            }

            Count--;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            
            if (index >= 0)
            {
                RemoveAt(index);
            }

            return index >= 0;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return getter(GetIndexKey(i));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns the pref key for the item at the provided index
        /// </summary>
        /// <param name="i">The index of the item</param>
        private string GetIndexKey(int i) => key + "/" + i;
    }
}