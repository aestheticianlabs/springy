using UnityEditor;
using UnityEngine;

namespace Springy.Editor
{
    internal static class AssetDatabaseUtil
    {
        public static bool IsValidFolder(string guid)
        {
            return AssetDatabase.IsValidFolder(
                AssetDatabase.GUIDToAssetPath(guid)
            );
        }

        public static string GUIDFromInstanceID(int instanceID)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(
                instanceID, out var guid, out long _
            );
            return guid;
        }

        public static int InstanceIDFromGUID(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            return asset.GetInstanceID();
        }
    }
}