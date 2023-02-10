using UnityEditor;
using UnityEngine;

public static class ColorPaletteEditor
{
    [MenuItem("Editor/Color Palette/Update selected", false, 1)]
    public static void PrintObjects()
    {
        var g = Selection.activeGameObject;
        var targets = g.GetComponentsInChildren<ColorPaletteTarget>();
        foreach (var target in targets)
        {
            target.UpdateTargetColor(target.value.GetColor());
        }
        EditorApplication.QueuePlayerLoopUpdate();
        Debug.Log($"ColorPaletteTargets updated: {targets.Length}");
    }
}