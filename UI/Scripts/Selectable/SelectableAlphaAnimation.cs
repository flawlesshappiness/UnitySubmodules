using Flawliz.Lerp;
using System.Collections;
using UnityEngine;

public class SelectableAlphaAnimation : SelectableAnimation
{
    [SerializeField] private CanvasGroup cvg;
    [SerializeField] private float alpha_deselect, alpha_selected;

    private void OnValidate()
    {
        if(cvg == null)
        {
            cvg = GetComponentInParent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        cvg.alpha = alpha_deselect;
    }

    public override CustomCoroutine AnimateDeselect()
    {
        return AnimateSelected(false);
    }

    public override CustomCoroutine AnimateSelect()
    {
        return AnimateSelected(true);
    }

    public CustomCoroutine AnimateSelected(bool selected)
    {
        var end = selected ? alpha_selected : alpha_deselect;
        return this.StartCoroutineWithID(Cr(), "animate_" + GetInstanceID()); ;
        IEnumerator Cr()
        {
            yield return LerpEnumerator.Alpha(cvg, duration, end)
                .UnscaledTime();
        }
    }
}