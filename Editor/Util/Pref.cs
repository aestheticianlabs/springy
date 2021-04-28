using System;

namespace Springy.Editor.Util
{
    /// <summary>
    /// Manages access to an Editor/Player pref
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Pref<T>
    {
        /// <summary>
        /// The name of the key to read/write the pref
        /// </summary>
        public readonly string Key;
        
        /// <summary>
        /// The method used to get the pref item
        /// (i.e. <see cref="UnityEditor.EditorPrefs.GetInt(string)"/>)
        /// </summary>
        protected readonly Func<string, T, T> getter;
        
        /// <summary>
        /// The method used to set the pref item
        /// (i.e. <see cref="UnityEditor.EditorPrefs.SetInt"/>)
        /// </summary>
        protected readonly Action<string, T> setter;
        
        /// <summary>
        /// The default value if the pref does not exist
        /// </summary>
        protected readonly T defaultValue;

        /// <summary>
        /// The value of this pref
        /// </summary>
        public T Value
        {
            get => getter(Key, defaultValue);
            set => setter(Key, value);
        }
        
        /// <summary>
        /// Creates a new pref
        /// </summary>
        /// <param name="key">The name of the key to read/write the pref</param>
        /// <param name="getter">
        /// The method used to get the pref item
        /// (i.e. <see cref="UnityEditor.EditorPrefs.GetInt(string)"/>)
        /// </param>
        /// <param name="setter">
        /// The method used to set the pref item
        /// (i.e. <see cref="UnityEditor.EditorPrefs.SetInt"/>)
        /// </param>
        /// <param name="defaultValue"></param>
        public Pref(
            string key, 
            Func<string, T, T> getter, 
            Action<string, T> setter,
            T defaultValue = default
        )
        {
            Key = key;
            this.getter = getter;
            this.setter = setter;
            this.defaultValue = defaultValue;
        }

        public static implicit operator T(Pref<T> pref) => pref.Value;
    }
}