using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{   
    [HideInInspector] public Vector2 TouchDist;
    [HideInInspector] public Vector2 PointerOld;
    [HideInInspector] public int PointerId;

    private void OnEnable() => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();      

    public void OnDrag(PointerEventData eventData)
    {
        TouchDist = new Vector2(eventData.position.x, eventData.position.y) - PointerOld;
        PointerOld = eventData.position;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        PointerId = eventData.clickCount;
        PointerOld = eventData.position;
    }

    public virtual void OnPointerUp(PointerEventData eventData) => TouchDist = Vector2.zero; 
}
