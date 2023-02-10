using UnityEngine;

public abstract class ColorPaletteTarget : MonoBehaviour
{
    public ColorPaletteValue value = new ColorPaletteValue();
    public abstract void UpdateTargetColor(Color color);

    public virtual void OnValidate()
    {
        if (value.editor_update)
        {
            value.editor_update = false;
            UpdateTargetColor(value.GetColor());
        }
    }
}