using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColorPaletteValue), true)]
public class ColorPaletteValueDrawer : PropertyDrawer
{
    private SerializedProperty editor_update;
    private SerializedProperty palette_index;
    private SerializedProperty palette_name;
    private SerializedProperty map_index;
    private SerializedProperty map_name;
    private SerializedProperty color;

    private const float LENGTH_COLOR = 40;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        // Find properties
        editor_update = property.FindPropertyRelative(nameof(editor_update));
        palette_index = property.FindPropertyRelative(nameof(palette_index));
        palette_name = property.FindPropertyRelative(nameof(palette_name));
        map_index = property.FindPropertyRelative(nameof(map_index));
        map_name = property.FindPropertyRelative(nameof(map_name));
        color = property.FindPropertyRelative(nameof(color));

        // Draw label
        var rectLabel = EditorGUI.IndentedRect(position);
        var indent = rectLabel.x - position.x;
        GUI.Label(new Rect(rectLabel.position, new Vector2(rectLabel.width * EditorGUIUtility.labelWidth, rectLabel.height)), label);

        var rect_position = new Vector2(position.x + EditorGUIUtility.labelWidth - indent, position.y);
        var rect_size = new Vector2(position.width - EditorGUIUtility.labelWidth + indent, position.height);
        var rect = new Rect(rect_position, rect_size);
        var rect_popup = new Rect(rect_position.x, rect_position.y, rect_size.x - LENGTH_COLOR, rect_size.y);
        var rect_color = new Rect(rect_position.x + rect_popup.width, rect_position.y, LENGTH_COLOR, rect_size.y);

        EditorGUI.BeginChangeCheck();

        DrawPalette(rect_popup);
        DrawMap(rect_popup);
        DrawColor(rect_color);

        if (EditorGUI.EndChangeCheck())
        {
            color.colorValue = GetSelectedColor();
            editor_update.boolValue = true;
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    private void DrawPalette(Rect rect)
    {
        var palettes = Resources.LoadAll(EditorPaths.COLOR_PALETTE_RESOURCE_FOLDER);
        var options = palettes.Select(p => p.name).ToArray();

        rect = new Rect(rect.x, rect.y, rect.width * 0.5f, rect.height);
        var selected_index = GetSelectedIndex(options, palette_index.intValue, palette_name.stringValue);
        selected_index = EditorGUI.Popup(rect, selected_index, options);
        var selected_option = options[selected_index];

        palette_index.intValue = selected_index;
        palette_name.stringValue = selected_option;
    }

    private void DrawMap(Rect rect)
    {
        var palette = GetSelectedPalette();
        var options = palette.maps.Select(m => m.name).ToArray();

        var w = rect.width;
        rect = new Rect(rect.x + w * 0.5f, rect.y, w * 0.5f, rect.height);
        var selected_index = GetSelectedIndex(options, map_index.intValue, map_name.stringValue);
        selected_index = EditorGUI.Popup(rect, selected_index, options);
        var selected_option = options[selected_index];

        map_index.intValue = selected_index;
        map_name.stringValue = selected_option;
    }

    private void DrawColor(Rect rect)
    {
        GUI.enabled = false;
        var color = GetSelectedColor();
        EditorGUI.ColorField(rect, color);
        GUI.enabled = true;
    }

    private ColorPalette GetSelectedPalette()
    {
        var path = $"{EditorPaths.COLOR_PALETTE_RESOURCE_FOLDER}/{palette_name.stringValue}";
        var palette = Resources.Load<ColorPalette>(path);
        return palette;
    }

    private Color GetSelectedColor()
    {
        var palette = GetSelectedPalette();
        var color = palette.Get(map_name.stringValue);
        return color;
    }

    private int GetSelectedIndex(string[] options, int index, string option)
    {
        if (options.Contains(option))
        {
            return options.ToList().IndexOf(option);
        }
        else
        {
            return Mathf.Min(options.Length - 1, index);
        }
    }
}