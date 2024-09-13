using UnityEditor;
using UnityEngine;

public class UnusedAssetFinder : MonoBehaviour
{
    [MenuItem("Assets/Find Unused Assets")]
    static void FindUnusedAssets()
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths();
        foreach (string assetPath in allAssets)
        {
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset != null && !AssetDatabase.IsOpenForEdit(asset))
            {
                string[] dependencies = AssetDatabase.GetDependencies(assetPath);
                if (dependencies.Length == 1) // Means the asset is only dependent on itself
                {
                    Debug.Log("Unused Asset: " + assetPath);
                }
            }
        }
    }
}

