using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class CreateViewWindow
{
    private const string EDITOR_PREFS_VIEW = "GeneratedViewAsset";

    [MenuItem("Create/View", false, 1)]
    [MenuItem("Assets/Create/UI/View", false, 1)]
    public static void ShowWinow()
    {
        NameObjectWindow.Show("NewView", TryCreateView);
    }

    private static void TryCreateView(string name)
    {
        var type_exists = ExtraEditorUtility.IdentifyScriptType(name) != null;
        if (type_exists)
        {
            Debug.Log("Asset already exists with name: " + name);
            return;
        }

        CreateScript(name);
    }

    private static void CreateScript(string name)
    {
        string path_script = EditorPaths.VIEW_SCRIPTS;
        ExtraEditorUtility.EnsureDirectoryExists(path_script);
        var script_directory = AssetDatabase.LoadAssetAtPath(path_script, typeof(UnityEngine.Object));
        Selection.activeObject = script_directory;
        AssetDatabase.Refresh();

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                    $"{EditorPaths.UI_TEMPLATES}/ViewTemplate.cs.txt",
                    name + ".cs");
        RefocusEditorWindow();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorPrefs.SetString(EDITOR_PREFS_VIEW, name);
        EditorUtility.DisplayProgressBar("Hold on", "Recompiling scripts", 0f);
    }

    private static void RefocusEditorWindow()
    {
        var current_window = EditorWindow.focusedWindow;
        var assembly = typeof(EditorWindow).Assembly;
        var type = assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var hierarchy_window = EditorWindow.GetWindow(type);
        hierarchy_window.Focus();
        current_window.Focus();
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        var asset_name = EditorPrefs.GetString(EDITOR_PREFS_VIEW);
        if(asset_name != "")
        {
            CreateViewPrefab(asset_name);
            EditorPrefs.SetString(EDITOR_PREFS_VIEW, "");
        }

        EditorUtility.ClearProgressBar();
    }

    private static void CreateViewPrefab(string name)
    {
        var asset = AssetDatabase.LoadAssetAtPath<GameObject>($"{EditorPaths.UI_PREFABS}/PrefabView.prefab");
        var prefab = Object.Instantiate(asset);
        prefab.AddComponent(ExtraEditorUtility.IdentifyScriptType(name));

        string dir = EditorPaths.VIEW_PREFABS;
        string path = string.Format("{0}/{1}.prefab", dir, name);
        ExtraEditorUtility.EnsureDirectoryExists(dir);
        PrefabUtility.SaveAsPrefabAsset(prefab, path);
        AssetDatabase.SaveAssets();
        Object.DestroyImmediate(prefab);
    }
}
