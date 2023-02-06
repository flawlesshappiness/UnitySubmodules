using Flawliz.Lerp;
using System.Collections;
using UnityEngine;

public class SelectableLeftRightAnimation : SelectableSizeDeltaAnimation
{
    [Header("SELECTABLE LEFT RIGHT ANIMATION")]
    public RectTransform rt_anim_left;
    public RectTransform rt_anim_right;
    public float anim_distance;
    public float anim_duration;
    public AnimationCurve anim_curve;

    public CustomCoroutine AnimateLeft()
    {
        return this.StartCoroutineWithID(AnimateCr(rt_anim_left, Vector3.left), "animate_left_" + GetInstanceID());
    }

    public CustomCoroutine AnimateRight()
    {
        return this.StartCoroutineWithID(AnimateCr(rt_anim_right, Vector3.right), "animate_right_" + GetInstanceID());
    }

    private IEnumerator AnimateCr(RectTransform rt, Vector3 dir)
    {
        var start = Vector3.zero;
        var end = dir * anim_distance;
        yield return LerpEnumerator.AnchoredPosition(rt, anim_duration, start, end).Curve(anim_curve).UnscaledTime();
    }
}