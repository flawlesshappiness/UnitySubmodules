using UnityEngine;
using EasingCurve;

public static class EasingCurves {

    public static AnimationCurve EaseInQuad { get { return GetCurve(EasingFunctions.Ease.EaseInQuad); } }
    public static AnimationCurve EaseOutQuad { get { return GetCurve(EasingFunctions.Ease.EaseOutQuad); } }
    public static AnimationCurve EaseInOutQuad { get { return GetCurve(EasingFunctions.Ease.EaseInOutQuad); } }
    public static AnimationCurve EaseInCubic { get { return GetCurve(EasingFunctions.Ease.EaseInCubic); } }
    public static AnimationCurve EaseOutCubic { get { return GetCurve(EasingFunctions.Ease.EaseOutCubic); } }
    public static AnimationCurve EaseInOutCubic { get { return GetCurve(EasingFunctions.Ease.EaseInOutCubic); } }
    public static AnimationCurve EaseInQuart { get { return GetCurve(EasingFunctions.Ease.EaseInQuart); } }
    public static AnimationCurve EaseOutQuart { get { return GetCurve(EasingFunctions.Ease.EaseOutQuart); } }
    public static AnimationCurve EaseInOutQuart { get { return GetCurve(EasingFunctions.Ease.EaseInOutQuart); } }
    public static AnimationCurve EaseInQuint { get { return GetCurve(EasingFunctions.Ease.EaseInQuint); } }
    public static AnimationCurve EaseOutQuint { get { return GetCurve(EasingFunctions.Ease.EaseOutQuint); } }
    public static AnimationCurve EaseInOutQuint { get { return GetCurve(EasingFunctions.Ease.EaseInOutQuint); } }
    public static AnimationCurve EaseInSine { get { return GetCurve(EasingFunctions.Ease.EaseInSine); } }
    public static AnimationCurve EaseOutSine { get { return GetCurve(EasingFunctions.Ease.EaseOutSine); } }
    public static AnimationCurve EaseInOutSine { get { return GetCurve(EasingFunctions.Ease.EaseInOutSine); } }
    public static AnimationCurve EaseInExpo { get { return GetCurve(EasingFunctions.Ease.EaseInExpo); } }
    public static AnimationCurve EaseOutExpo { get { return GetCurve(EasingFunctions.Ease.EaseOutExpo); } }
    public static AnimationCurve EaseInOutExpo { get { return GetCurve(EasingFunctions.Ease.EaseInOutExpo); } }
    public static AnimationCurve EaseInCirc { get { return GetCurve(EasingFunctions.Ease.EaseInCirc); } }
    public static AnimationCurve EaseOutCirc { get { return GetCurve(EasingFunctions.Ease.EaseOutCirc); } }
    public static AnimationCurve EaseInOutCirc { get { return GetCurve(EasingFunctions.Ease.EaseInOutCirc); } }
    public static AnimationCurve Linear { get { return AnimationCurve.Linear(0, 0, 1, 1); } }
    public static AnimationCurve Spring { get { return GetCurve(EasingFunctions.Ease.Spring); } }
    public static AnimationCurve EaseInBounce { get { return GetCurve(EasingFunctions.Ease.EaseInBounce); } }
    public static AnimationCurve EaseOutBounce { get { return GetCurve(EasingFunctions.Ease.EaseOutBounce); } }
    public static AnimationCurve EaseInOutBounce { get { return GetCurve(EasingFunctions.Ease.EaseInOutBounce); } }
    public static AnimationCurve EaseInBack { get { return GetCurve(EasingFunctions.Ease.EaseInBack); } }
    public static AnimationCurve EaseOutBack { get { return GetCurve(EasingFunctions.Ease.EaseOutBack); } }
    public static AnimationCurve EaseInOutBack { get { return GetCurve(EasingFunctions.Ease.EaseInOutBack); } }
    public static AnimationCurve EaseInElastic { get { return GetCurve(EasingFunctions.Ease.EaseInElastic); } }
    public static AnimationCurve EaseOutElastic { get { return GetCurve(EasingFunctions.Ease.EaseOutElastic); } }
    public static AnimationCurve EaseInOutElastic { get { return GetCurve(EasingFunctions.Ease.EaseInOutElastic); } }

    private static AnimationCurve GetCurve(EasingFunctions.Ease ease) {
        return EasingAnimationCurve.EaseToAnimationCurve(ease);
    }
}
