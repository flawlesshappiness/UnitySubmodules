using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleView : MonoBehaviourExtended
{
    private CanvasGroup cvg { get { return GetComponentOnce<CanvasGroup>(); } }
    public void SetVisible(bool visible)
    {
        cvg.alpha = visible ? 1 : 0;
        cvg.blocksRaycasts = visible;
    }
}
