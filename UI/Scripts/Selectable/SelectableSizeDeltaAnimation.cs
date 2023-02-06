using Flawliz.Lerp;
using System.Collections;
using UnityEngine;

public class SelectableSizeDeltaAnimation : SelectableAnimation
{
    [Header("SELECTABLE SIZE DELTA ANIMATION")]
    [SerializeField] private RectTransform pivot_animation;
    [SerializeField] private CanvasGroup cvg_animation, cvg_main;

    public CanvasGroup CanvasGroup { get { return cvg_main; } }

    private void OnEnable()
    {
        cvg_animation.alpha = 0;
        pivot_animation.sizeDelta = Vector2.right * 100f;
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
        var scale_show = 0f;
        var scale_hide = 100f;
        var scale_start = pivot_animation.sizeDelta;
        var scale_end = Vector2.right * (selected ? scale_show : scale_hide);
        var alpha_start = cvg_animation.alpha;
        var alpha_end = selected ? 1 : 0;

        return this.StartCoroutineWithID(Cr(), "animate_" + GetInstanceID()); ;
        IEnumerator Cr()
        {
            yield return LerpEnumerator.Value(0.15f, f =>
            {
                cvg_animation.alpha = Mathf.Lerp(alpha_start, alpha_end, f);
                pivot_animation.sizeDelta = Vector2.Lerp(scale_start, scale_end, f);
            }).UnscaledTime();
        }
    }
}