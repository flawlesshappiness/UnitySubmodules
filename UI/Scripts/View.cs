using System.Collections;
using UnityEngine;
using Flawliz.Lerp;

public abstract class View : MonoBehaviourExtended
{
    protected Canvas Canvas { get { return GetComponentOnce<Canvas>(ComponentSearchType.PARENT); } }
    protected CanvasGroup CanvasGroup { get { return GetComponentOnce<CanvasGroup>(ComponentSearchType.THIS); } }
    protected RectTransform Root { get { return GetComponentOnce<RectTransform>(ComponentSearchType.THIS); } }
    public bool Interactable { set { CanvasGroup.interactable = value; CanvasGroup.blocksRaycasts = value; } }

    public void Close(float duration)
    {
        if(duration == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Lerp.Alpha(CanvasGroup, duration, 0f)
                .Connect(CanvasGroup.gameObject)
                .UnscaledTime();
            Destroy(gameObject, duration);
        }
    }

    public void Show(float duration)
    {
        if(duration == 0)
        {
            CanvasGroup.alpha = 1f;
        }
        else
        {
            Lerp.Alpha(CanvasGroup, duration, 0f, 1f)
                .Connect(CanvasGroup.gameObject)
                .UnscaledTime();
        }
    }
}