using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHuman : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _speedWalk = 7;
    [SerializeField] private float _speedRun = 7;
    [SerializeField] private float _freeSpeedRotate = 7;
    [SerializeField] private float _aimSpeedRotate = 7;
    [SerializeField] private int _jumpForce = 1500;
    [SerializeField] private int _rigBodyYRotation = 50;
    [SerializeField] private float _smoothRig;
    [SerializeField] private float _aimConstraintLiftSpeed;


    [Header("CameraLookMovement")]
    [SerializeField] private Transform _transformMainCamera;
    [SerializeField] private TransitionBetweenAiming _mainCamera;
    [SerializeField] private GameObject _bodyForRotate;
    [SerializeField] private Weapon _gun;


    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private float _smoothBlend = 0.15f;
    [SerializeField] private Rig _spineRig;
    [SerializeField] private Rig _handRig;
    [SerializeField] private MultiAimConstraint _multiAimConstraint;


    // Buttons  
    private AimButton _aimButton;
    private JumpButton _jumpButton; 

    [Header("Jump")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _checkRadius = 0.1f;
    
    private PlayerControl _inputSystemControl;
    private InputController _inputController;
    private Rigidbody _rb;
    private Vector3 _functionMove;
    private Quaternion _playerRotation;
    private bool _onGround;
    private int _horizontalHash;
    private int _verticalHash;
    private int _isRunningHash;
    private int _magnitudeHash;
    private int _isAimHash;
    private int _onGroundHash;
    private int _targetRigWeight;
    private float _aimConstraintLift;

    private void Start() => MyStart();
    private void Update() => MyUpdate();
    private void FixedUpdate() => Movement();
    private void OnEnable() => _inputSystemControl.Enable();      
    private void OnDisable() => _inputSystemControl.Disable();   
    private void Awake() => _inputSystemControl = new PlayerControl();

 
    private void MyStart()
    {
        _inputController = FindObjectOfType<InputController>();
        _jumpButton = FindObjectOfType<JumpButton>();
        _aimButton = FindObjectOfType<AimButton>();
        _rb = GetComponent<Rigidbody>();
        _horizontalHash = Animator.StringToHash("Horizontal");
        _verticalHash = Animator.StringToHash("Vertical");
        _isRunningHash = Animator.StringToHash("IsRunning");
        _magnitudeHash = Animator.StringToHash("Magnitude");
        _isAimHash = Animator.StringToHash("IsAim");
        _onGroundHash = Animator.StringToHash("OnGround");
        if (_aimButton != null)          
            _aimButton.MyStart();
       
    }

    private void MyUpdate()
    {
        Rotation();       
        ChekingGround();
    }

    private void Movement()
    {
        var input = _inputSystemControl.Player.Movement.ReadValue<Vector2>();
        _functionMove = new Vector3(input.x, 0, input.y);

        //movement with the camera
        var CameraForwardNormalized = new Vector3(_transformMainCamera.transform.forward.x, 0, _transformMainCamera.transform.forward.z).normalized;
        var CameraRightNormalized = new Vector3(_transformMainCamera.transform.right.x, 0, _transformMainCamera.transform.right.z).normalized;
        _functionMove = _functionMove.x * CameraRightNormalized + _functionMove.z * CameraForwardNormalized;
        _functionMove.y = 0f; 
        _rb.velocity = new Vector3(_functionMove.x * _speedWalk , _rb.velocity.y, _functionMove.z * _speedWalk);
        
        //animations
        if (_functionMove.magnitude >= 0.05f)
            _anim.SetBool(_isRunningHash, true);
        else
            _anim.SetBool(_isRunningHash, false);      

        //motion switch
        if (Input.GetKey(KeyCode.LeftShift))
            Run();
        else
            _anim.SetFloat(_magnitudeHash, _functionMove.magnitude, _smoothBlend, Time.deltaTime);
    }

    private void Run()
    {
        _rb.velocity = new Vector3(_functionMove.x * _speedRun, _rb.velocity.y, _functionMove.z * _speedRun);
        _anim.SetFloat(_magnitudeHash, _functionMove.magnitude * 2f , _smoothBlend, Time.deltaTime);
    }

    private void Rotation()
    {
        var InputMouseLeft = _inputSystemControl.Player.Shoot.IsPressed();
        var InputMouseRight = _inputSystemControl.Player.Aim.IsPressed();
        float currentSpeedRotation = 0;
        
        //360 rotation without direct player at camera
        if ((!InputMouseLeft && !InputMouseRight  /*&& _aimButton.OnAim == false*/) && _gun.OnReloading == false)
        {
            _targetRigWeight = 0;
            _handRig.weight = 1;
            _mainCamera.ExitAiming();
            _anim.SetBool(_isAimHash, false);
            currentSpeedRotation = _freeSpeedRotate;
            Debug.Log("1");
        }

        //360 rotation with direct player at camera
        if ((InputMouseLeft || InputMouseRight /*|| _aimButton.OnAim == true*/) && _gun.OnReloading == false)
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
        else if (_functionMove.magnitude >= 0.05f && _gun.OnReloading == false)
        {
            _playerRotation = Quaternion.LookRotation(_functionMove, Vector3.up);         
            Debug.Log("3");
        }
        
        //360 rotation without direct player at camera
        if (_gun.OnReloading == true)
        {
            _handRig.weight = 0;
            _targetRigWeight = 0;
            _mainCamera.ExitAiming();
            if (_functionMove != Vector3.zero)
                _playerRotation = Quaternion.LookRotation(_functionMove, Vector3.up);       
            currentSpeedRotation = _freeSpeedRotate;
            Debug.Log("4");
        }

        _bodyForRotate.transform.rotation = Quaternion.Lerp(_bodyForRotate.transform.rotation, _playerRotation, currentSpeedRotation * Time.deltaTime);

        //smooth spineRig
        if (_functionMove.magnitude >= 0.05f) _aimConstraintLift -= Time.deltaTime * _aimConstraintLiftSpeed;

        else _aimConstraintLift += Time.deltaTime * _aimConstraintLiftSpeed;

        if (_aimConstraintLift > 1) _aimConstraintLift = 1;

        if (_aimConstraintLift < 0) _aimConstraintLift = 0;

        _multiAimConstraint.data.offset = Vector3.Lerp(new Vector3(10, _multiAimConstraint.data.offset.y, 10), new Vector3(0, _multiAimConstraint.data.offset.y, 0), _aimConstraintLift);
        _spineRig.weight = Mathf.Lerp(_spineRig.weight, _targetRigWeight, Time.deltaTime * 10); 
    }

    private void ChekingGround()
    {
        _onGround = Physics.CheckSphere(_groundCheck.position, _checkRadius, _ground);
        _anim.SetBool(_onGroundHash, _onGround);
        Jump();
    }

    private void Jump()
    {
        var ButtonJump = _inputSystemControl.Player.Jump.IsPressed();
        if ((ButtonJump /*|| _jumpButton.IsJump*/) && _onGround)
        _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime);
    }
}
