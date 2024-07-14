using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LongRangeRotation : MonoBehaviour
{  
    [Header("Stats")]
    [SerializeField] private float _freeSpeedRotate = 10;
    [SerializeField] private float _aimSpeedRotate = 20;
    [SerializeField] private float _smoothSpineRigSpeed = 3;
    [SerializeField] private float _directionLiftSpeed = 25;

    [Header("CameraLookMovement")]
    [SerializeField] private Transform _transformMainCamera;
    [SerializeField] private TransitionBetweenAiming _mainCamera;
    [SerializeField] private GameObject _bodyForRotate;
    [SerializeField] private RangedWeapon _gun;

    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Rig _spineRig;
    [SerializeField] private Rig _leftArmRig;
    [SerializeField] private MultiAimConstraint _multiAimConstraint;

    private int _targetRigWeight;
    private int _isAimHash;
    private int _isHorizontalHash;
    private int _isVerticalHash;
    private Quaternion _playerRotation;
    private PlayerControl _inputSystemControl;
    private float _aimConstraintLift;
    private BaseMovement _baseMovement;
    private Vector2 _currentInputVector;
    private Vector2 _smoothVectorVelocity;

    private void OnEnable() => _inputSystemControl.Enable();
    private void OnDisable() => _inputSystemControl.Disable();
    private void Awake() => _inputSystemControl = new PlayerControl();
    private void Start() => MyStart();
    private void Update() => Rotation();

    private void MyStart()
    {
        _baseMovement = GetComponent<BaseMovement>();
        _isAimHash = Animator.StringToHash("IsAim");
        _isHorizontalHash = Animator.StringToHash("Horizontal");
        _isVerticalHash = Animator.StringToHash("Vertical");
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
            _leftArmRig.weight = 1;
            _mainCamera.ExitAiming();
            _anim.SetBool(_isAimHash, false);
            currentSpeedRotation = _freeSpeedRotate;
            Debug.Log("1");
        }

        //360 rotation with direct player at camera
        if ((InputMouseLeft || InputMouseRight) && _gun.OnReloading == false)
        {
            _targetRigWeight = 1;
            _leftArmRig.weight = 1;
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
            _leftArmRig.weight = 0;
            _targetRigWeight = 0;
            _mainCamera.ExitAiming();
            if (_baseMovement.FunctionMove != Vector3.zero)
                _playerRotation = Quaternion.LookRotation(_baseMovement.FunctionMove, Vector3.up);
            currentSpeedRotation = _freeSpeedRotate;
            Debug.Log("4");
        }

        SmoothAimMovement();
        SmoothSpineRig();      
        _spineRig.weight = Mathf.Lerp(_spineRig.weight, _targetRigWeight, Time.deltaTime * 10);//turn and off spineRig
        _bodyForRotate.transform.rotation = Quaternion.Lerp(_bodyForRotate.transform.rotation, _playerRotation, currentSpeedRotation * Time.deltaTime);
    }

    private void SmoothSpineRig()
    {
        var GunDown = new Vector3(10, 50, 20);
        var GunUp = new Vector3(0, 42, 0);

        if (_baseMovement.FunctionMove.magnitude >= .05f && !Input.GetKey(KeyCode.S)) 
            _aimConstraintLift -= Time.deltaTime * _smoothSpineRigSpeed;
        else 
            _aimConstraintLift += Time.deltaTime * _smoothSpineRigSpeed;

        if (_aimConstraintLift > 1) _aimConstraintLift = 1;
        if (_aimConstraintLift < 0) _aimConstraintLift = 0;
        
        _multiAimConstraint.data.offset = Vector3.Lerp(GunDown, GunUp, _aimConstraintLift);
    }

    private void SmoothAimMovement()
    {
        var input = _inputSystemControl.Player.Movement.ReadValue<Vector2>();
        _currentInputVector = Vector2.SmoothDamp(_currentInputVector, input, ref _smoothVectorVelocity, _directionLiftSpeed * Time.deltaTime);
        _anim.SetFloat(_isHorizontalHash, _currentInputVector.x);
        _anim.SetFloat(_isVerticalHash, _currentInputVector.y);
    }
}
