using UnityEngine;

public abstract class SelectableAnimation : MonoBehaviour
{
    [SerializeField] protected float duration;
    public abstract CustomCoroutine AnimateSelect();
    public abstract CustomCoroutine AnimateDeselect();

    private void Reset()
    {
        var group = GetComponent<SelectableGroupAnimation>();
        if(group != null && group != this)
        {
            group.AddAnimation(this);
        }
    }
}