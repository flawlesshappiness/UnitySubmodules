using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NameObjectWindow : EditorWindow
{
    private static NameObjectWindow window;
    private const int HEIGHT = 60;

    private string input;
    private System.Action<string> onSubmit;

    public static void Show(string default_input, System.Action<string> onSubmit)
    {
        if (window != null)
        {
            // Window already open
            return;
        }

        var rect = EditorGUIUtility.GetMainWindowPosition();

        window = CreateInstance<NameObjectWindow>();
        window.ShowPopup();

        window.minSize = new Vector2(rect.width * 0.5f, HEIGHT);
        window.maxSize = window.minSize;

        var x = rect.position.x + (rect.width * 0.5f) - (window.position.width * 0.5f);
        var y = rect.position.y + (rect.height * 0.5f) - (window.position.height * 0.5f);
        rect.width = window.position.width;
        rect.height = window.position.height;
        rect.position = new Vector2(x, y);
        window.position = rect;

        window.input = default_input;
        window.onSubmit = onSubmit;
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
        input = EditorGUILayout.TextField(input, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
                    onSubmit?.Invoke(input);
                    return;
            }
        }

        EditorGUI.FocusTextInControl("TextField");
    }
}