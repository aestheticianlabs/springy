using System.Linq;
using UnityEditor;

namespace Springy.Editor
{
    internal static class MenuItems
    {
        private const string ExlcudeMenuOption = "Assets/Pin";
        private const string IncludeMenuOption = "Assets/Unpin";
        
        private const int Priority = 50;
        
        [MenuItem(ExlcudeMenuOption, priority = Priority)]
        private static void DisableSpringyContextMenu()
        {
            foreach (var guid in Selection.assetGUIDs.Except(Springy.ExcludedGUIDs))
            {
                Springy.Exclude(guid);
            }
        }

        [MenuItem(IncludeMenuOption, priority = Priority + 1)]
        private static void EnableSpringyContextMenu()
        {
            foreach (var guid in Selection.assetGUIDs.Intersect(Springy.ExcludedGUIDs))
            {
                Springy.Include(guid);
            }
        }

        [MenuItem(ExlcudeMenuOption, true)]
        private static bool DisableSpringyValidation()
        {
            // selection may not contain excluded assets or non-folders
            return Selection.assetGUIDs.All(AssetDatabaseUtil.IsValidFolder)
                   && !Selection.assetGUIDs.Intersect(Springy.ExcludedGUIDs).Any();
        }

        [MenuItem(IncludeMenuOption, true)]
        private static bool EnableSpringyValidation()
        {
            // selection may only contain excluded assets
            return Selection.assetGUIDs.Intersect(Springy.ExcludedGUIDs).Any();
        }
    }
}