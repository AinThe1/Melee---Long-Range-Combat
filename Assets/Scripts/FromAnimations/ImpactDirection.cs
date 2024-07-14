using DG.Tweening;
using UnityEngine;

public class ImpactDirection : MonoBehaviour
{
    private Vector3 _moveDirection;
    private bool _canDirectPlayerAttack;
    public Vector3 MoveDirection { get { return _moveDirection ;} set {_moveDirection = value ;}}
    public bool CanDirectPlayerAttack { get { return _canDirectPlayerAttack ;}}

    private void ForceImpactDirection() // using in Events(Animation)
    {
        _canDirectPlayerAttack = true;
        Invoke("DisableBool",.1f);
        //transform.DOMove(_moveDirection, .3f);
    }

    private void DisableBool() => _canDirectPlayerAttack = false;
}
