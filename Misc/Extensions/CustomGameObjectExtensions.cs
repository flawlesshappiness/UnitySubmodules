using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class CustomGameObjectExtensions {
	public static T GetInterface<T>(this GameObject g)
	{
		return g.GetComponents<Component>().OfType<T>().FirstOrDefault();
	}

	public static IEnumerable<T> GetInterfaces<T>(this GameObject g)
	{
		return g.GetComponents<Component>().OfType<T>();
	}

	public static IEnumerable<T> GetInterfacesInChildren<T>(this GameObject g)
	{
		return g.GetComponentsInChildren<Component>().OfType<T>();
	}

	public static bool IsLastSibling(GameObject g)
	{
		var t = g.transform;
		var p = t.parent;
		return p.GetSiblingIndex() == p.childCount-1;
	}

	public static bool IsFirstSibling(this GameObject g)
	{
		return g.transform.GetSiblingIndex() == 0;
	}
}
