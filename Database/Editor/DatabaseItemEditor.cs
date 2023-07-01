using UnityEditor;
using UnityEngine;

public class DatabaseItemEditor<V, DB> : Editor
    where V : ScriptableObject
    where DB : Database<V>
{
    protected V script;

    protected virtual void OnEnable()
    {
        script = target as V;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUIHelper.DrawDatabaseButtons<DB, V>(script);
        GUIHelper.DrawAssetSaveButton(script);
        EditorGUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }
}