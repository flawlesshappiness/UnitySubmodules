using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public abstract class View : MonoBehaviourExtended
{
    protected Canvas Canvas { get { return GetComponentOnce<Canvas>(ComponentSearchType.PARENT); } }
    protected CanvasGroup CanvasGroup { get { return GetComponentOnce<CanvasGroup>(ComponentSearchType.THIS); } }
    protected RectTransform Root { get { return GetComponentOnce<RectTransform>(ComponentSearchType.THIS); } }
    public bool Interactable { set { CanvasGroup.interactable = value; CanvasGroup.blocksRaycasts = value; } }

    public void Close(float time)
    {
        if(time == 0)
        {
            OnClose();
        }
        else
        {
            StartCoroutine(CloseCr(time));
        }
    }

    private IEnumerator CloseCr(float time)
    {
        yield return Lerp.Alpha(CanvasGroup, time, 0f)
            .UnscaledTime()
            .GetCoroutine();
        OnClose();
    }

    private void OnClose()
    {
        Destroy(gameObject);
    }

    public void Show(float time)
    {
        if(time == 0)
        {
            CanvasGroup.alpha = 1f;
        }
        else
        {
            Lerp.Alpha(CanvasGroup, time, 0f, 1f)
            .UnscaledTime();
        }
    }
}