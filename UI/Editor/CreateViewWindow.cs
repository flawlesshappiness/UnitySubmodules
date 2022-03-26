using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateViewWindow : EditorWindow
{
    private static EditorWindow window;
    private static string view_filename = "NewView";

    private const string EDITOR_PREFS_VIEW = "GeneratedViewAsset";

    [MenuItem("Assets/Create/UI/New View", false, 1)]
    public static void ShowWinow()
    {
        if(window != null)
        {
            // Window already open
            return;
        }

        window = GetWindow<CreateViewWindow>();
        window.titleContent = new GUIContent("Create View");
        window.minSize = new Vector2(250, 60);
        window.maxSize = window.minSize;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        ExtraEditorUtility.FittedLabel("Name: ");
        view_filename = EditorGUILayout.TextField(view_filename);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create", GUILayout.Height(40)))
        {
            TryCreateView(view_filename);
            window.Close();
        }
    }

    private void TryCreateView(string name)
    {
        var type_exists = ExtraEditorUtility.IdentifyScriptType(name) != null;
        if (type_exists)
        {
            Debug.Log("Asset already exists with name: " + name);
            return;
        }

        CreateScript(name);
    }

    private void CreateScript(string name)
    {
        string path_script = "Assets/Scripts/Views";
        ExtraEditorUtility.EnsureDirectoryExists(path_script);
        var script_directory = AssetDatabase.LoadAssetAtPath(path_script, typeof(UnityEngine.Object));
        Selection.activeObject = script_directory;
        AssetDatabase.Refresh();

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                    "Assets/UnitySubmodules/UI/Templates/ViewTemplate.cs.txt",
                    name + ".cs");
        RefocusEditorWindow();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorPrefs.SetString(EDITOR_PREFS_VIEW, name);
        EditorUtility.DisplayProgressBar("Hold on", "Recompiling scripts", 0f);
    }

    private void RefocusEditorWindow()
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
        var asset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UnitySubmodules/UI/Prefabs/PrefabView.prefab");
        var prefab = Instantiate(asset);
        prefab.AddComponent(ExtraEditorUtility.IdentifyScriptType(name));

        string dir = "Assets/Resources/Views/";
        string path = string.Format("{0}{1}.prefab", dir, name);
        ExtraEditorUtility.EnsureDirectoryExists(dir);
        PrefabUtility.SaveAsPrefabAsset(prefab, path);
        AssetDatabase.SaveAssets();
        DestroyImmediate(prefab);
    }
}
