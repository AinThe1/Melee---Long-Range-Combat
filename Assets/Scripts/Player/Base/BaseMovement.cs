using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [Header("Stats")]   
    [SerializeField] private int _speedWalk = 2;
    [SerializeField] private int _speedRun = 7;
    [SerializeField] private int _jumpForce = 1500;

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
    private Rigidbody _rb;
    private bool _onGround;
    private int _speedMove = 2;
    private int _isRunningHash;
    private int _magnitudeHash;
    private int _onGroundHash;
    private Vector3 _functionMove;

    public int JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
    public bool OnGround { get { return _onGround; } }
    public Vector3 FunctionMove { get { return _functionMove; } }
    public int SpeedMove { get { return _speedMove; }  set { _speedMove = value; } }

    private void Start() => MyStart();
    private void Update() => ChekingGround();
    private void FixedUpdate() => Movement();
    private void OnEnable() => _inputSystemControl.Enable();      
    private void OnDisable() => _inputSystemControl.Disable();   
    private void Awake() => _inputSystemControl = new PlayerControl();

    private void MyStart()
    {
        _rb = GetComponent<Rigidbody>();
        _isRunningHash = Animator.StringToHash("IsRunning");
        _magnitudeHash = Animator.StringToHash("Magnitude");
        _onGroundHash = Animator.StringToHash("OnGround");      
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
        _rb.velocity = new Vector3(_functionMove.x * _speedMove, _rb.velocity.y, _functionMove.z * _speedMove);

        //Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speedMove = _speedRun;
            _anim.SetFloat(_magnitudeHash, _functionMove.magnitude * 2f, _smoothBlend, Time.deltaTime);
            _anim.SetBool(_isRunningHash, true);
        }
        else
        {
            _speedMove = _speedWalk;
            _anim.SetFloat(_magnitudeHash, _functionMove.magnitude, _smoothBlend, Time.deltaTime);            
            _anim.SetBool(_isRunningHash, false);
        }
            
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
        if (ButtonJump && _onGround)
        _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime);
    }
}
