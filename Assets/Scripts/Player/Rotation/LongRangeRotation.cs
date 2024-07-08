using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LongRangeRotation : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _freeSpeedRotate = 10;
    [SerializeField] private float _aimSpeedRotate = 20;
    [SerializeField] private float _aimConstraintLiftSpeed = 3;

    [Header("CameraLookMovement")]
    [SerializeField] private Transform _transformMainCamera;
    [SerializeField] private TransitionBetweenAiming _mainCamera;
    [SerializeField] private GameObject _bodyForRotate;
    [SerializeField] private Weapon _gun;

    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Rig _spineRig;
    [SerializeField] private Rig _handRig;
    [SerializeField] private MultiAimConstraint _multiAimConstraint;

    private int _targetRigWeight;
    private int _isAimHash;
    private Quaternion _playerRotation;
    private PlayerControl _inputSystemControl;
    private float _aimConstraintLift;
    private BaseMovement _baseMovement;

    private void OnEnable() => _inputSystemControl.Enable();
    private void OnDisable() => _inputSystemControl.Disable();
    private void Awake() => _inputSystemControl = new PlayerControl();
    private void Start() => MyStart();
    private void Update() => Rotation();

    private void MyStart()
    {
        _baseMovement = GetComponent<BaseMovement>();
        _isAimHash = Animator.StringToHash("IsAim");
    }

    private void Rotation()
    {
        var InputMouseLeft = _inputSystemControl.Player.Shoot.IsPressed();
        var InputMouseRight = _inputSystemControl.Player.Aim.IsPressed();
        float currentSpeedRotation = 0;

        //360 rotation without direct player at camera
        if (!InputMouseLeft && !InputMouseRight && _gun.OnReloading == false)
        {
            _targetRigWeight = 0;
            _handRig.weight = 1;
            _mainCamera.ExitAiming();
            _anim.SetBool(_isAimHash, false);
            currentSpeedRotation = _freeSpeedRotate;
            Debug.Log("1");
        }

        //360 rotation with direct player at camera
        if ((InputMouseLeft || InputMouseRight) && _gun.OnReloading == false)
        {
            _targetRigWeight = 1;
            _handRig.weight = 1;
            _playerRotation = Quaternion.Euler(0, _transformMainCamera.eulerAngles.y, 0); // aiming rotation       
            _mainCamera.Aiming();
            _anim.SetBool(_isAimHash, true);
            currentSpeedRotation = _aimSpeedRotate;
            Debug.Log("2");
        }

        //direct player while its idle (work with first if)
        else if (_baseMovement.FunctionMove.magnitude >= .05f && _gun.OnReloading == false)
        {
            _playerRotation = Quaternion.LookRotation(_baseMovement.FunctionMove, Vector3.up);
            Debug.Log("3");
        }

        //360 rotation without direct player at camera
        if (_gun.OnReloading == true)
        {
            _handRig.weight = 0;
            _targetRigWeight = 0;
            _mainCamera.ExitAiming();
            if (_baseMovement.FunctionMove != Vector3.zero)
                _playerRotation = Quaternion.LookRotation(_baseMovement.FunctionMove, Vector3.up);
            currentSpeedRotation = _freeSpeedRotate;
            Debug.Log("4");
        }

        _bodyForRotate.transform.rotation = Quaternion.Lerp(_bodyForRotate.transform.rotation, _playerRotation, currentSpeedRotation * Time.deltaTime);

        //smooth spineRig
        if (_baseMovement.FunctionMove.magnitude >= .05f) _aimConstraintLift -= Time.deltaTime * _aimConstraintLiftSpeed;

        else _aimConstraintLift += Time.deltaTime * _aimConstraintLiftSpeed;

        if (_aimConstraintLift > 1) _aimConstraintLift = 1;

        if (_aimConstraintLift < 0) _aimConstraintLift = 0;

        _multiAimConstraint.data.offset = Vector3.Lerp(new Vector3(10, _multiAimConstraint.data.offset.y, 10), new Vector3(0, _multiAimConstraint.data.offset.y, 0), _aimConstraintLift);
        _spineRig.weight = Mathf.Lerp(_spineRig.weight, _targetRigWeight, Time.deltaTime * 10);
    }
}
