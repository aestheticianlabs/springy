using System;
using System.Reflection;
using UnityEditor;

namespace Springy.Editor
{
    internal static class ProjectBrowserUtil
    {
        private static Type projectBrowserType;
        private static Type treeViewControllerType;

        private static FieldInfo lastInteractedProjectBrowser;
        private static FieldInfo treeField;
        private static MethodInfo changeExpandedState;
        private static MethodInfo getItemAndRowIndex;

        static ProjectBrowserUtil()
        {
            var editorAssembly = typeof(EditorUtility).Assembly;

            // reflect out ProjectBrowser info
            projectBrowserType = editorAssembly.GetType(
                "UnityEditor.ProjectBrowser", true
            );

            lastInteractedProjectBrowser = GetField(
                projectBrowserType, "s_LastInteractedProjectBrowser",
                BindingFlags.Public | BindingFlags.Static
            );

            treeField = GetField(
                projectBrowserType, "m_AssetTree",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            // reflect out TreeViewController info
            treeViewControllerType = editorAssembly.GetType(
                "UnityEditor.IMGUI.Controls.TreeViewController", true
            );

            changeExpandedState = GetMethod(
                treeViewControllerType, "ChangeExpandedState",
                BindingFlags.NonPublic | BindingFlags.Instance
            );

            getItemAndRowIndex = GetMethod(
                treeViewControllerType, "GetItemAndRowIndex",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
        }

        public static void ChangeExpandedState(
            int id, bool state, bool includeChildren = true
        )
        {
            var browser = GetLastInteractedProjectBrowser();
            var tree = GetFolderTree(browser);
            if (tree == null) return;

            var item = GetTreeItem(tree, id);
            if (item == null) return;

            changeExpandedState.Invoke(
                tree, new object[] {item, state, includeChildren}
            );
        }

        private static object GetLastInteractedProjectBrowser()
        {
            return lastInteractedProjectBrowser.GetValue(null);
        }

        private static object GetFolderTree(object projectBrowser)
        {
            return treeField.GetValue(projectBrowser);
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

        private static MethodInfo GetMethod(
            Type type, string name, BindingFlags flags = BindingFlags.Default
        )
        {
            var method = type.GetMethod(name, flags);

            if (method == null)
                throw new Exception($"Couldn't find method {name}");

            return method;
        }
    }
}