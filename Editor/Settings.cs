using System;
using Springy.Editor.Util;
using UnityEditor;

namespace Springy.Editor
{
    /// <summary>
    /// Manages the settings for the package
    /// </summary>
    internal class Settings : SettingsProvider
    {
        /// <summary>
        /// Prefix used to store package preferences
        /// e.g. com.aela.springy
        /// </summary>
        private const string Prefix = Springy.PackageName;

        /// <summary>
        /// Prefix used to store project-specific preferences
        /// e.g. com.aela.springy/TheCompany.TheProduct
        /// </summary>
        private static string ProjectPrefix = Prefix +
                                              $"/{PlayerSettings.companyName}." +
                                              $"{PlayerSettings.productName}";

        /// <summary>
        /// Whether to automatically collapse folders
        /// </summary>
        public static readonly SettingsPref<bool> AutoCollapse =
            new SettingsPref<bool>(
                AsEditorKey("autoCollapse"), "Collapse Folders",
                EditorPrefs.GetBool, EditorPrefs.SetBool,
                true
            );

        /// <summary>
        /// Whether to automatically expand pinned folders
        /// </summary>
        public static readonly SettingsPref<bool> ExpandPinned =
            new SettingsPref<bool>(
                AsEditorKey("expandPinned"), "Auto-expand Pinned",
                EditorPrefs.GetBool, EditorPrefs.SetBool,
                true
            );

        /// <summary>
        /// Project pinned items
        /// </summary>
        public static readonly PrefList<string> Pinned =
            new PrefList<string>(
                AsProjectKey("exclude"),
                EditorPrefs.GetString, EditorPrefs.SetString
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
        /// Returns a fully qualified preferences key for the
        /// package at the user/editor level
        /// (i.e. com.aela.springy/TheKey)
        /// </summary>
        public static string AsEditorKey(string key) =>
            $"{Prefix}/{key}";

        /// <summary>
        /// Returns a fully qualified preferences key for the
        /// package at the user/editor level
        /// (i.e. com.aela.springy/TheKey)
        /// </summary>
        public static string AsProjectKey(string key) =>
            $"{ProjectPrefix}/{key}";

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