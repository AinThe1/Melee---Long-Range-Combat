using UnityEngine;
using UnityEngine.EventSystems;

public class ShotingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private TouchField _touchField;
    [SerializeField] private AimButton _AimButton;
    [HideInInspector] public bool Shoot;

    public void OnPointerDown(PointerEventData eventData)
    {             
        _touchField.PointerId = eventData.pointerId;
        _touchField.PointerOld = eventData.position;
        
        _AimButton.OnAim = true;
        Shoot = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _touchField.TouchDist = new Vector2();
        if (!_AimButton.OnHoldDownAim)
        _AimButton.OnAim = false;
        Shoot = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _touchField.TouchDist = new Vector2(eventData.position.x, eventData.position.y) - _touchField.PointerOld;
        _touchField.PointerOld = eventData.position;
    }
}
