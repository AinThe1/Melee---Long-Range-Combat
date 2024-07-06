using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [HideInInspector] public bool IsJump;
    [SerializeField] private TouchField _touchField;

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchField.PointerId = eventData.pointerId;
        _touchField.PointerOld = eventData.position;

        IsJump = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _touchField.TouchDist = new Vector2();
        IsJump = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _touchField.TouchDist = new Vector2(eventData.position.x, eventData.position.y) - _touchField.PointerOld;
        _touchField.PointerOld = eventData.position;
    }
}
