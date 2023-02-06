using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

[System.Serializable]
public abstract class FakeEnum : IComparable {
    public string id;

    public FakeEnum(string id) {
        this.id = id;
    }

    public override string ToString() => id;

    public static IEnumerable<FakeEnum> GetAll(Type type) =>
        type.GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<FakeEnum>();

    public override bool Equals(object obj) {
        if (obj.GetType() != typeof(FakeEnum)) {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = id.Equals(((FakeEnum)obj).id);

        return typeMatches && valueMatches;
    }

    public static bool operator ==(FakeEnum A, FakeEnum B) {
        if (ReferenceEquals(A, B)) {
            return true;
        }

        if (ReferenceEquals(A, null)) {
            return false;
        }

        if (ReferenceEquals(B, null)) {
            return false;
        }

        return A.id.Equals(B.id);
    }

    public static bool operator !=(FakeEnum A, FakeEnum B) {
        if(ReferenceEquals(A, null)) {
            return !ReferenceEquals(B, null);
        }

        if(ReferenceEquals(B, null)) {
            return !ReferenceEquals(A, null);
        }

        return !(A.id == B.id);
    }

    public int CompareTo(object other) =>
        String.Compare(id, ((FakeEnum)other).id, StringComparison.Ordinal);
}
