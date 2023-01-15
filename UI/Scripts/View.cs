using System.Collections;
using UnityEngine;
using Flawliz.Lerp;

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
        var lerp = LerpEnumerator.Value(time, f =>
        {
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 0f, f);
        });
        lerp.UnscaledTime = true;
        yield return lerp;
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
            Lerp.Value(time, f => CanvasGroup.alpha = Mathf.Lerp(0f, 1f, f))
                .UnscaledTime();
        }
    }
}