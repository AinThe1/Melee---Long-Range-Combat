using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [Header("Stats")]   
    [SerializeField] private float _speedWalk = 2;
    [SerializeField] private float _speedRun = 7;
    [SerializeField] private float _jumpForce = 0.5f;
    [SerializeField] private float _gravityMultiplier = 0.1f;
    [SerializeField] private float _jumpRespite = 1;
    [Header("CameraLookMovement")]
    [SerializeField] private Transform _transformMainCamera;

    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private float _smoothBlend = .15f;

    [Header("Jump")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _checkRadius = .1f;

    private PlayerControl _inputSystemControl;
    private CharacterController _characterController;
    private float _speedMove = 2;
    private int _magnitudeHash;
    private int _onGroundHash;
    private Vector3 _functionMove;
    private Vector3 _gravityVelocity;
    private float _velocity;
    private float _gravity = -9.81f;
    private bool _onGround;
    private bool _canJump = true;

    public bool OnGround { get { return _onGround; } }
    public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
    public Vector3 FunctionMove { get { return _functionMove; } }
    public float SpeedMove { get { return _speedMove; }  set { _speedMove = value; } }

    private void Start() => MyStart();
    private void Update() => MyUpdate();
    private void OnEnable() => _inputSystemControl.Enable();      
    private void OnDisable() => _inputSystemControl.Disable();   
    private void Awake() => _inputSystemControl = new PlayerControl();

    private void MyUpdate()
    {
        ChekingGround();
        Movement();
    }

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
        _functionMove = new Vector3(input.x, 0, input.y);

        //movement with the camera
        var CameraForwardNormalized = new Vector3(_transformMainCamera.transform.forward.x,0, _transformMainCamera.transform.forward.z).normalized;
        var CameraRightNormalized = new Vector3(_transformMainCamera.transform.right.x, 0, _transformMainCamera.transform.right.z).normalized;
        _functionMove = _functionMove.x * CameraRightNormalized + _functionMove.z * CameraForwardNormalized;

        //movement
        Jump();
        Gravity();
        var normalizedDirection = new Vector3(_functionMove.x * _speedMove * Time.deltaTime, _gravityVelocity.y, _functionMove.z * _speedMove * Time.deltaTime);
        _characterController.Move(normalizedDirection);
       
        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speedMove = _speedRun;
            _anim.SetFloat(_magnitudeHash, _functionMove.magnitude * 2f, _smoothBlend, Time.deltaTime);
        }

        else
        {
            _speedMove = _speedWalk;
            _anim.SetFloat(_magnitudeHash, _functionMove.magnitude, _smoothBlend, Time.deltaTime);            
        }
    }
   

    private void ChekingGround()
    {
        _onGround = Physics.CheckSphere(_groundCheck.position, _checkRadius, _ground);
        _anim.SetBool(_onGroundHash, _onGround);    
    }

    private void Jump()
    {
        var ButtonJump = _inputSystemControl.Player.Jump.WasPressedThisFrame();
        if (ButtonJump && _onGround && _canJump)
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

        _gravityVelocity.y = _velocity * Time.deltaTime * 7;
   }
}
