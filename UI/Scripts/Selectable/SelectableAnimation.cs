using UnityEngine;

public abstract class SelectableAnimation : MonoBehaviour
{
    public abstract CustomCoroutine AnimateSelect();
    public abstract CustomCoroutine AnimateDeselect(); 
}