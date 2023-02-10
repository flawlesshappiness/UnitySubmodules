using UnityEditor;
using UnityEngine;

public static class CreateColorPaletteEditor
{
    [MenuItem("Create/ColorPalette", false, 1)]
    [MenuItem("Assets/Create/ColorPalette/new ColorPalette", false, 1)]
    public static void NameColorPaletteObject()
    {
        NameObjectWindow.Show("newColorPalette", CreateColorPalette);
    }

    private static void CreateColorPalette(string name)
    {
        var dir = EditorPaths.COLOR_PALETTE_OBJECTS;
        ExtraEditorUtility.EnsureDirectoryExists(dir);

        var path = $"{dir}/{name}.asset";
        var inst = ScriptableObject.CreateInstance<ColorPalette>();
        AssetDatabase.CreateAsset(inst, path);
        AssetDatabase.SaveAssets();
    }
}