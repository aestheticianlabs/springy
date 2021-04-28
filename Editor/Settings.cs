using System;
using Springy.Editor.Util;
using UnityEditor;
using UnityEngine;

namespace Springy.Editor
{
    internal class Settings : SettingsProvider
    {
        public const string Prefix = Springy.PackageName;
        
        /// <summary>
        /// Whether to automatically collapse folders
        /// </summary>
        public static readonly SettingsPref<bool> AutoCollapse =
            new SettingsPref<bool>(
                GetPackageKey("autoCollapse"), "Collapse Folders",
                EditorPrefs.GetBool, EditorPrefs.SetBool,
                true
            );

        /// <summary>
        /// Whether to automatically expand pinned folders
        /// </summary>
        public static readonly SettingsPref<bool> ExpandPinned =
            new SettingsPref<bool>(
                GetPackageKey("expandPinned"), "Auto-expand Pinned",
                EditorPrefs.GetBool, EditorPrefs.SetBool,
                true
            );

        private Settings() : base(
            path: "Preferences/Springy",
            scopes: SettingsScope.User
        )
        {
        }

        public override void OnGUI(string searchContext)
        {
            DrawToggleField(AutoCollapse);
            DrawToggleField(ExpandPinned);
            base.OnGUI(searchContext);
        }

        private void DrawToggleField(SettingsPref<bool> pref) => pref.Value =
            EditorGUILayout.Toggle(pref.Name, pref.Value);

        /// <summary>
        /// Returns a fully qualified preferences key for the package
        /// (i.e. com.aela.springy/TheKey)
        /// </summary>
        public static string GetPackageKey(string key) => $"{Prefix}/{key}";

        [SettingsProvider]
        private static SettingsProvider RegisterProvider() => new Settings();
    }
    
    internal class SettingsPref<T> : Pref<T>
    {
        /// <summary>
        /// Human-readable name
        /// </summary>
        public readonly string Name;
        
        public SettingsPref(
            string key, string name,
            Func<string, T, T> getter, Action<string, T> setter,
            T defaultValue = default
        ) : base(key, getter, setter, defaultValue)
        {
            Name = name;
        }
    }
}