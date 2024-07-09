using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _speedWalk = 2;
    [SerializeField] private float _speedRun = 7;
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
    private int _isRunningHash;
    private int _magnitudeHash;
    private int _onGroundHash;
    private Vector3 _functionMove;
    public Vector3 FunctionMove { get { return _functionMove; } private set{} }

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
        _rb.velocity = new Vector3(_functionMove.x * _speedWalk , _rb.velocity.y, _functionMove.z * _speedWalk);
        
        //animations
        if (_functionMove.magnitude >= .05f)
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
