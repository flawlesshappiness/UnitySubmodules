using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomStringExtensions {
	public static string Color(this string s, Color32 color)
	{
		return "<color=#"+color.ToHex()+">"+s+"</color>";
	}

	public static string Bold(this string s)
	{
		return "<b>"+s+"</b>";
	}

	public static string Italic(this string s)
	{
		return "<i>"+s+"</i>";
	}

	public static Vector2 GetSize(this string s)
	{
		return GUI.skin.label.CalcSize(new GUIContent(s));
	}

	public static Vector2 GetSize(this string s, GUIStyle style)
	{
		return style.CalcSize(new GUIContent(s));
	}

	public static void PrintValues(this string[] ss)
	{
		string s = "String array(" + ss.Length + ")[";
		for (int i = 0; i < ss.Length; i++) 
		{
			if(i > 0) s += ", " + ss[i];
			else s += ss[i];
		}
		s += "]";
		Debug.Log(s);
	}
}
