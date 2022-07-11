using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemButton : Button
{
    private bool Selected { get; set; }
    public System.Action<bool> OnSelectionChanged { get; set; }
    public System.Action OnSelected { get; set; }
    public System.Action OnDeselected { get; set; }

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
            Selected = selected;
            OnSelectionChanged?.Invoke(Selected);

            if (selected)
            {
                OnSelected?.Invoke();
            }
            else
            {
                OnDeselected?.Invoke();
            }
        }
    }
}