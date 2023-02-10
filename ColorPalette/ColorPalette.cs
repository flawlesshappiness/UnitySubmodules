using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "Game/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public List<ColorPaletteMap> maps = new List<ColorPaletteMap>();

    public Color Get(string name)
    {
        var m = maps.FirstOrDefault(m => m.name == name);
        return m != null ? m.color : Color.white;
    }
}