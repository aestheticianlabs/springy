using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;

namespace Springy.Editor
{
    [InitializeOnLoad]
    public static class Springy
    {
        public const string PackagePath = "Packages/com.aela.springy";
        
        // todo: Better language than "exclude." Maybe "pinned" or "ignored?"
        private static ProjectPrefs.PrefsList<string> exclude;

        /// <summary>
        /// Asset GUIDs that are not auto-collapsed
        /// </summary>
        public static IEnumerable<string> ExcludedGUIDs => exclude;

        static Springy()
        {
            EditorApplication.update += EditorUpdate;
            exclude = ProjectPrefs.GetPrefsList(
                EditorPrefs.GetString, EditorPrefs.SetString, "exclude"
            );
        }

        public static bool IsAssetExcluded(string guid) =>
            exclude.Contains(guid);

        public static void Exclude(string guid)
        {
            // todo: probably should throw an error if the guid is not a valid folder
            if (!exclude.Contains(guid))
                exclude.Add(guid);
        }

        public static void Include(string guid)
        {
            if (exclude.Contains(guid))
                exclude.Remove(guid);
        }

        private static void EditorUpdate()
        {
            // collapses all items that aren't currently selected or a parent of selected

            // get selected items and their ancestors
            var selected = GetWithAncestors(
                Selection.instanceIDs.ToList()
            );

            // get exclude list instance ids
            var excludeIDs = exclude.Select(
                AssetDatabaseUtil.InstanceIDFromGUID
            );

            // filter out items that aren't selected or excluded
            var items = InternalEditorUtility.expandedProjectWindowItems;
            var collapse = items
                .Except(selected)
                .Except(GetWithAncestors(excludeIDs));

            // collapse items
            foreach (var item in collapse)
            {
                ProjectBrowserUtil.ChangeExpandedState(item, false);
            }
        }

        
        private static IEnumerable<int> GetWithAncestors(
            IEnumerable<int> source
        )
        {
            // todo: this probably could be optimized
            return source.Union(
                source.SelectMany(ProjectWindowUtil.GetAncestors).Distinct()
            );
        }
    }
}