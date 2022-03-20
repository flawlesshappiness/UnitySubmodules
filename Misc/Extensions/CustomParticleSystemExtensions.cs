using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomParticleSystemExtensions {
    public static void Adjust(this ParticleSystem ps, System.Action<ParticleSystem.MainModule, ParticleSystem.EmissionModule> adjust)
    {
        adjust(ps.main, ps.emission);
    }

    public class Parent
    {
        private Transform parent;
        private bool inheritPosition;
        private bool inheritRotation;
        private bool inheritScale;
        private bool twoD;

        public Parent(Transform parent, bool twoD, bool inheritPosition, bool inheritRotation, bool inheritScale)
        {
            this.parent = parent;
            this.twoD = twoD;
            this.inheritPosition = inheritPosition;
            this.inheritRotation = inheritRotation;
            this.inheritScale = inheritScale;
        }

        public void Adjust(ParticleSystem ps)
        {
            var t = ps.transform;
            t.SetParent(parent);
            if (inheritPosition) t.position = parent.position;
            if (inheritRotation) t.rotation = twoD ? Quaternion.Euler(parent.rotation.z + 90f, 90f, -90f) : parent.rotation;
            if (inheritScale) t.localScale = parent.localScale;
        }
    }

    /// <summary>
    /// Instantiates a ParticleSystem and adjusts its Transform
    /// </summary>
    /// <param name="prefab">ParticleSystem to instantiate</param>
    /// <param name="parent">Parent settings for the ParticleSystem. If Null, does nothing.</param>
    /// <param name="position">Custom position</param>
    /// <returns></returns>
    public static ParticleSystem InstantiatePrefab(this ParticleSystem prefab, Parent parent, Vector3 position)
    {
        GameObject g = GameObject.Instantiate(prefab.gameObject) as GameObject;
        ParticleSystem ps = g.GetComponent<ParticleSystem>();
        if (!ps.main.loop) GameObject.Destroy(g, ps.main.duration);

        ps.transform.position = position;
        if(parent != null) parent.Adjust(ps);

        return ps;
    }
}
