using System.Linq;
using Springy.Editor.Util;
using UnityEditor;

namespace Springy.Editor
{
    /// <summary>
    /// Holds the menu item action and validation methods
    /// </summary>
    internal static class MenuItems
    {
        private const string ExlcudeMenuOption = "Assets/Pin";
        private const string IncludeMenuOption = "Assets/Unpin";
        
        private const int Priority = 50;
        
        [MenuItem(ExlcudeMenuOption, priority = Priority)]
        private static void DisableSpringyContextMenu()
        {
            foreach (var guid in Selection.assetGUIDs.Except(Settings.Pinned))
            {
                Springy.Pin(guid);
            }
        }

        [MenuItem(IncludeMenuOption, priority = Priority + 1)]
        private static void EnableSpringyContextMenu()
        {
            foreach (var guid in Selection.assetGUIDs.Intersect(Settings.Pinned))
            {
                Springy.Unpin(guid);
            }
        }

        [MenuItem(ExlcudeMenuOption, true)]
        private static bool DisableSpringyValidation()
        {
            // selection may not contain excluded assets or non-folders
            return Selection.assetGUIDs.All(AssetDatabaseUtil.IsValidFolder)
                   && !Selection.assetGUIDs.Intersect(Settings.Pinned).Any();
        }

        [MenuItem(IncludeMenuOption, true)]
        private static bool EnableSpringyValidation()
        {
            // selection may only contain excluded assets
            return Selection.assetGUIDs.Intersect(Settings.Pinned).Any();
        }
    }
}