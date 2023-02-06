using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections;
using System;

[CustomPropertyDrawer(typeof(FakeEnum), true)]
public class FakeEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var rectLabel = EditorGUI.IndentedRect(position);
        GUI.Label(new Rect(rectLabel.position, new Vector2(rectLabel.width * EditorGUIUtility.labelWidth, rectLabel.height)), label);

        var indent = rectLabel.x - position.x;

        EditorGUI.BeginChangeCheck();
        if (property.serializedObject != null)
        {
            var current = GetValue(property) as FakeEnum;
            var types = FakeEnum.GetAll(current.GetType()).ToArray();
            var options = types.Select(x => x.id).ToArray();
            var idx = types.ToList().FindIndex((x) => x == current);
            var rectPopup = new Rect(new Vector2(position.x + EditorGUIUtility.labelWidth - indent, position.y), new Vector2(position.width - EditorGUIUtility.labelWidth + indent, position.height));
            idx = EditorGUI.Popup(rectPopup, Mathf.Clamp(idx, 0, types.Length - 1), options);
            current.id = options[Mathf.Clamp(idx, 0, types.Length - 1)];
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(property.serializedObject.targetObject);
        }
    }

    // https://answers.unity.com/questions/425012/get-the-instance-the-serializedproperty-belongs-to.html
    public object GetParent(SerializedProperty prop)
    {
        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        foreach (var element in elements.Take(elements.Length - 1))
        {
            if (element.Contains("["))
            {
                var elementName = element.Substring(0, element.IndexOf("["));
                var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue(obj, elementName, index);
            }
            else
            {
                obj = GetValue(obj, element);
            }
        }
        return obj;
    }

    public object GetValue(object source, string name)
    {
        if (source == null)
            return null;
        var type = source.GetType();
        var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (f == null)
        {
            var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (p == null)
                return null;
            return p.GetValue(source, null);
        }
        return f.GetValue(source);
    }

    public object GetValue(object source, string name, int index)
    {
        var enumerable = GetValue(source, name) as IEnumerable;
        var enm = enumerable.GetEnumerator();
        while (index-- >= 0)
            enm.MoveNext();
        return enm.Current;
    }

    public object GetValue(SerializedProperty prop)
    {
        string path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        string[] elements = path.Split('.');

        foreach (string element in elements.Take(elements.Length))
        {
            if (element.Contains("["))
            {
                string elementName = element.Substring(0, element.IndexOf("["));
                int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                obj = GetValue(obj, elementName, index);
            }
            else
            {
                obj = GetValue(obj, element);
            }
        }

        return obj;
    }
}