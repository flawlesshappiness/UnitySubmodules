using UnityEngine;

namespace Flawliz.VisualConsole
{
    public abstract class VisualConsoleWindow : MonoBehaviour
    {
        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public virtual void Clear() { }
    }
}