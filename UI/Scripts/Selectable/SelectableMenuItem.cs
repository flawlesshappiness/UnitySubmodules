using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableMenuItem : Selectable, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler
{
    [Header("SELECTABLE MENU ITEM")]
    [SerializeField] private bool select_on_start;
    [SerializeField] private SelectableAnimation select_animation;

    [SerializeField] private FMODEventReference sfx_select;

    public event System.Action onClick;

    public static SelectableMenuItem Selected { get; set; }

    protected override void Start()
    {
        if (Application.isPlaying && select_on_start)
        {
            EventSystemController.Instance.EventSystem.SetSelectedGameObject(gameObject);
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        select_animation.AnimateSelect();

        if(Selected != null && Selected != this)
        {
            sfx_select.Play();
        }

        Selected = this;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        select_animation.AnimateDeselect();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Select();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        onClick?.Invoke();
    }
}