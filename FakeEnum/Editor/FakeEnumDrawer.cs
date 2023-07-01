using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FakeEnum), true)]
public class FakeEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var rectLabel = EditorGUI.IndentedRect(position);
        GUI.Label(new Rect(rectLabel.position, new Vector2(rectLabel.width * EditorGUIUtility.labelWidth, rectLabel.height)), label);

        var indent = rectLabel.x - position.x;

        if (property.serializedObject != null)
        {
            var value = property.GetValue();
            var current = value as FakeEnum;
            var types = FakeEnum.GetAll(current.GetType()).ToArray();
            var options = types.Select(x => x.id).ToArray();
            var idx = types.ToList().FindIndex((x) => x == current);
            var rectPopup = new Rect(new Vector2(position.x + EditorGUIUtility.labelWidth, position.y), new Vector2(position.width - EditorGUIUtility.labelWidth, position.height));
            SearchPopup.DrawButton(rectPopup, idx, options, i =>
            {
                current.id = options[Mathf.Clamp(i, 0, types.Length - 1)];
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            });
        }
    }
}