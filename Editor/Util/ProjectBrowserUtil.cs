using System;
using System.Reflection;
using UnityEditor;

namespace Springy.Editor.Util
{
    /// <summary>
    /// Wraps reflected methods/members from
    /// <see cref="UnityEditor.ProjectBrowser"/> and related classes.
    /// </summary>
    internal static class ProjectBrowserUtil
    {
        private static readonly FieldInfo lastInteractedProjectBrowser;
        private static readonly FieldInfo tree;

        private static readonly PropertyInfo treeData;

        private static readonly MethodInfo changeExpandedState;
        private static readonly MethodInfo getItemAndRowIndex;
        private static readonly MethodInfo isExpanded;

        static ProjectBrowserUtil()
        {
            var editorAssembly = typeof(EditorUtility).Assembly;

            // reflect out ProjectBrowser info
            var projectBrowser = editorAssembly.GetType(
                "UnityEditor.ProjectBrowser", true
            );

            lastInteractedProjectBrowser = GetField(
                projectBrowser, "s_LastInteractedProjectBrowser",
                BindingFlags.Public | BindingFlags.Static
            );

            tree = GetField(
                projectBrowser, "m_AssetTree",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            // reflect out TreeViewController info
            var treeViewController = editorAssembly.GetType(
                "UnityEditor.IMGUI.Controls.TreeViewController", true
            );

            treeData = GetProperty(
                treeViewController, "data",
                BindingFlags.Public | BindingFlags.Instance
            );

            changeExpandedState = GetMethod(
                treeViewController, "ChangeExpandedState",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            getItemAndRowIndex = GetMethod(
                treeViewController, "GetItemAndRowIndex",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            // reflect out AssetsTreeViewDataSource info
            var assetsTreeViewDataSource = editorAssembly.GetType(
                "UnityEditor.AssetsTreeViewDataSource", true
            );

            isExpanded = GetMethod(
                assetsTreeViewDataSource, "IsExpanded",
                BindingFlags.Public | BindingFlags.Instance,
                new[] {typeof(int)}
            );
        }

        /// <summary>
        /// Sets the expanded state for the folder with the provided instance ID
        /// </summary>
        /// <param name="id">The instance ID of the folder</param>
        /// <param name="state">The expanded state</param>
        /// <param name="includeChildren">Whether to change children states</param>
        public static void ChangeExpandedState(
            int id, bool state, bool includeChildren = true
        )
        {
            var browser = GetLastInteractedProjectBrowser();

            var tree = GetFolderTree(browser);
            if (tree == null) return;

            var data = GetTreeData(tree);
            if (data == null) return;

            // skip if item is already in desired state
            if (IsExpanded(data, id) == state) return;

            var item = GetTreeItem(tree, id);
            if (item == null) return;

            changeExpandedState.Invoke(
                tree, new[] {item, state, includeChildren}
            );
        }

        /// <summary>
        /// Returns whether the folder with the provided instance ID is expanded
        /// in the provided tree data object
        /// </summary>
        /// <param name="data">The <see cref="UnityEditor.IMGUI.Controls.TreeViewController.data"/> object</param>
        /// <param name="id">The instance ID for the folder</param>
        public static bool IsExpanded(object data, int id)
        {
            return (bool) isExpanded.Invoke(data, new object[] {id});
        }

        /// <summary>
        /// Returns the value of <see cref="UnityEditor.IMGUI.Controls.TreeViewController.data"/>
        /// on the provided <see cref="UnityEditor.IMGUI.Controls.TreeViewController"/>
        /// </summary>
        private static object GetTreeData(object tree)
        {
            return treeData.GetValue(tree);
        }

        /// <summary>
        /// Returns the value of
        /// <see cref="UnityEditor.ProjectBrowser.s_LastInteractedProjectBrowser"/>.
        /// This should be the current project browser window.
        /// </summary>
        private static object GetLastInteractedProjectBrowser()
        {
            return lastInteractedProjectBrowser.GetValue(null);
        }

        /// <summary>
        /// Returns the folder <see cref="UnityEditor.IMGUI.Controls.TreeViewController"/>
        /// object from the provided <see cref="UnityEditor.ProjectBrowser"/>
        /// </summary>
        private static object GetFolderTree(object projectBrowser)
        {
            return tree.GetValue(projectBrowser);
        }

        /// <summary>
        /// Gets the <see cref="UnityEditor.IMGUI.Controls.TreeViewItem"/>
        /// for the asset with the provided instance ID
        /// </summary>
        /// <param name="tree">The <see cref="UnityEditor.IMGUI.Controls.TreeViewController"/> to search</param>
        /// <param name="id">The instance ID of the asset</param>
        private static object GetTreeItem(object tree, int id)
        {
            return getItemAndRowIndex.Invoke(tree, new object[] {id, 0});
        }

        private static FieldInfo GetField(
            Type type, string name, BindingFlags flags = BindingFlags.Default
        )
        {
            var field = type.GetField(name, flags);

            if (field == null)
                throw new Exception($"Couldn't find field {name}");

            return field;
        }

        private static PropertyInfo GetProperty(
            Type type, string name, BindingFlags flags = BindingFlags.Default
        )
        {
            var property = type.GetProperty(name, flags);

            if (property == null)
                throw new Exception($"Couldn't find property {name}");

            return property;
        }

        private static MethodInfo GetMethod(
            Type type, string name, BindingFlags flags = BindingFlags.Default
        )
        {
            var method = type.GetMethod(name, flags);

            if (method == null)
                throw new Exception($"Couldn't find method {name}");

            return method;
        }

        private static MethodInfo GetMethod(
            Type type, string name,
            BindingFlags flags, Type[] types
        )
        {
            var method = type.GetMethod(name, flags, null, types, null);

            if (method == null)
                throw new Exception($"Couldn't find method {name}");

            return method;
        }
    }
}