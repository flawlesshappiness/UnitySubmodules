using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableGroupAnimation : SelectableAnimation
{
    [SerializeField] private List<SelectableAnimation> animations = new List<SelectableAnimation>();

    private void OnValidate()
    {
        foreach (var animation in GetComponentsInChildren<SelectableAnimation>())
        {
            if (animation == this) continue;
            if (!animations.Contains(animation))
            {
                AddAnimation(animation);
            }
        }
    }

    public void AddAnimation(SelectableAnimation animation)
    {
        animations.Add(animation);
    }

    public override CustomCoroutine AnimateDeselect()
    {
        var crs = new List<CustomCoroutine>();
        foreach(var animation in animations)
        {
            var cr = animation.AnimateDeselect();
            crs.Add(cr);
        }
        return this.StartCoroutineWithID(WaitForCoroutinesCr(crs), "animate_" + GetInstanceID());
    }

    public override CustomCoroutine AnimateSelect()
    {
        var crs = new List<CustomCoroutine>();
        foreach (var animation in animations)
        {
            var cr = animation.AnimateSelect();
            crs.Add(cr);
        }
        return this.StartCoroutineWithID(WaitForCoroutinesCr(crs), "animate_" + GetInstanceID());
    }

    private IEnumerator WaitForCoroutinesCr(List<CustomCoroutine> crs)
    {
        foreach(var cr in crs)
        {
            yield return cr;
        }
    }
}