#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

static class SceneAutoLoader
{
    [MenuItem("Scene Autoload/Select Master Scene...")]
    private static void SelectStartupScene()
    {
        string masterScene = EditorUtility.OpenFilePanel(
            "Select Master Scene",
            Application.dataPath,
            "unity"
        );
        masterScene = masterScene.Replace(Application.dataPath, "Assets");
        if (!string.IsNullOrEmpty(masterScene))
        {
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(
                masterScene
            );
        }
    }

    [MenuItem("Scene Autoload/Disable Scene Autoload")]
    private static void DisableSceneAutoload()
    {
        EditorSceneManager.playModeStartScene = null;
    }
}
#endif
