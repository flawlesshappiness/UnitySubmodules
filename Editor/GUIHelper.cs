using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class GUIHelper
{
    private static Stack<Color> gui_color_stack = new Stack<Color>();

    public static void PushColor(Color color)
    {
        gui_color_stack.Push(GUI.color);
        GUI.color = color;
    }

    public static void PopColor()
    {
        GUI.color = gui_color_stack.Pop();
    }

    public static void CenterLabel(string text, params GUILayoutOption[] options)
    {
        var prev = GUI.skin.label.alignment;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label(text, options);
        GUI.skin.label.alignment = prev;
    }

    public static void FittedLabel(string label)
    {
        EditorGUILayout.LabelField(label, GUILayout.Width(LabelWidth(label)));
    }

    public static float LabelWidth(string label)
    {
        return EditorStyles.label.CalcSize(new GUIContent(label)).x;
    }

    public enum GUITexture { PLUS, MINUS }
    public static Texture GetTexture(GUITexture type)
    {
        return type switch
        {
            GUITexture.MINUS => AssetDatabase.LoadAssetAtPath<Texture>("Assets/UnitySubmodules/Sprites/icon_minus.png"),
            GUITexture.PLUS => AssetDatabase.LoadAssetAtPath<Texture>("Assets/UnitySubmodules/Sprites/icon_plus.png"),
            _ => null
        };
    }
}