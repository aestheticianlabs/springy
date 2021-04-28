using UnityEditor;
using UnityEngine;

namespace Springy.Editor.Util
{
    internal static class AssetDatabaseUtil
    {
        /// <summary>
        /// Returns whether the provided asset GUID is a valid folder
        /// </summary>
        public static bool IsValidFolder(string guid)
        {
            return AssetDatabase.IsValidFolder(
                AssetDatabase.GUIDToAssetPath(guid)
            );
        }

        /// <summary>
        /// Returns the GUID for the asset with the provided instance ID
        /// </summary>
        public static string GUIDFromInstanceID(int instanceID)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(
                instanceID, out var guid, out long _
            );
            return guid;
        }

        /// <summary>
        /// Returns the instance ID for the asset with the provided GUID
        /// </summary>
        public static int InstanceIDFromGUID(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            return asset.GetInstanceID();
        }
    }
}