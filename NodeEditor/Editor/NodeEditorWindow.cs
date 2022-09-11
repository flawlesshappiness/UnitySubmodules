using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Flawliz.Node.Editor
{
    public class NodeEditorWindow : EditorWindow
    {
        protected List<Node> nodes = new List<Node>();
        protected List<Connection> connections = new List<Connection>();

        protected Vector2 offset;
        protected Vector2 drag;

        protected GUIStyle style_node;

        private void OnEnable()
        {
            style_node = new GUIStyle();
            style_node.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            style_node.focused.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            style_node.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnGUI()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawConnections();
            DrawNodes();
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);
            if (GUI.changed) Repaint();
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            nodes.ForEach(n => n.Draw());
        }

        private void DrawConnections()
        {
            connections.ForEach(c => c.Draw());
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;
            nodes.ForEach(node => node.Drag(delta));
            GUI.changed = true;
        }

        private void ProcessNodeEvents(Event e)
        {
            GUI.changed = GUI.changed || nodes.Any(node => node.ProcessEvents(e));
        }

        protected virtual void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }

        protected Node AddNode(Vector2 position)
        {
            var node = new Node(position, style_node);
            nodes.Add(node);
            return node;
        }

        protected void RemoveNode(Node node)
        {
            nodes.Remove(node);
        }

        protected void RemoveAllNodes()
        {
            nodes.Clear();
            connections.Clear();
        }

        protected Connection ConnectNodes(Node nodeA, Node nodeB)
        {
            var connection = new Connection(nodeA, nodeB);
            connections.Add(connection);
            return connection;
        }

        protected void DisconnectNode(Node node)
        {
            connections.Where(c => c.start == node || c.end == node)
                .ToList().ForEach(c =>
                {
                    connections.Remove(c);
                });
        }

        protected void DisconnectNodes(Node nodeA, Node nodeB)
        {
            connections.Where(c => (c.start == nodeA && c.end == nodeB) || (c.start == nodeB && c.end == nodeA))
                .ToList().ForEach(c =>
                {
                    connections.Remove(c);
                });
        }
    }
}