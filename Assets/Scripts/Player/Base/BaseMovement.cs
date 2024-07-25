using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [Header("Stats")]   
    [SerializeField] private float _speedWalk = 2;
    [SerializeField] private float _speedRun = 7;
    [SerializeField] private float _jumpForce = 1.3f;
    [SerializeField] private float _gravityMultiplier = 0.3f;
    [SerializeField] private float _jumpRespite = 0.5f;
    [Header("CameraLookMovement")]
    [SerializeField] private Transform _transformMainCamera;

    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private float _smoothBlend = 0.15f;

    [Header("Jump")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _checkRadius = 0.1f;

    private PlayerControl _inputSystemControl;
    private CharacterController _characterController;
    private float _speedMove = 2;
    private int _magnitudeHash;
    private int _onGroundHash;
    private Vector3 _playerDirection;
    private Vector3 _gravityVelocity;
    private float _velocity;
    private float _gravity = -9.81f;
    private bool _onGround;
    private bool _canJump = true;
    
    public bool OnGround { get { return _onGround; } }
    public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
    public Vector3 PlayerDirection { get { return _playerDirection; } set { _playerDirection = value; } }
    public float SpeedMove { get { return _speedMove; }  set { _speedMove = value; } }

    private void Start() => MyStart();
    private void Update() => Movement();
    private void OnEnable() => _inputSystemControl.Enable();      
    private void OnDisable() => _inputSystemControl.Disable();   
    private void Awake() => _inputSystemControl = new PlayerControl();

    private void MyStart()
    {
        _characterController = GetComponent<CharacterController>();
        _magnitudeHash = Animator.StringToHash("Magnitude");
        _onGroundHash = Animator.StringToHash("OnGround");      
    }

    private void Movement()
    {
        //input
        var input = _inputSystemControl.Player.Movement.ReadValue<Vector2>();
        _playerDirection = new Vector3(input.x, 0, input.y);

        //camera directs the player
        var CameraForwardNormalized = new Vector3(_transformMainCamera.transform.forward.x,0, _transformMainCamera.transform.forward.z).normalized;
        var CameraRightNormalized = new Vector3(_transformMainCamera.transform.right.x, 0, _transformMainCamera.transform.right.z).normalized;
        _playerDirection = _playerDirection.x * CameraRightNormalized + _playerDirection.z * CameraForwardNormalized;

        Jump();
        Gravity();
        WalkAndRun();
        ChekingGround();
    }
   
    private void ChekingGround()
    {
        _onGround = Physics.CheckSphere(_groundCheck.position, _checkRadius, _ground);
        _anim.SetBool(_onGroundHash, _onGround);    
    }

    private void Jump()
    {
        var ButtonJump = _inputSystemControl.Player.Jump.WasPressedThisFrame();
        if (ButtonJump && _characterController.isGrounded && _canJump)
        {
            _velocity += _jumpForce + 1;
            _canJump = false;
            Invoke("CanJump", _jumpRespite);
        }
    }
    
    private void CanJump() => _canJump = true;
    
    private void Gravity()
    {
        if (_onGround && _velocity < 0.0f)
            _velocity = -1.0f;
        else
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;

        _gravityVelocity.y = _velocity * Time.deltaTime * 3;
    }

    public void WalkAndRun()
    {
        var normalizedDirection = new Vector3(_playerDirection.x * _speedMove * Time.deltaTime, _gravityVelocity.y, _playerDirection.z * _speedMove * Time.deltaTime);
        _characterController.Move(normalizedDirection);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speedMove = _speedRun;
            _anim.SetFloat(_magnitudeHash, _playerDirection.magnitude * 2f, _smoothBlend, Time.deltaTime);
        }

        else
        {
            _speedMove = _speedWalk;
            _anim.SetFloat(_magnitudeHash, _playerDirection.magnitude, _smoothBlend, Time.deltaTime);
        }    
    }
}