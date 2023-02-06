using UnityEngine;
using UnityEngine.EventSystems;

public class LeftRightMenuItem : SelectableMenuItem
{
    [Header("LEFT RIGHT MENU ITEM")]
    public SelectableLeftRightAnimation left_right_animation;

    public event System.Action onLeft;
    public event System.Action onRight;
    public event System.Action<int> onMove;

    public override void OnMove(AxisEventData eventData)
    {
        if(eventData.moveDir == MoveDirection.Left)
        {
            left_right_animation.AnimateLeft();
            onLeft?.Invoke();
            onMove?.Invoke(-1);
        }
        else if(eventData.moveDir == MoveDirection.Right)
        {
            left_right_animation.AnimateRight();
            onRight?.Invoke();
            onMove?.Invoke(1);
        }
        else
        {
            base.OnMove(eventData);
        }
    }
}