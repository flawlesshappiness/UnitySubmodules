using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CreateViewWindow : EditorWindow
{
    private static EditorWindow window;
    private static string view_filename = "NewView";

    private const string EDITOR_PREFS_VIEW = "GeneratedViewAsset";

    private const int HEIGHT = 60;

    [MenuItem("Assets/Create/UI/View", false, 1)]
    public static void ShowWinow()
    {
        if(window != null)
        {
            // Window already open
            return;
        }

        var rect = EditorGUIUtility.GetMainWindowPosition();

        window = CreateInstance<CreateViewWindow>();
        window.ShowPopup();
        window.titleContent = new GUIContent("Create View");

        window.minSize = new Vector2(rect.width * 0.5f, HEIGHT);
        window.maxSize = window.minSize;

        var x = rect.position.x + (rect.width * 0.5f) - (window.position.width * 0.5f);
        var y = rect.position.y + (rect.height * 0.5f) - (window.position.height * 0.5f);
        rect.width = window.position.width;
        rect.height = window.position.height;
        rect.position = new Vector2(x, y);
        window.position = rect;
    }

    private void Update()
    {
        if (!window.IsFocused())
        {
            window.Close();
        }
    }

    private void OnGUI()
    {
        var font_prev = GUI.skin.textField.fontSize;
        GUI.skin.textField.fontSize = (int)(HEIGHT * 0.7f);

        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("TextField");
        view_filename = EditorGUILayout.TextField(view_filename, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.EndHorizontal();

        GUI.skin.textField.fontSize = font_prev;

        if (Event.current.isKey)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.Escape:
                    window.Close();
                    break;

                case KeyCode.Return:
                    window.Close();
                    TryCreateView(view_filename);
                    break;
            }
        }

        EditorGUI.FocusTextInControl("TextField");
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
