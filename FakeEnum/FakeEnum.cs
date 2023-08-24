using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[Serializable]
public abstract class FakeEnum : IComparable
{
    public string id;

    public FakeEnum(string id)
    {
        this.id = id;
    }

    public override string ToString() => id;

    public static IEnumerable<FakeEnum> GetAll(Type type) =>
        type.GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<FakeEnum>();

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = id.Equals(((FakeEnum)obj).id);

        return typeMatches && valueMatches;
    }

    public static bool operator ==(FakeEnum A, FakeEnum B)
    {
        if (A is null && B is null) return true;
        if (A is null || B is null) return false;
        return A.id.Equals(B.id);
    }

    public static bool operator !=(FakeEnum A, FakeEnum B)
    {
        if (A is null && B is null) return false;
        if (A is null) return true;
        if (B is null) return true;
        return !(A.id == B.id);
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }

    public int CompareTo(object other) =>
        String.Compare(id, (other as FakeEnum).id, StringComparison.Ordinal);
}
