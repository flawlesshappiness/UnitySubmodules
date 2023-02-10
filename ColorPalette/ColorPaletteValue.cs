using UnityEngine;

[System.Serializable]
public class ColorPaletteValue
{
    public bool editor_update;
    public int palette_index;
    public string palette_name;
    public int map_index;
    public string map_name;
    public Color color;
    public Color GetColor() => color;
}