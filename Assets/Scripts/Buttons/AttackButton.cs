using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private TouchField _touchField;
    [HideInInspector] public bool IsAttack;

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchField.PointerId = eventData.pointerId;
        _touchField.PointerOld = eventData.position;
        IsAttack = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsAttack = false;
        _touchField.TouchDist = new Vector2();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _touchField.TouchDist = new Vector2(eventData.position.x, eventData.position.y) - _touchField.PointerOld;
        _touchField.PointerOld = eventData.position;
    }
}
