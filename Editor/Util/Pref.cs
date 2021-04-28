using System;

namespace Springy.Editor.Util
{
    internal class Pref<T>
    {
        public readonly string Key;
        protected readonly Func<string, T, T> getter;
        protected readonly Action<string, T> setter;
        protected readonly T defaultValue;

        public T Value
        {
            get => getter(Key, defaultValue);
            set => setter(Key, value);
        }
        
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