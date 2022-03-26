using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ExtraEditorUtility
{
    public static void EnsureDirectoryExists(string path)
    {
        System.IO.Directory.CreateDirectory(path);
    }

    public static void FittedLabel(string label)
    {
        EditorGUILayout.LabelField(label, GUILayout.Width(LabelWidth(label)));
    }

    public static float LabelWidth(string label)
    {
        return EditorStyles.label.CalcSize(new GUIContent(label)).x;
    }

    public static Type IdentifyScriptType(string name)
    {
        Type type = null;
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            type = assembly.GetType(name);
            if (type != null)
                break;
        }

        return type;
    }
}
