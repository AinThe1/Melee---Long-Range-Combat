using UnityEngine;
using UnityEngine.EventSystems;

public class AimButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private TransitionBetweenAiming _MainCamera;
    [SerializeField] private ShotingButton _shootingButton;
    [SerializeField] private TouchField _touchField;
    [HideInInspector] public bool OnAim = false;
    [HideInInspector] public bool OnHoldDownAim = false;

    public void MyStart() => _MainCamera = FindObjectOfType<TransitionBetweenAiming>();

    public void ButtonAim()
    {
        if (_MainCamera.IsAim)
        {
            _MainCamera.Aiming();
            OnAim = true;
            OnHoldDownAim = true;
        }
               
        else if (_MainCamera.ExitAim)
        {
            _MainCamera.ExitAiming();
            OnAim = false;
            OnHoldDownAim = false;
        }
    }

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

        if (_shootingButton.Shoot == false)
            ButtonAim();
    }
}
