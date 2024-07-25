using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MeleeRotation : MonoBehaviour
{
    [SerializeField] private float _freeSpeedRotate = 10;
    [SerializeField] private MeleeWeapon _meleeWeapon;
    [SerializeField] private CheckerForAttack _checkerForAttack;
    [SerializeField] private BaseMovement _baseMovement;
    [SerializeField] private GameObject _bodyForRotate;
    [SerializeField] private ImpactDirection _impactDirection;
    [SerializeField] private Rig _rightArmRig;

    private Quaternion _playerRotation;

    private void Update() => Rotation();

    private void Rotation()
    {
         if ((_meleeWeapon.AnimAttackIsPlaying == false || _impactDirection.CanDirectPlayerAttack) && _baseMovement.PlayerDirection.magnitude > 0.5f)
             _playerRotation = Quaternion.LookRotation(new Vector3(_baseMovement.PlayerDirection.x,0,_baseMovement.PlayerDirection.z), Vector3.up);

         _bodyForRotate.transform.rotation = Quaternion.Lerp(_bodyForRotate.transform.rotation, _playerRotation, _freeSpeedRotate * Time.deltaTime);
         ArmRigSwitchChecher();
    }
    
    private void ArmRigSwitchChecher()
    {    
        if (_baseMovement.OnGround)
            _rightArmRig.weight += Time.deltaTime * 1;
        else
            _rightArmRig.weight = 0;   
    }
}
