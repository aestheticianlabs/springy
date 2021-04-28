using System;
using System.Reflection;
using UnityEditor;

namespace Springy.Editor.Util
{
    internal static class ProjectBrowserUtil
    {
        private static readonly Type projectBrowser;
        private static readonly Type treeViewController;
        private static readonly Type assetsTreeViewDataSource;

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
            projectBrowser = editorAssembly.GetType(
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
            treeViewController = editorAssembly.GetType(
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
            assetsTreeViewDataSource = editorAssembly.GetType(
                "UnityEditor.AssetsTreeViewDataSource", true
            );

            isExpanded = GetMethod(
                assetsTreeViewDataSource, "IsExpanded",
                BindingFlags.Public | BindingFlags.Instance,
                new [] { typeof(int) }
            );
        }

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

        public static bool IsExpanded(object data, int id)
        {
            return (bool) isExpanded.Invoke(data, new object[] {id});
        }

        private static object GetTreeData(object tree)
        {
            return treeData.GetValue(tree);
        }

        private static object GetLastInteractedProjectBrowser()
        {
            return lastInteractedProjectBrowser.GetValue(null);
        }

        private static object GetFolderTree(object projectBrowser)
        {
            return tree.GetValue(projectBrowser);
        }

        private static object GetTreeItem(object tree, int id)
        {
            int row = 0;
            return getItemAndRowIndex.Invoke(tree, new object[] {id, row});
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