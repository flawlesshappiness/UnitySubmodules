using UnityEngine;
using UnityEngine.UI;

public class ColorPaletteGraphic : ColorPaletteTarget
{
    public Graphic graphic;

    public override void UpdateTargetColor(Color color)
    {
        if (graphic == null) return;
        graphic.color = color;
    }

    public override void OnValidate()
    {
        base.OnValidate();
        graphic = graphic ?? GetComponent<Graphic>();
        UpdateTargetColor(value.GetColor());
    }
}