using DG.Tweening;
using UnityEngine;

public class ForceOfAttraction : MonoBehaviour
{
    [SerializeField] private CheckerForAttack _attackChecker;     
    [HideInInspector] public Vector3 Direction;

    private void ForceAsideEnemy() // using in Events(Animation)
    {
        if (_attackChecker.AtZoneForAttack)
        {
            transform.DOLookAt(Direction, .2f);
            transform.DOMove(Direction, .3f);
        }
    }
}
