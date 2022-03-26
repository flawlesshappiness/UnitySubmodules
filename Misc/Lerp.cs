using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

#region ENUMERATOR
public class LerpEnumerator : IEnumerator
{
    private ILerp _lerp;

    public LerpEnumerator(ILerp lerp)
    {
        _lerp = lerp;
    }

    public object Current => _lerp;

    public bool MoveNext()
    {
        if (!_lerp.IsObjectActive()) return false;
        _lerp.Apply();
        return !_lerp.HasEnded();
    }

    public void Reset()
    {
        _lerp.Reset();
    }
}
#endregion
#region ILERP
public interface ILerp : IEnumerable
{
    /// <summary>
    /// Gets this lerp's id
    /// </summary>
    /// <returns>The id</returns>
    string GetID();

    /// <summary>
    /// Checks if the lerp has ended
    /// </summary>
    /// <returns>True if has ended, else false</returns>
    bool HasEnded();

    /// <summary>
    /// Checks if the cancelWith object exists and is active
    /// </summary>
    /// <returns>True if active, else false</returns>
    bool IsObjectActive();

    /// <summary>
    /// Applies the lerp value based on the current time
    /// </summary>
    void Apply();

    /// <summary>
    /// Ends the lerp, and calls the lerp's OnEnd function, if any
    /// </summary>
    void End();

    /// <summary>
    /// Sets a curve function
    /// </summary>
    /// <param name="curve">The curve function</param>
    /// <returns>The lerp</returns>
    ILerp Curve(Func<float, float> funcCurve);

    /// <summary>
    /// Sets a curve function from one of the predefined curve types
    /// </summary>
    /// <param name="curve">The curve type</param>
    /// <returns>The lerp</returns>
    ILerp Curve(Lerp.Curve curve);

    /// <summary>
    /// Sets the curve function from an AnimationCurve
    /// </summary>
    /// <param name="animCurve">The AnimationCurve</param>
    /// <returns>The lerp</returns>
    ILerp Curve(AnimationCurve animCurve);

    /// <summary>
    /// Set action to be called when the lerp is finished
    /// </summary>
    /// <param name="onFinished">Action to call on lerp finish</param>
    /// <returns>The lerp</returns>
    ILerp OnEnd(Action onFinished);

    /// <summary>
    /// Set how many times the lerp will loop. -1 = infinitely
    /// </summary>
    /// <param name="loops">Loop amount</param>
    /// <returns>The lerp</returns>
    ILerp Loop(int loopAmount = -1);

    /// <summary>
    /// Oscillates the lerp, when it loops
    /// </summary>
    /// <returns>The lerp</returns>
    ILerp Oscillate();

    /// <summary>
    /// Delays the start and end time of the lerp
    /// </summary>
    /// <param name="time">The delay time</param>
    /// <returns>The lerp</returns>
    ILerp Delay(float time);

    /// <summary>
    /// Resets the lerp
    /// </summary>
    /// <returns>The lerp</returns>
    ILerp Reset();

    /// <summary>
    /// Unclamps the lerp function
    /// </summary>
    /// <returns>The lerp</returns>
    ILerp Unclamp();
}
#endregion
#region LERP<T>
public class Lerp<T> : ILerp
{
    public string id;

    private float _time;
    private float _timeStart;
    private float _timeEnd;

    private T _valueStart;
    private T _valueEnd;

    private Func<T, T, float, T> _funcLerpClamped;
    private Func<T, T, float, T> _funcLerpUnclamped;
    private Func<float, float> _funcCurve;
    public Action<T> onApply;
    public Action onEnd;

    public GameObject cancelWith;

    // Loop
    private int _loops = 0;
    private bool _oscillate = false;

    // Unclamp
    private bool _unclamped = false;

    /// <summary>
    /// A lerp object, used for linear interpolation between two values.
    /// </summary>
    /// <param name="time">Duration for the lerp to reach the end value</param>
    /// <param name="start">The start value</param>
    /// <param name="end">The end value</param>
    /// <param name="funcLerp">The function used for lerping. If null, supports the following types: float, Vector2, Vector3, Vector4, Color, Color32 and Quaternion</param>
    public Lerp(float time, T start, T end, Func<T, T, float, T> funcLerp = null)
    {
        _time = time;
        _timeStart = GetTime();
        _timeEnd = _timeStart + _time;
        _valueStart = start;
        _valueEnd = end;

        _funcLerpClamped = funcLerp ??
            (
            (typeof(T) == typeof(float)) ? (Func<T, T, float, T>)(object)(Func<float, float, float, float>)Mathf.Lerp :
            (typeof(T) == typeof(Vector2)) ? (Func<T, T, float, T>)(object)(Func<Vector2, Vector2, float, Vector2>)Vector2.Lerp : 
            (typeof(T) == typeof(Vector3)) ? (Func<T, T, float, T>)(object)(Func<Vector3, Vector3, float, Vector3>)Vector3.Lerp : 
            (typeof(T) == typeof(Vector4)) ? (Func<T, T, float, T>)(object)(Func<Vector4, Vector4, float, Vector4>)Vector4.Lerp : 
            (typeof(T) == typeof(Color)) ? (Func<T, T, float, T>)(object)(Func<Color, Color, float, Color>)Color.Lerp : 
            (typeof(T) == typeof(Color32)) ? (Func<T, T, float, T>)(object)(Func<Color32, Color32, float, Color32>)Color32.Lerp : 
            (typeof(T) == typeof(Quaternion)) ? (Func<T, T, float, T>)(object)(Func<Quaternion, Quaternion, float, Quaternion>)Quaternion.Lerp : 
            null
            );

        _funcLerpUnclamped = funcLerp ??
            (
            (typeof(T) == typeof(float)) ? (Func<T, T, float, T>)(object)(Func<float, float, float, float>)Mathf.LerpUnclamped :
            (typeof(T) == typeof(Vector2)) ? (Func<T, T, float, T>)(object)(Func<Vector2, Vector2, float, Vector2>)Vector2.LerpUnclamped :
            (typeof(T) == typeof(Vector3)) ? (Func<T, T, float, T>)(object)(Func<Vector3, Vector3, float, Vector3>)Vector3.LerpUnclamped :
            (typeof(T) == typeof(Vector4)) ? (Func<T, T, float, T>)(object)(Func<Vector4, Vector4, float, Vector4>)Vector4.LerpUnclamped :
            (typeof(T) == typeof(Color)) ? (Func<T, T, float, T>)(object)(Func<Color, Color, float, Color>)Color.LerpUnclamped :
            (typeof(T) == typeof(Color32)) ? (Func<T, T, float, T>)(object)(Func<Color32, Color32, float, Color32>)Color32.LerpUnclamped :
            (typeof(T) == typeof(Quaternion)) ? (Func<T, T, float, T>)(object)(Func<Quaternion, Quaternion, float, Quaternion>)Quaternion.LerpUnclamped :
            null
            );

        _funcCurve = (float t) => t; // Linear
    }

    /// <summary>
    /// Restarts the lerp from current time
    /// </summary>
    public void Restart()
    {
        _timeStart = GetTime();
        _timeEnd = _timeStart + _time;

        if (_oscillate)
        {
            var temp = _valueStart;
            _valueStart = _valueEnd;
            _valueEnd = temp;
        }
    }

    /// <summary>
    /// Gets the current lerp value
    /// </summary>
    /// <returns>The lerp value</returns>
    T GetLerp()
    {
        return _unclamped ?
            _funcLerpUnclamped(_valueStart, _valueEnd, _funcCurve(TimePercentage())) :
            _funcLerpClamped(_valueStart, _valueEnd, _funcCurve(TimePercentage()));
    }

    /// <summary>
    /// Gets the current time percentage between start and end time
    /// </summary>
    /// <returns>The time percentage</returns>
    float TimePercentage()
    {
        return (GetTime() - _timeStart) / (_timeEnd - _timeStart);
    }

    /// <summary>
    /// Checks whether the lerp has ended, and restarts if looping
    /// </summary>
    /// <returns>True if has ended and is not looping, else false</returns>
    public bool HasEnded()
    {
        bool timeFinished = GetTime() >= _timeEnd;
        if (timeFinished && _loops != 0)
        {
            if(_loops > 0) _loops--;
            Restart();
            return false;
        }

        return timeFinished;
    }

    /// <summary>
    /// Checks whether the bound object exists and is active
    /// </summary>
    /// <returns>True if exists and is active</returns>
    public bool IsObjectActive()
    {
        return cancelWith != null && cancelWith.activeInHierarchy;
    }

    /// <summary>
    /// Gets the current time
    /// </summary>
    /// <returns>The time</returns>
    float GetTime()
    {
        return Time.time;
    }

    #region INTERFACE FUNCTIONS
    public string GetID()
    {
        return id;
    }

    public void Apply()
    {
        if (GetTime() < _timeStart) return; // Lerp hasn't started yet
        if (!IsObjectActive()) return; // Object is inactive/inexistent
        onApply?.Invoke(GetLerp());
    }

    public void End()
    {
        _timeEnd = GetTime(); // Set the end time to the current time

        if (IsObjectActive())
        {
            onEnd?.Invoke();
        }
    }

    public ILerp Curve(Func<float, float> funcCurve)
    {
        _funcCurve = funcCurve;
        return this;
    }

    public ILerp Curve(Lerp.Curve curve)
    {
        _funcCurve = Lerp.GetCurve(curve);
        return this;
    }

    public ILerp Curve(AnimationCurve animCurve)
    {
        _funcCurve = (float t) => animCurve.Evaluate(t);
        return this;
    }

    public ILerp OnEnd(Action onEnd)
    {
        this.onEnd = onEnd;
        return this;
    }

    public ILerp Loop(int loops = -1)
    {
        _loops = loops - 1;
        return this;
    }

    public ILerp Oscillate()
    {
        _oscillate = true;
        return this;
    }

    public ILerp Delay(float time)
    {
        _time += time;
        _timeStart += time;
        _timeEnd += time;
        return this;
    }

    public ILerp Reset()
    {
        _timeStart = GetTime();
        _timeEnd = _timeStart + _time;
        return this;
    }

    public ILerp Unclamp()
    {
        _unclamped = true;
        return this;
    }

    public IEnumerator GetEnumerator() => (IEnumerator)(new LerpEnumerator(this));
    #endregion
}
#endregion
#region LERP
public class Lerp : MonoBehaviour
{
    private enum TypeTransform { POSITION, ROTATION, EULER, SCALE } // Enum used to create transform lerps
    private enum TypeGraphic { COLOR, ALPHA } // Enum used to create graphic lerps
    public enum Axis { ALL, X, Y, Z } // Enum used to create transform lerps on an axis
    public enum Curve { LINEAR, EASE_END, EASE_START, EXPONENTIAL, SMOOTHSTEP, SMOOTHERSTEP, SINE, SINE_2 } // Predefined lerp curve types
    private static Lerp instance; // The lerp instance running the coroutines

    private const bool DEBUG = false; // Enable to print debug messages

    private Dictionary<string, IEnumerator> dicCoroutines = new Dictionary<string, IEnumerator>(); // Dictionary of <lerp id, coroutine>

    #region HELPER CLASSES
    private class LerpObjectWrapper<V, O>
    {
        public GameObject gameObject;
        public int InstanceID;
        public string id;
        public Func<V> Get;
        public Action<V> Set;
    }

    private class LerpTransformWrapper<V, O> : LerpObjectWrapper<V, O>
    {
        public LerpTransformWrapper(TypeTransform type, O target, Axis axis, bool local)
        {
            // Get & Set
            if(typeof(O) == typeof(Transform))
            {
                Transform t = (Transform)(object)target;
                gameObject = t.gameObject;
                InstanceID = t.GetInstanceID();

                Get =
                    (type == TypeTransform.POSITION && axis == Axis.ALL) ? (Func<V>)(object)(local ? (Func<Vector3>)(() => t.localPosition) : () => t.position) :
                    (type == TypeTransform.POSITION && axis == Axis.X) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localPosition.x) : () => t.position.x) :
                    (type == TypeTransform.POSITION && axis == Axis.Y) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localPosition.y) : () => t.position.y) :
                    (type == TypeTransform.POSITION && axis == Axis.Z) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localPosition.z) : () => t.position.z) :

                    type == TypeTransform.ROTATION ? (Func<V>)(object)(local ? (Func<Quaternion>)(() => t.localRotation) : () => t.rotation) :

                    (type == TypeTransform.EULER && axis == Axis.ALL) ? (Func<V>)(object)(local ? (Func<Vector3>)(() => t.localEulerAngles) : () => t.eulerAngles) :
                    (type == TypeTransform.EULER && axis == Axis.X) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localEulerAngles.x) : () => t.eulerAngles.x) :
                    (type == TypeTransform.EULER && axis == Axis.Y) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localEulerAngles.y) : () => t.eulerAngles.y) :
                    (type == TypeTransform.EULER && axis == Axis.Z) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localEulerAngles.z) : () => t.eulerAngles.z) :

                    (type == TypeTransform.SCALE && axis == Axis.ALL) ? (Func<V>)(object)(Func<Vector3>)(() => t.localScale) :
                    (type == TypeTransform.SCALE && axis == Axis.X) ? (Func<V>)(object)(Func<float>)(() => t.localScale.x) :
                    (type == TypeTransform.SCALE && axis == Axis.Y) ? (Func<V>)(object)(Func<float>)(() => t.localScale.y) :
                    (type == TypeTransform.SCALE && axis == Axis.Z) ? (Func<V>)(object)(Func<float>)(() => t.localScale.z) :

                    null;

                Set =
                    (type == TypeTransform.POSITION && axis == Axis.ALL) ? (Action<V>)(object)(local ? (Action<Vector3>)((Vector3 v) => t.localPosition = v) : (Vector3 v) => t.position = v) :
                    (type == TypeTransform.POSITION && axis == Axis.X) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localPosition = new Vector3(f, t.localPosition.y, t.localPosition.z)) : (float f) => t.position = new Vector3(f, t.position.y, t.position.z)) :
                    (type == TypeTransform.POSITION && axis == Axis.Y) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localPosition = new Vector3(t.localPosition.x, f, t.localPosition.z)) : (float f) => t.position = new Vector3(t.position.x, f, t.position.z)) :
                    (type == TypeTransform.POSITION && axis == Axis.Z) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, f)) : (float f) => t.position = new Vector3(t.position.x, t.position.y, f)) :

                    type == TypeTransform.ROTATION ? (Action<V>)(object)(local ? (Action<Quaternion>)((Quaternion q) => t.localRotation = q) : (Quaternion q) => t.rotation = q) :

                    (type == TypeTransform.EULER && axis == Axis.ALL) ? (Action<V>)(object)(local ? (Action<Vector3>)((Vector3 v) => t.localEulerAngles = v) : (Vector3 v) => t.eulerAngles = v) :
                    (type == TypeTransform.EULER && axis == Axis.X) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localEulerAngles = new Vector3(f, t.localEulerAngles.y, t.localEulerAngles.z)) : (float f) => t.eulerAngles = new Vector3(f, t.eulerAngles.y, t.eulerAngles.z)) :
                    (type == TypeTransform.EULER && axis == Axis.Y) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localEulerAngles = new Vector3(t.localEulerAngles.x, f, t.localEulerAngles.z)) : (float f) => t.eulerAngles = new Vector3(t.eulerAngles.x, f, t.eulerAngles.z)) :
                    (type == TypeTransform.EULER && axis == Axis.Z) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, f)) : (float f) => t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, f)) :

                    (type == TypeTransform.SCALE && axis == Axis.ALL) ? (Action<V>)(object)(Action<Vector3>)((Vector3 v) => t.localScale = v) :
                    (type == TypeTransform.SCALE && axis == Axis.X) ? (Action<V>)(object)(Action<float>)((float f) => t.localScale = new Vector3(f, t.localScale.y, t.localScale.z)) :
                    (type == TypeTransform.SCALE && axis == Axis.Y) ? (Action<V>)(object)(Action<float>)((float f) => t.localScale = new Vector3(t.localScale.x, f, t.localScale.z)) :
                    (type == TypeTransform.SCALE && axis == Axis.Z) ? (Action<V>)(object)(Action<float>)((float f) => t.localScale = new Vector3(t.localScale.x, t.localScale.y, f)) :

                    null;
            }
            else if(typeof(O) == typeof(RectTransform))
            {
                RectTransform t = (RectTransform)(object)target;
                gameObject = t.gameObject;
                InstanceID = t.GetInstanceID();

                Get =
                    (type == TypeTransform.POSITION && axis == Axis.ALL) ? (Func<V>)(object)(local ? (Func<Vector3>)(() => t.localPosition) : () => t.anchoredPosition) :
                    (type == TypeTransform.POSITION && axis == Axis.X) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localPosition.x) : () => t.anchoredPosition.x) :
                    (type == TypeTransform.POSITION && axis == Axis.Y) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localPosition.y) : () => t.anchoredPosition.y) :

                    type == TypeTransform.ROTATION ? (Func<V>)(object)(local ? (Func<Quaternion>)(() => t.localRotation) : () => t.rotation) :

                    (type == TypeTransform.EULER && axis == Axis.ALL) ? (Func<V>)(object)(local ? (Func<Vector3>)(() => t.localEulerAngles) : () => t.eulerAngles) :
                    (type == TypeTransform.EULER && axis == Axis.X) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localEulerAngles.x) : () => t.eulerAngles.x) :
                    (type == TypeTransform.EULER && axis == Axis.Y) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localEulerAngles.y) : () => t.eulerAngles.y) :
                    (type == TypeTransform.EULER && axis == Axis.Z) ? (Func<V>)(object)(local ? (Func<float>)(() => t.localEulerAngles.z) : () => t.eulerAngles.z) :

                    (type == TypeTransform.SCALE && axis == Axis.ALL) ? (Func<V>)(object)(Func<Vector3>)(() => t.sizeDelta) :
                    (type == TypeTransform.SCALE && axis == Axis.X) ? (Func<V>)(object)(Func<float>)(() => t.sizeDelta.x) :
                    (type == TypeTransform.SCALE && axis == Axis.Y) ? (Func<V>)(object)(Func<float>)(() => t.sizeDelta.y) :
                    null;

                Set =
                    (type == TypeTransform.POSITION && axis == Axis.ALL) ? (Action<V>)(object)(local ? (Action<Vector3>)((Vector3 v) => t.localPosition = v) : (Vector3 v) => t.position = v) :
                    (type == TypeTransform.POSITION && axis == Axis.X) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localPosition = new Vector3(f, t.localPosition.y, t.localPosition.z)) : (float f) => t.anchoredPosition = new Vector3(f, t.anchoredPosition.y)) :
                    (type == TypeTransform.POSITION && axis == Axis.Y) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localPosition = new Vector3(t.localPosition.x, f, t.localPosition.z)) : (float f) => t.anchoredPosition = new Vector3(t.anchoredPosition.x, f)) :

                    type == TypeTransform.ROTATION ? (Action<V>)(object)(local ? (Action<Quaternion>)((Quaternion q) => t.localRotation = q) : (Quaternion q) => t.rotation = q) :

                    (type == TypeTransform.EULER && axis == Axis.ALL) ? (Action<V>)(object)(local ? (Action<Vector3>)((Vector3 v) => t.localEulerAngles = v) : (Vector3 v) => t.eulerAngles = v) :
                    (type == TypeTransform.EULER && axis == Axis.X) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localEulerAngles = new Vector3(f, t.localEulerAngles.y, t.localEulerAngles.z)) : (float f) => t.eulerAngles = new Vector3(f, t.eulerAngles.y, t.eulerAngles.z)) :
                    (type == TypeTransform.EULER && axis == Axis.Y) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localEulerAngles = new Vector3(t.localEulerAngles.x, f, t.localEulerAngles.z)) : (float f) => t.eulerAngles = new Vector3(t.eulerAngles.x, f, t.eulerAngles.z)) :
                    (type == TypeTransform.EULER && axis == Axis.Z) ? (Action<V>)(object)(local ? (Action<float>)((float f) => t.localEulerAngles = new Vector3(t.localEulerAngles.x, t.localEulerAngles.y, f)) : (float f) => t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, f)) :

                    (type == TypeTransform.SCALE && axis == Axis.ALL) ? (Action<V>)(object)(Action<Vector3>)((Vector3 v) => t.localScale = v) :
                    (type == TypeTransform.SCALE && axis == Axis.X) ? (Action<V>)(object)(Action<float>)((float f) => t.sizeDelta = new Vector3(f, t.sizeDelta.y)) :
                    (type == TypeTransform.SCALE && axis == Axis.Y) ? (Action<V>)(object)(Action<float>)((float f) => t.sizeDelta = new Vector3(t.sizeDelta.x, f)) :

                    null;
            }

            if (Get == null) print(string.Concat("Lerp; Unable to handle Get(", type.ToString(), " ", axis.ToString(), ") for object: ", target));
            if (Set == null) print(string.Concat("Lerp; Unable to handle Set(", type.ToString(), " ", axis.ToString(), ") for object: ", target));

            // ID
            id =
                type == TypeTransform.POSITION ? "position_" :
                type == TypeTransform.ROTATION ? "rotation_" :
                type == TypeTransform.EULER ? "rotation_" :
                type == TypeTransform.SCALE ? "scale_" :
                "";

            id +=
                axis == Axis.X ? "x_" :
                axis == Axis.Y ? "y_" :
                axis == Axis.Z ? "z_" :
                "";

            id += InstanceID;
        }
    }

    private class LerpGraphicWrapper<V,O> : LerpObjectWrapper<V,O>
    {
        public LerpGraphicWrapper(TypeGraphic type, O target)
        {
            if(typeof(O) == typeof(Graphic))
            {
                Graphic t = (Graphic)(object)target;
                gameObject = t.gameObject;
                InstanceID = t.GetInstanceID();

                Get =
                    type == TypeGraphic.COLOR ? (Func<V>)(object)(Func<Color>)(() => t.color) :
                    type == TypeGraphic.ALPHA ? (Func<V>)(object)(Func<float>)(() => t.color.a) :
                    null;

                Set =
                    type == TypeGraphic.COLOR ? (Action<V>)(object)(Action<Color>)((Color c) => t.color = c) :
                    type == TypeGraphic.ALPHA ? (Action<V>)(object)(Action<float>)((float a) => t.color = new Color(t.color.r, t.color.g, t.color.b, a)) :
                    null;
            }
            else if (typeof(O) == typeof(SpriteRenderer))
            {
                SpriteRenderer t = (SpriteRenderer)(object)target;
                gameObject = t.gameObject;
                InstanceID = t.GetInstanceID();

                Get =
                    type == TypeGraphic.COLOR ? (Func<V>)(object)(Func<Color>)(() => t.color) :
                    type == TypeGraphic.ALPHA ? (Func<V>)(object)(Func<float>)(() => t.color.a) :
                    null;

                Set =
                    type == TypeGraphic.COLOR ? (Action<V>)(object)(Action<Color>)((Color c) => t.color = c) :
                    type == TypeGraphic.ALPHA ? (Action<V>)(object)(Action<float>)((float a) => t.color = new Color(t.color.r, t.color.g, t.color.b, a)) :
                    null;
            }
            else if (typeof(O) == typeof(Image))
            {
                Image t = (Image)(object)target;
                gameObject = t.gameObject;
                InstanceID = t.GetInstanceID();

                Get =
                    type == TypeGraphic.COLOR ? (Func<V>)(object)(Func<Color>)(() => t.color) :
                    type == TypeGraphic.ALPHA ? (Func<V>)(object)(Func<float>)(() => t.color.a) :
                    null;

                Set =
                    type == TypeGraphic.COLOR ? (Action<V>)(object)(Action<Color>)((Color c) => t.color = c) :
                    type == TypeGraphic.ALPHA ? (Action<V>)(object)(Action<float>)((float a) => t.color = new Color(t.color.r, t.color.g, t.color.b, a)) :
                    null;
            }
            else if (typeof(O) == typeof(TMP_Text))
            {
                TMP_Text t = (TMP_Text)(object)target;
                gameObject = t.gameObject;
                InstanceID = t.GetInstanceID();

                Get =
                    type == TypeGraphic.COLOR ? (Func<V>)(object)(Func<Color>)(() => t.color) :
                    type == TypeGraphic.ALPHA ? (Func<V>)(object)(Func<float>)(() => t.alpha) :
                    null;

                Set =
                    type == TypeGraphic.COLOR ? (Action<V>)(object)(Action<Color>)((Color c) => t.color = c) :
                    type == TypeGraphic.ALPHA ? (Action<V>)(object)(Action<float>)((float a) => t.alpha = a) :
                    null;
            }

            else if (typeof(O) == typeof(CanvasGroup))
            {
                CanvasGroup t = (CanvasGroup)(object)target;
                gameObject = t.gameObject;
                InstanceID = t.GetInstanceID();

                Get =
                    type == TypeGraphic.ALPHA ? (Func<V>)(object)(Func<float>)(() => t.alpha) :
                    null;

                Set =
                    type == TypeGraphic.ALPHA ? (Action<V>)(object)(Action<float>)((float a) => t.alpha = a) :
                    null;
            }

            if (Get == null) print(string.Concat("Lerp; Unable to handle Get(", type.ToString(), ") for object: ", target));
            if (Set == null) print(string.Concat("Lerp; Unable to handle Set(", type.ToString(), ") for object: ", target));

            // ID
            id =
                type == TypeGraphic.COLOR ? "color_" :
                type == TypeGraphic.ALPHA ? "alpha_" :
                "";

            id += InstanceID;
        }
    }
    #endregion
    #region STATIC
    /// <summary>
    /// Creates and initializes the Lerp instance, which runs the coroutines
    /// </summary>
    static void Initialize()
    {
        if(instance == null)
        {
            GameObject g = new GameObject();
            g.name = "Lerp";
            instance = g.AddComponent<Lerp>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    /// <summary>
    /// Runs a lerp in a coroutine
    /// </summary>
    /// <typeparam name="T">The lerp type</typeparam>
    /// <param name="lerp">The lerp</param>
    public static ILerp Run<T>(Lerp<T> lerp)
    {
        if (instance == null) Initialize();
        instance.Run(LerpCoroutine(lerp), lerp.GetID());
        return lerp;
    }

    /// <summary>
    /// The lerp coroutine
    /// </summary>
    /// <param name="lerp">The lerp</param>
    /// <returns></returns>
    static IEnumerator LerpCoroutine(ILerp lerp)
    {
        yield return lerp.GetEnumerator();
        yield return null;
        lerp.Apply();
        lerp.End();

        // Remove lerp references
        instance.Remove(lerp.GetID());
    }

    /// <summary>
    /// Kills a lerp
    /// </summary>
    /// <param name="lerp">The lerp</param>
    public static void Kill(ILerp lerp)
    {
        string id = lerp.GetID();
        if(instance.HasID(id))
            instance.Kill(id);
    }

    /// <summary>
    /// Gets a unique ID for a lerp value
    /// </summary>
    /// <returns></returns>
    public static string GetUniqueID()
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Gets a curve function used for lerping
    /// </summary>
    /// <param name="curve">The curve type</param>
    /// <returns>The curve function</returns>
    public static Func<float, float> GetCurve(Curve curve)
    {
        switch (curve)
        {
            case Curve.LINEAR: return (float t) => t;
            case Curve.EASE_END: return (float t) => Mathf.Sin(t * Mathf.PI * 0.5f);
            case Curve.EASE_START: return (float t) => 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
            case Curve.EXPONENTIAL: return (float t) => t * t;
            case Curve.SMOOTHSTEP: return (float t) => t * t * (3f - 2f * t);
            case Curve.SMOOTHERSTEP: return (float t) => t * t * t * (t * (6f * t - 15f) + 10f);
            case Curve.SINE: return (float t) => Mathf.Sin(Mathf.PI * t);
            case Curve.SINE_2: return (float t) => Mathf.Sin(Mathf.PI * t * 2);
            default: return (float t) => t;
        }
    }
    #endregion
    #region INSTANCE
    /// <summary>
    /// Runs a coroutine and maps it with a given id
    /// </summary>
    /// <param name="coroutine">The coroutine</param>
    /// <param name="id">The id</param>
    public void Run(IEnumerator coroutine, string id)
    {
        // If any lerp with this id exists, kill it first
        if (HasID(id)) Kill(id);

        // Start lerp coroutine
        if (DEBUG) print("Lerp.Run; " + id);
        dicCoroutines.Add(id, coroutine);
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// Removes coroutine with given id
    /// </summary>
    /// <param name="id">The id</param>
    public void Remove(string id)
    {
        dicCoroutines.Remove(id);
        if (DEBUG) Debug.Log("Lerp.Remove: " + id);
    }

    /// <summary>
    /// Kills and removes coroutine with given id
    /// </summary>
    /// <param name="id">The id</param>
    public void Kill(string id)
    {
        if (DEBUG) print("Lerp.Kill; " + id);

        StopCoroutine(dicCoroutines[id]);
        Remove(id);
    }

    /// <summary>
    /// Checks if any coroutine exists with a given id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>True if coroutine exists, else false</returns>
    public bool HasID(string id)
    {
        return dicCoroutines.ContainsKey(id);
    }
    #endregion
    #region CREATE
    static Lerp<V> Create<V,O>(O target, float time, V start, V end, bool local, TypeTransform type, Axis axis)
    {
        LerpObjectWrapper<V,O> wrapper = new LerpTransformWrapper<V,O>(type, target, axis, local);
        Lerp<V> lerp = new Lerp<V>(time, start, end);
        lerp.onApply = wrapper.Set;
        lerp.id = wrapper.id;
        lerp.cancelWith = wrapper.gameObject;
        return lerp;
    }

    static Lerp<V> Create<V, O>(O target, float time, V end, bool local, TypeTransform type, Axis axis)
    {
        LerpObjectWrapper<V, O> wrapper = new LerpTransformWrapper<V, O>(type, target, axis, local);
        V start = wrapper.Get();
        Lerp<V> lerp = new Lerp<V>(time, start, end);
        lerp.onApply = wrapper.Set;
        lerp.id = wrapper.id;
        lerp.cancelWith = wrapper.gameObject;
        return lerp;
    }

    static Lerp<V> Create<V, O>(O target, float time, V start, V end, TypeGraphic type)
    {
        LerpObjectWrapper<V, O> wrapper = new LerpGraphicWrapper<V, O>(type, target);
        Lerp<V> lerp = new Lerp<V>(time, start, end);
        lerp.onApply = wrapper.Set;
        lerp.id = wrapper.id;
        lerp.cancelWith = wrapper.gameObject;
        return lerp;
    }

    static Lerp<V> Create<V, O>(O target, float time, V end, TypeGraphic type)
    {
        LerpObjectWrapper<V, O> wrapper = new LerpGraphicWrapper<V, O>(type, target);
        V start = wrapper.Get();
        Lerp<V> lerp = new Lerp<V>(time, start, end);
        lerp.onApply = wrapper.Set;
        lerp.id = wrapper.id;
        lerp.cancelWith = wrapper.gameObject;
        return lerp;
    }
    #endregion
    #region TIMER
    /// <summary>
    /// Calls a function when the given time has passed
    /// </summary>
    /// <param name="time">The timer duration</param>
    /// <param name="onEnd">Function called when the timer ends</param>
    /// <param name="cancelWith">If this has been destroyed, cancel the timer</param>
    /// <returns>The lerp</returns>
    public static ILerp Timer(float time, Action onEnd, GameObject cancelWith)
    {
        var lerp = new Lerp<float>(time, 0, 1);
        lerp.cancelWith = cancelWith;
        lerp.id = "timer_" + GetUniqueID();
        return Run(lerp).OnEnd(onEnd);
    }
    #endregion
    #region VALUE
    /// <summary>
    /// Lerps a value
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="time">Lerp duration</param>
    /// <param name="start">The start value</param>
    /// <param name="end">The end value</param>
    /// <param name="onApply">The action to call each lerp frame</param>
    /// <param name="cancelWith">The GameObject to cancel the coroutine with</param>
    /// <param name="id">A value-lerp with this id will be overwritten by this lerp</param>
    /// <returns>The lerp</returns>
    public static ILerp Value<T>(float time, T start, T end, Action<T> onApply, GameObject cancelWith, string id = null)
    {
        var lerp = new Lerp<T>(time, start, end);
        lerp.cancelWith = cancelWith;
        lerp.id = id == null ? "value_" + GetUniqueID() : "value_" + id;
        lerp.onApply = onApply;
        return Run(lerp);
    }
    #endregion
    #region POSITION
    /// <summary>
    /// Lerps the position of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <typeparam name="T">The lerp type</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">The lerp duration</param>
    /// <param name="start">The start position</param>
    /// <param name="end">The end position</param>
    /// <param name="local">If position should be local or global</param>
    /// <returns>The lerp</returns>
    public static ILerp Position<T>(T transform, float time, Vector3 start, Vector3 end, bool local = false)
    {
        var lerp = Create(transform, time, start, end, local, TypeTransform.POSITION, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the position of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <typeparam name="T">The lerp type</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">The lerp duration</param>
    /// <param name="end">The end position</param>
    /// <param name="local">If position should be local or global</param>
    /// <returns>The lerp</returns>
    public static ILerp Position<T>(T transform, float time, Vector3 end, bool local = false)
    {
        var lerp = Create(transform, time, end, local, TypeTransform.POSITION, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the position of an object on an axis
    /// </summary>
    /// <typeparam name="T">Transform, RectTransform</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">The lerp duration</param>
    /// <param name="start">The start value</param>
    /// <param name="end">The end value</param>
    /// <param name="axis">The axis</param>
    /// <param name="local">If position should be local or global</param>
    /// <returns>The lerp</returns>
    public static ILerp Position<T>(T transform, float time, float start, float end, Axis axis, bool local = false)
    {
        var lerp = Create(transform, time, start, end, local, TypeTransform.POSITION, axis);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the position of an object on an axis
    /// </summary>
    /// <typeparam name="T">Transform, RectTransform</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">The lerp duration</param>
    /// <param name="end">The end value</param>
    /// <param name="axis">The axis</param>
    /// <param name="local">If position should be local or global</param>
    /// <returns>The lerp</returns>
    public static ILerp Position<T>(T transform, float time, float end, Axis axis, bool local = false)
    {
        var lerp = Create(transform, time, end, local, TypeTransform.POSITION, axis);
        return Run(lerp);
    }
    #endregion
    #region ROTATION
    /// <summary>
    /// Lerps the rotation of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <param name="transform">The object</param>
    /// <param name="time">Lerp duration</param>
    /// <param name="start">The start rotation</param>
    /// <param name="end">The end rotation</param>
    /// <returns>The lerp</returns>
    public static ILerp Rotation<T>(T transform, float time, Quaternion start, Quaternion end, bool local = false)
    {
        var lerp = Create(transform, time, start, end, local, TypeTransform.ROTATION, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the rotation of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <param name="transform">The object</param>
    /// <param name="time">Lerp duration</param>
    /// <param name="end">The end rotation</param>
    /// <returns>The lerp</returns>
    public static ILerp Rotation<T>(T transform, float time, Quaternion end, bool local = false)
    {
        var lerp = Create(transform, time, end, local, TypeTransform.ROTATION, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the euler angles of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <param name="transform">The object</param>
    /// <param name="time">Lerp duration</param>
    /// <param name="start">The start euler angles</param>
    /// <param name="end">The end euler angles</param>
    /// <returns>The lerp</returns>
    public static ILerp Euler<T>(T transform, float time, Vector3 start, Vector3 end, bool local = false)
    {
        var lerp = Create(transform, time, start, end, local, TypeTransform.EULER, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the euler angles of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <param name="transform">The object</param>
    /// <param name="time">Lerp duration</param>
    /// <param name="end">The end euler angles</param>
    /// <returns>The lerp</returns>
    public static ILerp Euler<T>(T transform, float time, Vector3 end, bool local = false)
    {
        var lerp = Create(transform, time, end, local, TypeTransform.EULER, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the euler angles of an object, on an axis
    /// </summary>
    /// <typeparam name="T">Transform, RectTransform</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">lerp duration</param>
    /// <param name="start">Start value</param>
    /// <param name="end">End value</param>
    /// <param name="axis">The axis</param>
    /// <param name="local">If local euler or global</param>
    /// <returns>The lerp</returns>
    public static ILerp Euler<T>(T transform, float time, float start, float end, Axis axis, bool local = false)
    {
        var lerp = Create(transform, time, start, end, local, TypeTransform.EULER, axis);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the euler angles of an object, on an axis
    /// </summary>
    /// <typeparam name="T">Transform, RectTransform</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">lerp duration</param>
    /// <param name="end">End value</param>
    /// <param name="axis">The axis</param>
    /// <param name="local">If local euler or global</param>
    /// <returns>The lerp</returns>
    public static ILerp Euler<T>(T transform, float time, float end, Axis axis, bool local = false)
    {
        var lerp = Create(transform, time, end, local, TypeTransform.EULER, axis);
        return Run(lerp);
    }
    #endregion
    #region SCALE
    /// <summary>
    /// Lerps the scale of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <typeparam name="T">The lerp type</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">The lerp duration</param>
    /// <param name="start">The start scale</param>
    /// <param name="end">The end scale</param>
    /// <returns>The lerp</returns>
    public static ILerp Scale<T>(T transform, float time, Vector3 start, Vector3 end)
    {
        var lerp = Create(transform, time, start, end, false, TypeTransform.SCALE, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the scale of an object. Supported types are: Transform, RectTransform
    /// </summary>
    /// <typeparam name="T">The lerp type</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">The lerp duration</param>
    /// <param name="end">The end scale</param>
    /// <returns>The lerp</returns>
    public static ILerp Scale<T>(T transform, float time, Vector3 end)
    {
        var lerp = Create(transform, time, end, false, TypeTransform.SCALE, Axis.ALL);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the scale of an object, on an axis
    /// </summary>
    /// <typeparam name="T">Transform, RectTransform</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">lerp duration</param>
    /// <param name="start">Start value</param>
    /// <param name="end">End value</param>
    /// <returns>The lerp</returns>
    public static ILerp Scale<T>(T transform, float time, float start, float end, Axis axis)
    {
        var lerp = Create(transform, time, start, end, false, TypeTransform.SCALE, axis);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the scale of an object, on an axis
    /// </summary>
    /// <typeparam name="T">Transform, RectTransform</typeparam>
    /// <param name="transform">The object</param>
    /// <param name="time">lerp duration</param>
    /// <param name="end">End value</param>
    /// <returns>The lerp</returns>
    public static ILerp Scale<T>(T transform, float time, float end, Axis axis)
    {
        var lerp = Create(transform, time, end, false, TypeTransform.SCALE, axis);
        return Run(lerp);
    }
    #endregion
    #region COLOR
    /// <summary>
    /// Lerps the color of a graphic element. Supported types are Graphic, SpriteRenderer, TMP_Text
    /// </summary>
    /// <param name="time">Lerp duration</param>
    /// <param name="graphic">The graphic element</param>
    /// <param name="start">The start color</param>
    /// <param name="end">The end color</param>
    /// <returns>The lerp</returns>
    public static ILerp Color<T>(T graphic, float time, Color start, Color end)
    {
        var lerp = Create(graphic, time, start, end, TypeGraphic.COLOR);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the color of a graphic element. Supported types are Graphic, SpriteRenderer, TMP_Text
    /// </summary>
    /// <param name="time">Lerp duration</param>
    /// <param name="graphic">The graphic element</param>
    /// <param name="end">The end color</param>
    /// <returns>The lerp</returns>
    public static ILerp Color<T>(T graphic, float time, Color end)
    {
        var lerp = Create(graphic, time, end, TypeGraphic.COLOR);
        return Run(lerp);
    }
    #endregion
    #region ALPHA
    /// <summary>
    /// Lerps the alpha of a graphic element. Supported types are Graphic, SpriteRenderer, TMP_Text
    /// </summary>
    /// <param name="time">Lerp duration</param>
    /// <param name="graphic">The Graphic</param>
    /// <param name="start">The start alpha</param>
    /// <param name="end">The end alpha</param>
    /// <returns>The lerp</returns>
    public static ILerp Alpha<T>(T graphic, float time, float start, float end)
    {
        var lerp = Create(graphic, time, start, end, TypeGraphic.ALPHA);
        return Run(lerp);
    }

    /// <summary>
    /// Lerps the alpha of a graphic element. Supported types are Graphic, SpriteRenderer, TMP_Text
    /// </summary>
    /// <param name="time">Lerp duration</param>
    /// <param name="graphic">The Graphic</param>
    /// <param name="end">The end alpha</param>
    /// <returns>The lerp</returns>
    public static ILerp Alpha<T>(T graphic, float time, float end)
    {
        var lerp = Create(graphic, time, end, TypeGraphic.ALPHA);
        return Run(lerp);
    }
    #endregion
}
#endregion