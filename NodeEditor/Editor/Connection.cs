using Codice.Client.Commands;
using System;
using UnityEditor;
using UnityEngine;

namespace Flawliz.Node.Editor
{
    public class Connection
    {
        public Node start;
        public Node end;
        public Vector2 anchor_start;
        public Vector2 anchor_end;
        public Vector2 tangent_start;
        public Vector2 tangent_end;

        public System.Action onClick;

        public Connection(Node start, Node end) 
        {
            this.start = start;
            this.end = end;
        }

        public void Draw()
        {
            var offset_start = new Vector2(anchor_start.x, anchor_start.y) * start.rect.size;
            var offset_end = new Vector2(anchor_end.x, anchor_end.y) * end.rect.size;
            var start_position = start.rect.center + offset_start;
            var end_position = end.rect.center + offset_end;

            Handles.DrawBezier(
                start_position,
                end_position,
                start_position + tangent_start * 50,
                end_position + tangent_end * 50,
                Color.white,
                null,
                2f
            );

            var arrow_size = 8;
            Handles.DrawLine(end_position, end_position + new Vector2(arrow_size, -arrow_size), 2);
            Handles.DrawLine(end_position, end_position + new Vector2(-arrow_size, -arrow_size), 2);

            if (Handles.Button(Vector2.Lerp(start_position, end_position, 0.5f), Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                onClick?.Invoke();
            }
        }
    }
}