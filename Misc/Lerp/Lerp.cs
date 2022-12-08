using System.Collections;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Flawliz.Lerp
{
    public class Lerp : CustomYieldInstruction
    {
        private static readonly GUID guid = GUID.Generate();

        private CustomCoroutine coroutine;
        private LerpEnumerator enumerator;
        
        public override bool keepWaiting => coroutine.keepWaiting;
        public bool Ended { get { return coroutine.Completed; } }

        private Lerp(CustomCoroutine coroutine, LerpEnumerator enumerator) 
        {
            this.coroutine = coroutine;
            this.enumerator = enumerator;
        }

        public Lerp UnscaledTime()
        {
            enumerator.UnscaledTime = true;
            return this;
        }

        public Lerp Curve(AnimationCurve curve)
        {
            enumerator.AnimationCurve = curve;
            return this;
        }

        public Lerp Connect(GameObject connection)
        {
            enumerator.Connection = connection;
            enumerator.IsConnected = connection != null;
            return this;
        }

        public void Kill() => coroutine.Kill();

        /// <summary>
        /// Runs a lerp in a coroutine
        /// </summary>
        /// <typeparam name="T">The lerp type</typeparam>
        /// <param name="lerp">The lerp</param>
        private static Lerp Run(LerpEnumerator enumerator, string id, GameObject connection = null)
        {
            var coroutine = CoroutineController.Instance.Run(Cr(), id);
            var lerp = new Lerp(coroutine, enumerator);
            lerp.Connect(connection);
            return lerp;

            IEnumerator Cr()
            {
                yield return enumerator;
                CoroutineController.Instance.Kill(id);
            }
        }

        private static string GetID(string name, Object connection = null)
        {
            var id = $"{guid}_lerp_{name}";

            if(connection != null)
            {
                id += $"_{connection.GetInstanceID()}";
            }

            return id;
        }

        private static string GetMethodName([CallerMemberName] string caller = null) => caller;

        // Value
        public static Lerp Value(string id, float duration, float start, float end, System.Action<float> lerp_function) =>
            Run(LerpEnumerator.Value(duration, start, end, lerp_function), GetID($"{GetMethodName()}_{id}"));

        public static Lerp Value(string id, float duration, System.Action<float> lerp_function) =>
            Run(LerpEnumerator.Value(duration, lerp_function), GetID($"{GetMethodName()}_{id}"));

        public static Lerp Value(float duration, System.Action<float> lerp_function) =>
            Run(LerpEnumerator.Value(duration, lerp_function), GetID($"{GetMethodName()}_{GUID.Generate()}"));

        // Position
        public static Lerp Position(Transform target, float duration, Vector3 start, Vector3 end) =>
            Run(LerpEnumerator.Position(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Position(Transform target, float duration, Vector3 end) =>
            Run(LerpEnumerator.Position(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Local Position
        public static Lerp LocalPosition(Transform target, float duration, Vector3 start, Vector3 end) =>
            Run(LerpEnumerator.LocalPosition(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp LocalPosition(Transform target, float duration, Vector3 end) =>
            Run(LerpEnumerator.LocalPosition(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Rotation
        public static Lerp Rotation(Transform target, float duration, Quaternion start, Quaternion end) =>
            Run(LerpEnumerator.Rotation(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Rotation(Transform target, float duration, Quaternion end) =>
            Run(LerpEnumerator.Rotation(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Local Rotation
        public static Lerp LocalRotation(Transform target, float duration, Quaternion start, Quaternion end) =>
            Run(LerpEnumerator.LocalRotation(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp LocalRotation(Transform target, float duration, Quaternion end) =>
            Run(LerpEnumerator.LocalRotation(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Euler
        public static Lerp Euler(Transform target, float duration, Vector3 start, Vector3 end) =>
            Run(LerpEnumerator.Euler(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp LocalEuler(Transform target, float duration, Vector3 start, Vector3 end) =>
            Run(LerpEnumerator.LocalEuler(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Local Scale
        public static Lerp LocalScale(Transform target, float duration, Vector3 start, Vector3 end) =>
            Run(LerpEnumerator.LocalScale(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp LocalScale(Transform target, float duration, Vector3 end) =>
            Run(LerpEnumerator.LocalScale(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Size Delta
        public static Lerp SizeDelta(RectTransform target, float duration, Vector2 start, Vector2 end) =>
            Run(LerpEnumerator.SizeDelta(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp SizeDelta(RectTransform target, float duration, Vector2 end) =>
            Run(LerpEnumerator.SizeDelta(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Anchored Position
        public static Lerp AnchoredPosition(RectTransform target, float duration, Vector2 start, Vector2 end) =>
            Run(LerpEnumerator.AnchoredPosition(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp AnchoredPosition(RectTransform target, float duration, Vector2 end) =>
            Run(LerpEnumerator.AnchoredPosition(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Color
        public static Lerp Color(Graphic target, float duration, Color start, Color end) =>
            Run(LerpEnumerator.Color(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Color(Graphic target, float duration, Color end) =>
            Run(LerpEnumerator.Color(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Color(SpriteRenderer target, float duration, Color start, Color end) =>
            Run(LerpEnumerator.Color(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Color(SpriteRenderer target, float duration, Color end) =>
            Run(LerpEnumerator.Color(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        // Alpha
        public static Lerp Alpha(Graphic target, float duration, float start, float end) =>
            Run(LerpEnumerator.Alpha(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Alpha(Graphic target, float duration, float end) =>
            Run(LerpEnumerator.Alpha(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Alpha(SpriteRenderer target, float duration, float start, float end) =>
            Run(LerpEnumerator.Alpha(target, duration, start, end), GetID($"{GetMethodName()}", target), target.gameObject);

        public static Lerp Alpha(SpriteRenderer target, float duration, float end) =>
            Run(LerpEnumerator.Alpha(target, duration, end), GetID($"{GetMethodName()}", target), target.gameObject);
    }
}