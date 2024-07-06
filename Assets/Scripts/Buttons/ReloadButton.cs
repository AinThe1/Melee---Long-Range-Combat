using UnityEngine;
using UnityEngine.EventSystems;

public class ReloadButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private TouchField _touchField;
    private Weapon[] _gun;

    public void MyStart() => _gun = FindObjectsOfType<Weapon>();
  

    public void OnPointerUp(PointerEventData eventData) => _touchField.TouchDist = new Vector2();

    public void OnDrag(PointerEventData eventData)
    {
        _touchField.TouchDist = new Vector2(eventData.position.x, eventData.position.y) - _touchField.PointerOld;
        _touchField.PointerOld = eventData.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _touchField.PointerId = eventData.pointerId;
        _touchField.PointerOld = eventData.position;
        foreach (Weapon shoting in _gun)
            shoting.ButtonReload();
    }
}
