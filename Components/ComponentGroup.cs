using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ComponentGroup<T> : MonoBehaviour where T : Component
{
    protected List<T> Members { get; private set; } = new List<T>();

    protected virtual void Awake()
    {
        GetComponentsInChildren(true, Members);
    }

    public void SetMembersActive(bool active)
    {
        Members.ForEach(m => m.gameObject.SetActive(active));
    }

    protected List<T> EditorGetMembers()
    {
        return GetComponentsInChildren<T>(true).ToList();
    }

    protected virtual void OnDrawGizmos()
    {

    }
}