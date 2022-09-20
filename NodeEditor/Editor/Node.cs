using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Flawliz.Node.Editor
{
    public class Node
    {
        public Rect rect;
        public string title;
        public bool isDragged;
        public bool isRoot;

        public GUIStyle style_normal;
        public GUIStyle style_selected;
        public GUIStyle style_root;
        public GUIStyle style_root_selected;

        public event System.Action<Vector2> onPositionChanged;

        public bool Selected { get; private set; }

        private const int PADDING_X = 10;
        private const int PADDING_Y = 2;
        private const int FIELD_HEIGHT = 20;
        private const int FIELD_SPACING = 2;

        private List<Property> properties = new List<Property>();

        public Node(Vector2 position)
        {
            rect = new Rect(position.x, position.y, 200, 50);

            style_normal = LoadStyle("builtin skins/darkskin/images/node1.png");
            style_selected = LoadStyle("builtin skins/darkskin/images/node1 on.png");
            style_root = LoadStyle("builtin skins/darkskin/images/node2.png");
            style_root_selected = LoadStyle("builtin skins/darkskin/images/node2 on.png");

            SetPosition(position);
        }

        private GUIStyle LoadStyle(string path)
        {
            var style = new GUIStyle();
            style.normal.background = EditorGUIUtility.Load(path) as Texture2D;
            style.border = new RectOffset(12, 12, 12, 12);
            return style;
        }

        public void Drag(Vector2 delta)
        {
            SetPosition(rect.position + delta);
        }

        public void Select() => Selected = true;
        public void Deselect() => Selected = false;

        public void SetPosition(Vector2 position)
        {
            rect.position = position;
            onPositionChanged?.Invoke(position);
        }

        private GUIStyle GetCurrentStyle()
        {
            if (isRoot)
            {
                return Selected ? style_root_selected : style_root;
            }
            else
            {
                return Selected ? style_selected : style_normal;
            }
        }

        public void Draw()
        {
            rect.size = new Vector2(rect.width, (PADDING_Y * 2) + ((FIELD_HEIGHT + FIELD_SPACING) * (properties.Count + 1)));
            GUI.Box(rect, title, GetCurrentStyle());

            var rect_cur = new Rect(rect.position.x + PADDING_X, rect.position.y + PADDING_Y, rect.width - FIELD_HEIGHT, rect.height - FIELD_HEIGHT);
            var size = new Vector2(rect_cur.width, FIELD_HEIGHT);

            rect_cur.position += new Vector2(0, FIELD_HEIGHT * 0.5f);
            foreach (var property in properties.ToList())
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
        public abstract class Property
        {
            public abstract void Draw(Rect rect);
        }

        public abstract class PropertyValue<T> : Property
        {
            public string label;
            public T value;
            public bool enabled;
            public System.Action<T> onValueChanged;

            public abstract T DrawValue(Rect rect);

            public override void Draw(Rect rect)
            {
                GUI.enabled = enabled;
                EditorGUI.BeginChangeCheck();
                var w = rect.width * 1f / 3f;
                GUI.Label(new Rect(rect.position, new Vector2(w, rect.height)), label);
                rect.position = new Vector2(rect.x + w, rect.y);
                rect.size = new Vector2(rect.width * 2f / 3f, rect.height);
                var v = DrawValue(rect);
                GUI.enabled = true;

                if (EditorGUI.EndChangeCheck())
                {
                    value = v;
                    onValueChanged?.Invoke(v);
                }
            }
        }

        private class StringValue : PropertyValue<string>
        {
            public override string DrawValue(Rect rect)
            {
                return GUI.TextField(new Rect(rect.x, rect.y, rect.width, rect.height), value);
            }
        }

        private class IntValue : PropertyValue<int>
        {
            public override int DrawValue(Rect rect)
            {
                var s = GUI.TextField(new Rect(rect.x, rect.y, rect.width, rect.height), value.ToString());
                int.TryParse(s, out var i);
                return i;
            }
        }

        private class FloatValue : PropertyValue<float>
        {
            public override float DrawValue(Rect rect)
            {
                var s = GUI.TextField(new Rect(rect.x, rect.y, rect.width, rect.height), value.ToString());
                float.TryParse(s, out var v);
                return v;
            }
        }

        private class BoolValue : PropertyValue<bool>
        {
            public override bool DrawValue(Rect rect)
            {
                return GUI.Toggle(new Rect(rect.x, rect.y, rect.width, rect.height), value, "");
            }
        }

        public void ClearProperties()
        {
            properties.Clear();
        }

        public PropertyValue<string> AddProperty(string label, string value, bool enabled, System.Action<string> onValueChanged = null)
        {
            var property = new StringValue { label = label, value = value, enabled = enabled };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
            return property;
        }

        public PropertyValue<int> AddProperty(string label, int value, bool enabled, System.Action<int> onValueChanged = null)
        {
            var property = new IntValue { label = label, value = value, enabled = enabled };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
            return property;
        }

        public PropertyValue<float> AddProperty(string label, float value, bool enabled, System.Action<float> onValueChanged = null)
        {
            var property = new FloatValue { label = label, value = value, enabled = enabled };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
            return property;
        }

        public PropertyValue<bool> AddProperty(string label, bool value, bool enabled, System.Action<bool> onValueChanged = null)
        {
            var property = new BoolValue { label = label, value = value, enabled = enabled };
            property.onValueChanged += onValueChanged;
            properties.Add(property);
            return property;
        }
        #endregion
    }
}