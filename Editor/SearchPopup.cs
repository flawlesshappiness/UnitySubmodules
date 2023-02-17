using System.Linq;
using UnityEditor;
using UnityEngine;

public class SearchPopup : PopupWindowContent
{
    private Rect rect;
    private string[] options;
    private string[] valid_options;
    private int idx_selected;
    private System.Action<int> on_index_selected;

    private static string s_input;
    private static Vector2 scroll_position;

    private const int ELEMENT_HEIGHT = 20;

    public static void DrawButton(Rect rect, int idx_selected, string[] options, System.Action<int> on_index_selected)
    {
        idx_selected = Mathf.Clamp(idx_selected, 0, options.Length - 1);
        if (GUI.Button(rect, options[idx_selected]))
        {
            ShowPopup(rect, idx_selected, options, on_index_selected);
        }
    }

    public static SearchPopup ShowPopup(Rect rect, int idx_selected, string[] options, System.Action<int> on_index_selected)
    {
        var popup = new SearchPopup(rect, idx_selected, options, on_index_selected);
        PopupWindow.Show(rect, popup);
        return popup;
    }

    public SearchPopup(Rect rect, int idx_selected, string[] options, System.Action<int> on_index_selected)
    {
        this.rect = rect;
        this.idx_selected = idx_selected;
        this.options = options;
        this.on_index_selected = on_index_selected;
        UpdateValidOptions();
    }

    public override Vector2 GetWindowSize()
    {
        var height = Mathf.Min(400, ELEMENT_HEIGHT + (options.Length * ELEMENT_HEIGHT));
        return new Vector2(rect.width, height);
    }

    public override void OnGUI(Rect rect)
    {
        DrawSearchField();

        scroll_position = EditorGUILayout.BeginScrollView(scroll_position);

        var style_normal = GetButtonStyle(false);
        var style_selected = GetButtonStyle(true);
        for (int i = 0; i < valid_options.Length; i++)
        {
            var option = valid_options[i];
            var actual_index = GetActualIndex(i);
            var selected = idx_selected == actual_index;
            var style = selected ? style_selected : style_normal;
            if (GUILayout.Button(option, style, GUILayout.Height(ELEMENT_HEIGHT)))
            {
                var selected_option = valid_options[i];
                on_index_selected?.Invoke(actual_index);
                editorWindow.Close();
            }
        }

        EditorGUILayout.EndScrollView();

        editorWindow.Repaint();
    }

    private void DrawSearchField()
    {
        var new_input = GUILayout.TextField(s_input);
        if(new_input != s_input)
        {
            s_input = new_input;
            UpdateValidOptions();
        }
    }

    private void UpdateValidOptions()
    {
        valid_options = options
                .Where(s => string.IsNullOrEmpty(s_input) || s.ToLower().Contains(s_input.ToLower()))
                .ToArray();
    }

    private int GetActualIndex(int i) => options.ToList().IndexOf(valid_options[i]);

    GUIStyle GetButtonStyle(bool selected)
    {
        var text_color = GUI.skin.label.normal.textColor;
        var no_color = Color.clear;
        var selected_color = new Color(0, 0, 0, 0.2f);
        var normal_color = selected ? selected_color : no_color;

        var s = new GUIStyle(GUI.skin.box);
        s.stretchWidth = true;
        s.fixedWidth = 0;
        s.alignment = TextAnchor.MiddleLeft;
        s.margin = new RectOffset(0, 0, 0, 0);
        s.normal = new GUIStyleState { textColor = text_color, background = CreateTexture(2, 2, normal_color) };
        s.hover = new GUIStyleState { textColor = text_color, background = CreateTexture(2, 2, new Color(1, 1, 1, 0.2f)) };
        s.active = new GUIStyleState { textColor = text_color, background = CreateTexture(2, 2, new Color(1, 1, 1, 0.1f)) };
        return s;
    }

    private Texture2D CreateTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pixels);
        result.Apply();
        return result;
    }
}