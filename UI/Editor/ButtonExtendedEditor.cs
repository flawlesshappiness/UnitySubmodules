using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonExtendedEditor
{
    [MenuItem("GameObject/UI/Button - Extended", true)]
    public static bool CreateValidate()
    {
        return Selection.activeTransform != null;
    }

    [MenuItem("GameObject/UI/Button - Extended", false, 0)]
    public static void Create()
    {
        var selected = Selection.activeGameObject;
        var g = new GameObject("Button", typeof(RectTransform));
        GameObjectUtility.SetParentAndAlign(g, selected);
        g.transform.position = selected.transform.position;
        g.transform.rotation = selected.transform.rotation;

        var img = g.AddComponent<Image>();
        var btn = g.AddComponent<ButtonExtended>();
        btn.image = img;

        Selection.activeGameObject = g;

        Undo.RegisterCreatedObjectUndo(g, "Parented " + g.name);
    }
}