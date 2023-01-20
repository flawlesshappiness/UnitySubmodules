using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Flawliz.Lerp
{
    public class LerpEnumerator : IEnumerator
    {
        private IEnumerator enumerator;
        public bool UnscaledTime { get; set; }
        public AnimationCurve AnimationCurve { get; set; }
        public GameObject Connection { get; set; }
        public bool IsConnected { get; set; }

        private LerpEnumerator() { }
        public object Current => enumerator.Current;
        public bool MoveNext() => enumerator.MoveNext();
        public void Reset() => enumerator.Reset();
        
        public LerpEnumerator Curve(AnimationCurve curve)
        {
            AnimationCurve = curve;
            return this;
        }

        public LerpEnumerator Connect(GameObject connection)
        {
            Connection = connection;
            IsConnected = connection != null;
            return this;
        }


        public static LerpEnumerator Value(float duration, float start, float end, System.Action<float> lerp_function)
        {
            var le = new LerpEnumerator();
            var e = Enumerator();
            le.enumerator = e;
            return le;

            IEnumerator Enumerator()
            {
                var time_start_scaled = Time.time;
                var time_start_unscaled = Time.unscaledTime;
                var time_end_scaled = time_start_scaled + duration;
                var time_end_unscaled = time_start_unscaled + duration;
                while (HasConnection() && IsRunning())
                {
                    var t = (GetTime() - GetStartTime()) / duration;
                    var t_curve = GetCurve().Evaluate(t);
                    var v_lerp = Mathf.LerpUnclamped(start, end, t_curve);
                    lerp_function(v_lerp);
                    yield return null;
                }

                if (HasConnection())
                {
                    lerp_function(end);
                }

                bool IsUnscaledTime() => le.UnscaledTime;
                bool IsRunning() => GetTime() < GetEndTime();
                float GetTime() => IsUnscaledTime() ? Time.unscaledTime : Time.time;
                float GetStartTime() => IsUnscaledTime() ? time_start_unscaled : time_start_scaled;
                float GetEndTime() => IsUnscaledTime() ? time_end_unscaled : time_end_scaled;
                bool HasConnection() => !le.IsConnected || (GetConnection() != null && GetConnection().activeInHierarchy);
                AnimationCurve GetCurve() => le.AnimationCurve != null ? le.AnimationCurve : AnimationCurve.Linear(0, 0, 1, 1);
                GameObject GetConnection() => le.Connection;
            }
        }

        public static LerpEnumerator Value(float duration, System.Action<float> lerp_function) =>
            Value(duration, 0f, 1f, lerp_function);

        // Position
        public static LerpEnumerator Position(Transform transform, float duration, Vector3 end) =>
            Position(transform, duration, transform.position, end);
        public static LerpEnumerator Position(Transform transform, float duration, Vector3 start, Vector3 end) =>
            Value(duration, f => transform.position = Vector3.LerpUnclamped(start, end, f))
            .Connect(transform.gameObject);

        // Local Position
        public static LerpEnumerator LocalPosition(Transform transform, float duration, Vector3 end) =>
            Position(transform, duration, transform.localPosition, end);
        public static LerpEnumerator LocalPosition(Transform transform, float duration, Vector3 start, Vector3 end) =>
            Value(duration, f => transform.localPosition = Vector3.LerpUnclamped(start, end, f))
            .Connect(transform.gameObject);

        // Rotation
        public static LerpEnumerator Rotation(Transform transform, float duration, Quaternion end) =>
            Rotation(transform, duration, transform.rotation, end);
        public static LerpEnumerator Rotation(Transform transform, float duration, Quaternion start, Quaternion end) =>
            Value(duration, f => transform.rotation = Quaternion.LerpUnclamped(start, end, f))
            .Connect(transform.gameObject);

        // Local Rotation
        public static LerpEnumerator LocalRotation(Transform transform, float duration, Quaternion end) =>
            LocalRotation(transform, duration, transform.localRotation, end);
        public static LerpEnumerator LocalRotation(Transform transform, float duration, Quaternion start, Quaternion end) =>
            Value(duration, f => transform.localRotation = Quaternion.LerpUnclamped(start, end, f))
            .Connect(transform.gameObject);

        // Euler
        public static LerpEnumerator Euler(Transform transform, float duration, Vector3 end) =>
            Euler(transform, duration, transform.eulerAngles, end);
        public static LerpEnumerator Euler(Transform transform, float duration, Vector3 start, Vector3 end) =>
            Value(duration, f => transform.eulerAngles = Vector3.LerpUnclamped(start, end, f))
            .Connect(transform.gameObject);

        // Local Euler
        public static LerpEnumerator LocalEuler(Transform transform, float duration, Vector3 end) =>
            LocalEuler(transform, duration, transform.localEulerAngles, end);
        public static LerpEnumerator LocalEuler(Transform transform, float duration, Vector3 start, Vector3 end) =>
            Value(duration, f => transform.localEulerAngles = Vector3.LerpUnclamped(start, end, f))
            .Connect(transform.gameObject);

        // Local Scale
        public static LerpEnumerator LocalScale(Transform transform, float duration, Vector3 end) =>
            LocalScale(transform, duration, transform.localScale, end);
        public static LerpEnumerator LocalScale(Transform transform, float duration, Vector3 start, Vector3 end) =>
            Value(duration, f => transform.localScale = Vector3.LerpUnclamped(start, end, f))
            .Connect(transform.gameObject);

        // Size Delta
        public static LerpEnumerator SizeDelta(RectTransform rectTransform, float duration, Vector2 end) =>
            SizeDelta(rectTransform, duration, rectTransform.sizeDelta, end);
        public static LerpEnumerator SizeDelta(RectTransform rectTransform, float duration, Vector2 start, Vector2 end) =>
            Value(duration, f => rectTransform.sizeDelta = Vector2.LerpUnclamped(start, end, f))
            .Connect(rectTransform.gameObject);

        // Anchored Position
        public static LerpEnumerator AnchoredPosition(RectTransform rectTransform, float duration, Vector2 end) =>
            AnchoredPosition(rectTransform, duration, rectTransform.anchoredPosition, end);
        public static LerpEnumerator AnchoredPosition(RectTransform rectTransform, float duration, Vector2 start, Vector2 end) =>
            Value(duration, f => rectTransform.anchoredPosition = Vector2.LerpUnclamped(start, end, f))
            .Connect(rectTransform.gameObject);

        // Color
        public static LerpEnumerator Color(Graphic graphic, float duration, Color end) =>
            Color(graphic, duration, graphic.color, end);
        public static LerpEnumerator Color(Graphic graphic, float duration, Color start, Color end) =>
            Value(duration, f => graphic.color = UnityEngine.Color.LerpUnclamped(start, end, f))
            .Connect(graphic.gameObject);

        public static LerpEnumerator Color(SpriteRenderer spr, float duration, Color end) =>
            Color(spr, duration, spr.color, end);
        public static LerpEnumerator Color(SpriteRenderer spr, float duration, Color start, Color end) =>
            Value(duration, f => spr.color = UnityEngine.Color.LerpUnclamped(start, end, f))
            .Connect(spr.gameObject);

        // Alpha
        public static LerpEnumerator Alpha(Graphic graphic, float duration, float end) =>
            Alpha(graphic, duration, graphic.color.a, end);
        public static LerpEnumerator Alpha(Graphic graphic, float duration, float start, float end)
        {
            var c = graphic.color;
            var c_start = c.SetA(start);
            var c_end = c.SetA(end);
            return Color(graphic, duration, c_start, c_end);
        }

        public static LerpEnumerator Alpha(SpriteRenderer spr, float duration, float end) =>
            Alpha(spr, duration, spr.color.a, end);
        public static LerpEnumerator Alpha(SpriteRenderer spr, float duration, float start, float end)
        {
            var c = spr.color;
            var c_start = c.SetA(start);
            var c_end = c.SetA(end);
            return Color(spr, duration, c_start, c_end);
        }
    }
}