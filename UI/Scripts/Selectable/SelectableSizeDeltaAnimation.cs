using Flawliz.Lerp;
using System.Collections;
using UnityEngine;

public class SelectableSizeDeltaAnimation : SelectableAnimation
{
    [SerializeField] private RectTransform pivot_animation;
    [SerializeField] private Vector3 scale_deselected, scale_selected;

    private void OnEnable()
    {
        pivot_animation.sizeDelta = scale_deselected;
    }

    public override CustomCoroutine AnimateSelect()
    {
        return AnimateSelected(true);
    }

    public override CustomCoroutine AnimateDeselect()
    {
        return AnimateSelected(false);
    }

    public CustomCoroutine AnimateSelected(bool selected)
    {
        var end = selected ? scale_selected : scale_deselected;
        return this.StartCoroutineWithID(Cr(), "animate_" + GetInstanceID()); ;
        IEnumerator Cr()
        {
            yield return LerpEnumerator.SizeDelta(pivot_animation, duration, end)
                .UnscaledTime();
        }
    }
}