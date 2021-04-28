using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Springy.Editor.Util
{
    /// <summary>
    /// Manages project-specific editor preferences
    /// </summary>
    internal static class ProjectPrefs
    {
        /// <summary>
        /// Prefix used to store project-specific preferences
        /// </summary>
        private static readonly string prefix;

        static ProjectPrefs()
        {
            // e.g. Springy/TheCompany.TheProduct
            prefix = $"Springy/" +
                     $"{PlayerSettings.companyName}." +
                     $"{PlayerSettings.productName}";
        }

        /// <summary>
        /// Uses the provided <see cref="EditorPrefs"/> method to get the value
        /// for the provided key.
        /// </summary>
        /// <param name="editorPrefsGetter">
        /// The <see cref="EditorPrefs"/> method to use when getting the pref value
        /// </param>
        /// <param name="key">The pref key</param>
        public static T GetValue<T>(
            Func<string, T> editorPrefsGetter, string key
        )
        {
            return editorPrefsGetter(GetFullKey(key));
        }

        /// <summary>
        /// Uses the provided <see cref="EditorPrefs"/> method to set the value
        /// for the provided key.
        /// </summary>
        /// <param name="editorPrefsSetter">
        /// The <see cref="EditorPrefs"/> method to use when setting the pref value
        /// </param>
        /// <param name="key">The pref key</param>
        /// <param name="value">The value to set</param>
        public static void SetValue<T>(
            Action<string, T> editorPrefsSetter, string key, T value
        )
        {
            editorPrefsSetter(GetFullKey(key), value);
        }

        /// <summary>
        /// Returns a <see cref="PrefsList{T}"/> for the provided key and getter/setters
        /// </summary>
        /// <param name="getter">The <see cref="EditorPrefs"/> getter method</param>
        /// <param name="setter">The <see cref="EditorPrefs"/> setter method</param>
        /// <param name="key">The pref list key</param>
        public static PrefsList<T> GetPrefsList<T>(
            Func<string, T> getter, Action<string, T> setter, string key
        )
        {
            return new PrefsList<T>(key, getter, setter);
        }

        /// <summary>
        /// Returns whether or not the project prefs contains the provided key
        /// </summary>
        public static bool HasKey(string key) =>
            EditorPrefs.HasKey(GetFullKey(key));

        /// <summary>
        /// Returns <paramref name="key"/> with the project prefix added
        /// </summary>
        private static string GetFullKey(string key) => $"{prefix}/{key}";

        public class PrefsList<T> : IEnumerable<T>
        {
            private readonly string key;
            private readonly string countKey;
            private readonly Func<string, T> getter;
            private readonly Action<string, T> setter;

            public T this[int i]
            {
                get => ElementAt(i);
                set => Set(i, value);
            }

            private int Count
            {
                get => GetValue(EditorPrefs.GetInt, countKey);
                set => SetValue(EditorPrefs.SetInt, countKey, value);
            }

            public PrefsList(
                string key,
                Func<string, T> getter,
                Action<string, T> setter
            )
            {
                this.key = key;
                countKey = key + "/Count";

                this.getter = getter;
                this.setter = setter;

                if (!HasKey(countKey))
                {
                    SetValue(EditorPrefs.SetInt, countKey, 0);
                }
            }

            public T ElementAt(int i)
            {
                if (i >= Count || i < 0) throw new IndexOutOfRangeException();
                return GetValue(getter, GetIndexKey(i));
            }

            public void Add(T value)
            {
                SetValue(setter, GetIndexKey(Count), value);
                Count++;
            }

            public void Set(int i, T value)
            {
                if (i >= Count || i < 0) throw new IndexOutOfRangeException();
                SetValue(setter, GetIndexKey(i), value);
            }

            public void RemoveAt(int i)
            {
                if (i >= Count || i < 0) throw new IndexOutOfRangeException();

                // shift all elements ahead of index back by one
                for (int j = i; j < Count - 1; j++)
                {
                    Set(i, ElementAt(i + 1));
                }

                Count--;
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
                for (int i = 0; i < Count; i++)
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
                for (int i = 0; i < Count; i++)
                {
                    yield return GetValue(getter, GetIndexKey(i));
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private string GetIndexKey(int i) => key + "/" + i;
        }
    }
}