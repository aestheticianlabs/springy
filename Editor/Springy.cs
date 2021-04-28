using System.Collections.Generic;
using System.Linq;
using Springy.Editor.Util;
using UnityEditor;
using UnityEditorInternal;

namespace Springy.Editor
{
    [InitializeOnLoad]
    public static class Springy
    {
        public const string PackageName = "com.aela.springy";
        public const string PackagePath = "Packages/" + PackageName;

        static Springy()
        {
            EditorApplication.update += EditorUpdate;
        }

        public static bool IsFolderPinned(string guid)
        {
            return Settings.Pinned.Contains(guid);
        }

        public static void Exclude(string guid)
        {
            // todo: probably should throw an error if the guid is not a valid folder
            if (!Settings.Pinned.Contains(guid))
                Settings.Pinned.Add(guid);
        }

        public static void Include(string guid)
        {
            if (Settings.Pinned.Contains(guid))
                Settings.Pinned.Remove(guid);
        }

        private static void EditorUpdate()
        {
            // get selected items and their ancestors
            var selected = GetWithAncestors(
                Selection.instanceIDs.ToList()
            );

            // get pinned items' and their ancestors' instance ids
            var pinnedIDs = GetWithAncestors(
                Settings.Pinned.Select(
                    AssetDatabaseUtil.InstanceIDFromGUID
                )
            );

            // expand all pinned items
            if (Settings.ExpandPinned)
            {
                ExpandPinned(pinnedIDs);
            }

            // auto-collapse unpinned and unselected items
            if (Settings.AutoCollapse)
            {
                AutoCollapse(selected, pinnedIDs);
            }
        }

        private static void ExpandPinned(IEnumerable<int> pinned)
        {
            foreach (var item in pinned)
            {
                ProjectBrowserUtil.ChangeExpandedState(item, true);
            }
        }

        private static void AutoCollapse(
            IEnumerable<int> selected, IEnumerable<int> pinned
        )
        {
            // filter out items that aren't selected or pinned
            var items = InternalEditorUtility.expandedProjectWindowItems;
            var collapse = items.Except(selected).Except(pinned);

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
                source.SelectMany(ProjectWindowUtil.GetAncestors)
            ).Distinct();
        }
    }
}