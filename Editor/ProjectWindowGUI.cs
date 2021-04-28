using UnityEditor;
using UnityEngine;

namespace Springy.Editor
{
    /// <summary>
    /// Manages the project window GUI rendering
    /// </summary>
    [InitializeOnLoad]
    internal static class ProjectWindowGUI
    {
        private const string PinPath = Springy.PackagePath + "/Editor/Icons/pin.png";
        private const int IconSize = 12;
        private const int RightPad = 4;

        private static Texture pin;

        private static Texture Pin
        {
            get
            {
                if (!pin)
                    pin = AssetDatabase.LoadAssetAtPath<Texture>(PinPath);

                return pin;
            }
        }

        private static Texture debug = new Texture2D(12, 12);

        static ProjectWindowGUI()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect rect)
        {
            if (Event.current.type != EventType.Repaint) return;

            // filter for list folders only
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (
                !AssetDatabase.IsValidFolder(path)
                || !IsListAsset(rect)
            ) return;

            // draw pin next to pinned folders
            if (Springy.IsFolderPinned(guid))
            {
                // right aligned on x
                rect.x += rect.width - IconSize - RightPad;

                // middle aligned on y
                rect.y += (rect.height - IconSize) / 2;

                rect.width = IconSize;
                rect.height = IconSize;

                GUI.DrawTexture(rect, Pin, ScaleMode.ScaleToFit);
            }
        }

        private static bool IsListAsset(Rect rect)
        {
            // check if rect is taller than a line (square icon in project window)
            return rect.height <= 20;
        }
    }
}