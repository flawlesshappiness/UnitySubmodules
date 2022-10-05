using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtended : Button
{
    public event System.Action<bool> OnHoverChanged;
    public event System.Action<bool> OnSelectedChanged;
    protected bool Selected { get; set; }
    protected bool Hovered { get; set; }

    private bool _select_on_hover;

    private void Update()
    {
        if (Application.isPlaying)
        {
            SelectionUpdate();
        }
    }

    private void SelectionUpdate()
    {
        var selected = EventSystemController.Instance.EventSystem.currentSelectedGameObject == gameObject;
        if (Selected != selected)
        {
            if(Selected != selected)
            {
                OnSelectedChanged?.Invoke(selected);
            }

            Selected = selected;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (!Hovered)
        {
            OnHoverChanged?.Invoke(true);
        }

        Hovered = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (Hovered)
        {
            OnHoverChanged?.Invoke(false);
        }

        Hovered = false;
    }

    public void SetSelectOnHover(bool select)
    {
        if(select && !_select_on_hover)
        {
            OnHoverChanged += SelectOnHover;
            _select_on_hover = true;
        }
        else if(!select && _select_on_hover)
        {
            OnHoverChanged -= SelectOnHover;
            _select_on_hover = false;
        }
    }

    private void SelectOnHover(bool hovered)
    {
        if (hovered)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}