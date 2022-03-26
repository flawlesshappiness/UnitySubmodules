using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviourExtended
{
    private CanvasGroup _cvg { get { return GetComponentOnce<CanvasGroup>(ComponentSearchType.THIS); } }

    public void Close(float time)
    {
        StartCoroutine(CloseCr(time));
    }

    private IEnumerator CloseCr(float time)
    {
        yield return Lerp.Alpha(_cvg, time, 0f).GetEnumerator();
        Destroy(gameObject);
    }

    public void Show(float time)
    {
        Lerp.Alpha(_cvg, time, 0f, 1f);
    }
}