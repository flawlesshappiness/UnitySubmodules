using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Flawliz.Node.Editor
{
    public class Node
    {
        public Rect rect;
        public string title;
        public bool isDragged;

        public GUIStyle style;

        public event System.Action<Vector2> onPositionChanged;

        private const int PADDING_X = 10;
        private const int PADDING_Y = 2;
        private const int FIELD_HEIGHT = 20;
        private const int FIELD_SPACING = 2;

        private List<Property> properties = new List<Property>();

        public Node(Vector2 position, GUIStyle nodeStyle)
        {
            rect = new Rect(position.x, position.y, 200, 50);
            style = nodeStyle;

            SetPosition(position);
        }

        public void Drag(Vector2 delta)
        {
            SetPosition(rect.position + delta);
        }

        public void SetPosition(Vector2 position)
        {
            rect.position = position;
            onPositionChanged?.Invoke(position);
        }

        public void Draw()
        {
            rect.size = new Vector2(rect.width, (PADDING_Y * 2) + ((FIELD_HEIGHT + FIELD_SPACING) * (properties.Count + 1)));
            GUI.Box(rect, title, style);

            var rect_cur = new Rect(rect.position.x + PADDING_X, rect.position.y + PADDING_Y, rect.width - FIELD_HEIGHT, rect.height - FIELD_HEIGHT);
            var size = new Vector2(rect_cur.width, FIELD_HEIGHT);

            rect_cur.position += new Vector2(0, FIELD_HEIGHT * 0.5f);
            foreach (var property in properties)
            {
                property.Draw(new Rect(rect_cur.position, size));
                rect_cur.position += new Vector2(0, FIELD_HEIGHT + FIELD_SPACING);
            }
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            GUI.changed = true;
                        }
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        #region PROPERTY
        private abstract class Property
        {
            public virtual void Draw(Rect rect) { }
        }

        private abstract class PropertyValue<T> : Property
        {
            public string label;
            public T value;
            public System.Action<T> onValueChanged;

            public override void Draw(Rect rect)
            {
                base.Draw(rect);
                GUI.Label(new Rect(rect.position, new Vector2(rect.width * 0.5f, rect.height)), label);
            }
        }

        private class StringValue : PropertyValue<string>
        {
            public override void Draw(Rect rect)
            {
                base.Draw(rect);
                EditorGUI.BeginChangeCheck();
                var v = GUI.TextField(new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height), value);
                if (EditorGUI.EndChangeCheck())
                {
                    value = v;
                    onValueChanged?.Invoke(v);
                }
            }
        }

        private class IntValue : PropertyValue<int>
        {
            public override void Draw(Rect rect)
            {
                base.Draw(rect);
                EditorGUI.BeginChangeCheck();
                var s = GUI.TextField(new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height), value.ToString());
                if (EditorGUI.EndChangeCheck() && int.TryParse(s, out var v))
                {
                    value = v;
                    onValueChanged?.Invoke(v);
                }
            }
        }

        private class FloatValue : PropertyValue<float>
        {
            public override void Draw(Rect rect)
            {
                base.Draw(rect);
                EditorGUI.BeginChangeCheck();
                var s = GUI.TextField(new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height), value.ToString());
                if (EditorGUI.EndChangeCheck() && float.TryParse(s, out var v))
                {
                    value = v;
                    onValueChanged?.Invoke(v);
                }
            }
        }

        private class BoolValue : PropertyValue<bool>
        {
            public override void Draw(Rect rect)
            {
                base.Draw(rect);
                EditorGUI.BeginChangeCheck();
                var v = GUI.Toggle(new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height), value, "");
                if (EditorGUI.EndChangeCheck())
                {
                    value = v;
                    onValueChanged?.Invoke(v);
                }
            }
        }

        public void AddProperty(string label, string value, System.Action<string> onValueChanged = null)
        {
            var property = new StringValue { label = label, value = value };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
        }

        public void AddProperty(string label, int value, System.Action<int> onValueChanged = null)
        {
            var property = new IntValue { label = label, value = value };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
        }

        public void AddProperty(string label, float value, System.Action<float> onValueChanged = null)
        {
            var property = new FloatValue { label = label, value = value };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
        }

        public void AddProperty(string label, bool value, System.Action<bool> onValueChanged = null)
        {
            var property = new BoolValue { label = label, value = value };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
        }
        #endregion
    }
}