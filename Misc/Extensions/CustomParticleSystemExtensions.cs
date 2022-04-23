using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomParticleSystemExtensions {
    public static ParticleSystemDuplicate Duplicate(this ParticleSystem ps)
    {
        return new ParticleSystemDuplicate(ps);
    }
}

public class ParticleSystemDuplicate
{
    private ParticleSystem ps;
    private ParticleSystem.MainModule mm;
    private ParticleSystem.EmissionModule em;
    private ParticleSystem.CollisionModule cm;

    public ParticleSystemDuplicate(ParticleSystem ps)
    {
        this.ps = Object.Instantiate(ps.gameObject).GetComponent<ParticleSystem>();
        this.mm = ps.main;
        this.em = ps.emission;
        this.cm = ps.collision;
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

    public ParticleSystemDuplicate ModifyEmission(System.Action<ParticleSystem.EmissionModule> action)
    {
        action(em);
        return this;
    }

    public ParticleSystemDuplicate ModifyMain(System.Action<ParticleSystem.MainModule> action)
    {
        action(mm);
        return this;
    }

    public ParticleSystemDuplicate ModifyCollision(System.Action<ParticleSystem.CollisionModule> action)
    {
        action(cm);
        return this;
    }

    public ParticleSystemDuplicate ModifySystem(System.Action<ParticleSystem> action)
    {
        action(ps);
        return this;
    }
}
