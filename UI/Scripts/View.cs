using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviourExtended
{
    protected CanvasGroup CanvasGroup { get { return GetComponentOnce<CanvasGroup>(ComponentSearchType.THIS); } }

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