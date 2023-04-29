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
        GUIHelper.DrawAssetSaveButton(script);
        GUIHelper.DrawDatabaseButtons<DB, V>(script);
        EditorGUILayout.Space(20);
        base.OnInspectorGUI();
    }
}