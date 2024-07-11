using UnityEngine;

public class MeleeRotation : MonoBehaviour
{
    [SerializeField] private float _freeSpeedRotate = 10;
    [SerializeField] private MeleeWeapon _meleeWeapon;
    [SerializeField] private CheckerForAttack _checkerForAttack;
    [SerializeField] private BaseMovement _baseMovement;
    [SerializeField] private GameObject _bodyForRotate;

    private Quaternion _playerRotation;

    private void Update() => Rotation();

    private void Rotation()
    {
        if (_meleeWeapon.AnimAttackIsPlaying == false && _baseMovement.FunctionMove.magnitude >= 0.05f)
            _playerRotation = Quaternion.LookRotation(_baseMovement.FunctionMove, Vector3.up);
    
        if (_meleeWeapon.AnimAttackIsPlaying == true && _meleeWeapon.DirectionAttack != Vector3.zero && _checkerForAttack.Target != null)
        {
            Vector3 direction = (_checkerForAttack.Target.transform.position - transform.position).normalized;
            _playerRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        _bodyForRotate.transform.rotation = Quaternion.Lerp(_bodyForRotate.transform.rotation, _playerRotation, _freeSpeedRotate * Time.deltaTime);
    }
}
