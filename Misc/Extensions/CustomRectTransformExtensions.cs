using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CustomRectTransformExtensions {

	public static GameObject SpawnChildPrefabUI(this RectTransform parent, GameObject prefab)
	{
		var g = GameObject.Instantiate(prefab) as GameObject;
		g.transform.SetParent(parent);
		g.transform.localScale = Vector3.one;
		g.transform.position = parent.position;
		return g;
	}

	public static void SetButtonsEnabled(this RectTransform parent, bool enabled)
	{
		var buttons = parent.GetComponentsInChildren<Button>();
		foreach(Button b in buttons)
		{
			b.interactable = enabled;
		}
	}


    /// The following were found on https://stackoverflow.com/questions/26423549/how-to-modify-recttransform-properties-in-script-unity-4-6-beta
    /// February 27th, 2020
    /// Originally taken from https://orbcreation.com/cgi-bin/orbcreation/page.pl?1099

    public static void SetDefaultScale(this RectTransform trans)
    {
        trans.localScale = new Vector3(1, 1, 1);
    }
    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }
    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }
    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }

    public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }
    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }
}
