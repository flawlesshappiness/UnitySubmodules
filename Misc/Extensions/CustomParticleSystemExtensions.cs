using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class CustomParticleSystemExtensions {
    public static ParticleSystemDuplicate Duplicate(this ParticleSystem ps)
    {
        return new ParticleSystemDuplicate(ps);
    }

    public static void ModifyEmission(this ParticleSystem ps, System.Action<ParticleSystem.EmissionModule> action) => action(ps.emission);
    public static void ModifyMain(this ParticleSystem ps, System.Action<ParticleSystem.MainModule> action) => action(ps.main);
    public static void ModifyCollision(this ParticleSystem ps, System.Action<ParticleSystem.CollisionModule> action) => action(ps.collision);
    public static void ModifyTrails(this ParticleSystem ps, System.Action<ParticleSystem.TrailModule> action) => action(ps.trails);
    public static void ModifyShape(this ParticleSystem ps, System.Action<ParticleSystem.ShapeModule> action) => action(ps.shape);
}

public class ParticleSystemDuplicate
{
    public ParticleSystem ps;

    public ParticleSystemDuplicate(ParticleSystem ps)
    {
        this.ps = Object.Instantiate(ps.gameObject).GetComponent<ParticleSystem>();
    }

    public ParticleSystemDuplicate Play()
    {
        ps.Play();
        return this;
    }

    public ParticleSystemDuplicate Position(Vector3 position)
    {
        ps.transform.position = position;
        return this;
    }

    public ParticleSystemDuplicate Rotation(Quaternion rotation)
    {
        ps.transform.rotation = rotation;
        return this;
    }

    public ParticleSystemDuplicate Euler(Vector3 euler)
    {
        ps.transform.eulerAngles = euler;
        return this;
    }

    public ParticleSystemDuplicate Euler(float x, float y, float z)
    {
        ps.transform.eulerAngles = new Vector3(x, y, z);
        return this;
    }

    public ParticleSystemDuplicate Scale(Vector3 scale)
    {
        ps.transform.localScale = scale;
        return this;
    }

    public ParticleSystemDuplicate Scale(float x, float y, float z)
    {
        ps.transform.localScale = new Vector3(x, y, z);
        return this;
    }

    public ParticleSystemDuplicate Parent(Transform parent)
    {
        ps.transform.parent = parent;
        return this;
    }

    public ParticleSystemDuplicate Destroy(float time)
    {
        Object.Destroy(ps.gameObject, time);
        return this;
    }
}
