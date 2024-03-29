using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableMenuItem : Selectable, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler
{
    [Header("SELECTABLE MENU ITEM")]
    public bool submittable = true;
    [SerializeField] private bool select_on_start;
    [SerializeField] private CanvasGroup cvg;
    [SerializeField] private SelectableAnimation select_animation;

    public event System.Action onSubmit;
    public event System.Action onSelect;
    public event System.Action onDeselect;

    public CanvasGroup CanvasGroup { get { return cvg; } }
    public static SelectableMenuItem Selected { get; set; }

    protected void Reset()
    {
        transition = Transition.None;
    }

    protected void OnValidate()
    {
        if(cvg == null)
        {
            cvg = GetComponent<CanvasGroup>();
        }
    }

    protected override void Start()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (select_on_start)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if(EventSystem.current.currentSelectedGameObject == null)
        {
            if(Selected != null)
            {
                EventSystem.current.SetSelectedGameObject(Selected.gameObject);
            }
        }
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        if(select_animation != null)
        {
            select_animation.AnimateSelect();
        }

        if (Selected != null && Selected != this)
        {
            onSelect?.Invoke();
        }

        Selected = this;
    }

    public static void RemoveSelection()
    {
        if(Selected != null)
        {
            Selected.Deselect();
            Selected = null;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        Deselect();
        onDeselect?.Invoke();
    }

    public void Deselect()
    {
        if(select_animation != null)
        {
            select_animation.AnimateDeselect();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Submit();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Select();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Submit();
    }

    private void Submit()
    {
        if (IsInteractable() && submittable)
        {
            onSubmit?.Invoke();
        }
    }
}