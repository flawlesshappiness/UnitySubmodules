using UnityEngine;

public class Collider2DGroup : ComponentGroup<Collider2D>
{
    public bool gizmos_enabled;
    public bool gizmos_wire;
    public Color gizmos_color;

    public void SetMembersEnabled(bool enabled)
    {
        Members.ForEach(m => m.enabled = enabled);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!gizmos_enabled) return;

        Gizmos.color = gizmos_color;

        foreach(var m in EditorGetMembers())
        {
            DrawCircle(m);
            DrawBox(m);
        }
    }

    private void DrawCircle(Collider2D collider)
    {
        var c = collider as CircleCollider2D;
        if (c == null) return;

        var position = c.transform.position.ToVector2() + c.offset;
        var scale = transform.lossyScale;
        var scale_max = Mathf.Max(scale.x, scale.y, scale.z);
        var radius = c.radius * scale_max;

        if (gizmos_wire)
        {
            Gizmos.DrawWireSphere(position, radius);
        }
        else
        {
            Gizmos.DrawSphere(position, radius);
        }
    }

    private void DrawBox(Collider2D collider)
    {
        var c = collider as BoxCollider2D;
        if (c == null) return;

        var position = c.transform.position.ToVector2() + c.offset;
        var scale = transform.lossyScale;
        var size = c.size * scale;

        if (gizmos_wire)
        {
            Gizmos.DrawWireCube(position, size);
        }
        else
        {
            Gizmos.DrawCube(position, size);
        }
    }
}