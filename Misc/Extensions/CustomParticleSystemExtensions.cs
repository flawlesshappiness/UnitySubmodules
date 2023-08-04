using UnityEngine;

public static class CustomParticleSystemExtensions
{
    public static ParticleSystemDuplicate Duplicate(this ParticleSystem ps)
    {
        return new ParticleSystemDuplicate(ps);
    }

    public static void SetEmissionEnabled(this ParticleSystem ps, bool enabled)
    {
        if (enabled && !ps.isPlaying)
        {
            ps.Play();
        }

        ps.ModifyEmission(m => m.enabled = enabled);
    }

    public static void ModifyEmission(this ParticleSystem ps, System.Action<ParticleSystem.EmissionModule> action, bool affectChildren = true) => ps.Modify(ps => action(ps.emission), affectChildren);
    public static void ModifyMain(this ParticleSystem ps, System.Action<ParticleSystem.MainModule> action, bool affectChildren = true) => ps.Modify(ps => action(ps.main), affectChildren);
    public static void ModifyCollision(this ParticleSystem ps, System.Action<ParticleSystem.CollisionModule> action, bool affectChildren = true) => ps.Modify(ps => action(ps.collision), affectChildren);
    public static void ModifyTrails(this ParticleSystem ps, System.Action<ParticleSystem.TrailModule> action, bool affectChildren = true) => ps.Modify(ps => action(ps.trails), affectChildren);
    public static void ModifyShape(this ParticleSystem ps, System.Action<ParticleSystem.ShapeModule> action, bool affectChildren = true) => ps.Modify(ps => action(ps.shape), affectChildren);
    public static void ModifyRenderer(this ParticleSystem ps, System.Action<ParticleSystemRenderer> action, bool affectChildren = true) => ps.Modify(ps => action(ps.GetComponent<ParticleSystemRenderer>()), affectChildren);
    private static void Modify(this ParticleSystem ps, System.Action<ParticleSystem> action, bool affectChildren)
    {
        if (affectChildren)
        {
            foreach (var child in ps.GetComponentsInChildren<ParticleSystem>())
            {
                action(child);
            }
        }
        else
        {
            action(ps);
        }
    }
}

public class ParticleSystemDuplicate
{
    public ParticleSystem ps;

    public ParticleSystemDuplicate(ParticleSystem ps)
    {
        this.ps = Object.Instantiate(ps.gameObject).GetComponent<ParticleSystem>();
        this.ps.gameObject.SetActive(true);
        var t = ps.transform;
        Position(t.position);
        Rotation(t.rotation);
        Scale(t.localScale);
        Parent(t.parent);
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
