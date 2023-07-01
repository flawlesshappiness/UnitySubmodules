using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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

    public enum GUITexture { PLUS, MINUS, SAVE, DATABASE, DATABASE_GOOD, DATABASE_BAD }
    public static Texture GetTexture(GUITexture type)
    {
        return type switch
        {
            GUITexture.MINUS => AssetDatabase.LoadAssetAtPath<Texture>($"{EditorPaths.SPRITES}/icon_minus.png"),
            GUITexture.PLUS => AssetDatabase.LoadAssetAtPath<Texture>($"{EditorPaths.SPRITES}/icon_plus.png"),
            GUITexture.SAVE => AssetDatabase.LoadAssetAtPath<Texture>($"{EditorPaths.SPRITES}/icon_save.png"),
            GUITexture.DATABASE => AssetDatabase.LoadAssetAtPath<Texture>($"{EditorPaths.SPRITES}/icon_database.png"),
            GUITexture.DATABASE_GOOD => AssetDatabase.LoadAssetAtPath<Texture>($"{EditorPaths.SPRITES}/icon_database_good.png"),
            GUITexture.DATABASE_BAD => AssetDatabase.LoadAssetAtPath<Texture>($"{EditorPaths.SPRITES}/icon_database_bad.png"),
            _ => null
        };
    }

    public static void DrawAssetSaveButton(Object asset)
    {
        GUI.enabled = EditorUtility.IsDirty(asset.GetInstanceID());

        if (GUILayout.Button(GetTexture(GUITexture.SAVE), GUILayout.Width(30), GUILayout.Height(30)))
        {
            AssetDatabase.SaveAssets();
        }

        GUI.enabled = true;
    }

    public static void DrawDatabaseButtons<DB, V>(V value)
        where V : ScriptableObject
        where DB : Database<V>
    {
        var db = Database.Load<DB>();
        if (db == null)
        {
            Debug.LogError($"Database of type {typeof(DB)} does not exist");
        }
        else if (db.collection.Contains(value))
        {
            if (GUILayout.Button(GetTexture(GUITexture.DATABASE_GOOD), GUILayout.Width(30), GUILayout.Height(30)))
            {
                RemoveFromDatabase();
            }
        }
        else
        {
            if (GUILayout.Button(GetTexture(GUITexture.DATABASE_BAD), GUILayout.Width(30), GUILayout.Height(30)))
            {
                AddToDatabase();
            }
        }

        void RemoveFromDatabase()
        {
            var items = Selection.objects.Select(o => o as V);
            foreach (var item in items)
            {
                if (item == null) continue;
                db.collection.Remove(item);
            }

            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
        }

        void AddToDatabase()
        {
            var items = Selection.objects.Select(o => o as V);
            foreach (var item in items)
            {
                if (item == null) continue;
                db.collection.Add(item);
            }

            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
        }
    }
}