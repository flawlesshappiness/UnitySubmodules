using UnityEngine;

public class ColliderGroup : ComponentGroup<Collider>
{
    public void SetMembersEnabled(bool enabled)
    {
        Members.ForEach(m => m.enabled = enabled);
    }
}