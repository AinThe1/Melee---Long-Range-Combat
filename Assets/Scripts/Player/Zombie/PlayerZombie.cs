using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class PlayerZombie : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float SpeedMove = 8;
    [SerializeField] private int _speedRotate = 7;
    [SerializeField] private int _bodyRotate = 10;
    [SerializeField] private int _jumpForce = 1500;
   
    [Header("CameraLookMovement")]
    [SerializeField] private Transform _transformMainCamera;   
    [SerializeField] private TransitionBetweenAiming _mainCamera;
    private Joystick _joystick;

    private PlayerControl _inputSystemControl;
    private Quaternion _playerRotation;
    private Rigidbody _rb;
    private Vector3 _functionMove;
    private float _startSpeed;

    [Header("Animations")]
    [SerializeField] private Animator _anim;
    [SerializeField] private float _smoothBlend = 0.15f;
    [SerializeField] private float _holdClickForAttackTime = 1;
    private float _startValueHoldClickForAttackTime;

    //buttons
    private JumpButton _jumpButton;
    private AttackButton _attackButton;

    [Header("Jump")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _checkRadiusForGround = 0.1f;
    
    [Header("Attack")]
    [SerializeField] private AttackCheckerFromZombie _attackCheckerForHuman;
    [SerializeField] private float _checkRadiusForAttack = 2f;
    [SerializeField] private ForceOfAttraction _forceOfAttraction;

    private InputController _inputController;
    private Vector3 _directionAttack;
    private bool _onGround;
    private bool _animAttackPlaying;
    private int _magnitudeHash;
    private int _onGroundHash;
    private int _onAttackHash;
    private int _stateAttack2Hash;
    private int _stateAttack3Hash;

    private void Update() => MyUpdate();
    private void Start() => MyStart();
    private void FixedUpdate() => MyFixedUpdate();
    private void OnEnable() => _inputSystemControl.Enable();      
    private void OnDisable() => _inputSystemControl.Disable();   
    private void Awake() => _inputSystemControl = new PlayerControl();
  
    private void MyFixedUpdate()
    {
        Movement();
    }
    private void MyUpdate()
    {
        Jump();
        ChekingGround();
        StateAnimAttack();
        AttackAnimation();
    }

    private void MyStart()
    {
        _rb = GetComponent<Rigidbody>();
        _startSpeed = SpeedMove;
        _startValueHoldClickForAttackTime = _holdClickForAttackTime;
        _magnitudeHash = Animator.StringToHash("Magnitude");
        _onGroundHash = Animator.StringToHash("OnGround");
        _onAttackHash = Animator.StringToHash("OnAttack");
        _stateAttack2Hash = Animator.StringToHash("State2Attack");
        _stateAttack3Hash = Animator.StringToHash("State3Attack");
        _inputController = FindObjectOfType<InputController>();
        if (_inputController.InputIsPhone)
            _joystick = FindObjectOfType<Joystick>();
        _jumpButton = FindObjectOfType<JumpButton>();
        _attackButton = FindObjectOfType<AttackButton>();
    }

    private void Movement()
    {
        var input = _inputSystemControl.Player.Movement.ReadValue<Vector2>();
        //joystick
        if (_joystick != null)
            _functionMove = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        //keyboard
        if (_joystick == null)
            _functionMove = new Vector3(input.x,0,input.y);
        //movement from camera
        var CameraForwardNormalized = new Vector3(_transformMainCamera.forward.x, 0, _transformMainCamera.forward.z).normalized;
        var CameraRightNormalized = new Vector3(_transformMainCamera.right.x, 0, _transformMainCamera.right.z).normalized;
        _functionMove = _functionMove.x * CameraRightNormalized + _functionMove.z * CameraForwardNormalized;
        _functionMove.y = 0f;
        _rb.velocity = new Vector3(_functionMove.x * SpeedMove , _rb.velocity.y, _functionMove.z * SpeedMove);              
        //Animations
        if(Input.GetKey(KeyCode.LeftShift) && _onGround)
            Walk();
        else
            _anim.SetFloat(_magnitudeHash, _functionMove.magnitude, _smoothBlend, Time.deltaTime);

        Rotation();//////////////////
    }

    private void Walk()
    {
        _rb.velocity = new Vector3(_functionMove.x * SpeedMove / 3f, _rb.velocity.y, _functionMove.z * SpeedMove / 3f);
        _anim.SetFloat(_magnitudeHash, _functionMove.magnitude / 3f , _smoothBlend, Time.deltaTime);
    }

    private void Rotation()
    {
        if(_animAttackPlaying == false && _functionMove.magnitude >= 0.05f)
            _playerRotation = Quaternion.LookRotation(_functionMove, Vector3.up);
        
        if(_animAttackPlaying == true && _directionAttack != Vector3.zero && _attackCheckerForHuman.Target != null)
        {
            Vector3 direction = (_attackCheckerForHuman.Target.transform.position - transform.position).normalized;
            _playerRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }                        

        _rb.rotation = Quaternion.Lerp(_rb.rotation, _playerRotation, _speedRotate * Time.fixedDeltaTime);
    }

    private void ChekingGround()
    {
        _onGround = Physics.CheckSphere(_groundCheck.position, _checkRadiusForGround, _ground);
        _anim.SetBool(_onGroundHash, _onGround);
    }

    private void Jump()
    {
        var ButtonJump = _inputSystemControl.Player.Jump.IsPressed();
        if ((ButtonJump /*|| _jumpButton.IsJump*/) && _onGround)
        _rb.AddForce(Vector3.up * _jumpForce * Time.deltaTime);
    }

    private void AttackAnimation()
    {
        // damaging enemy at Events Animations (in the folder "events")
        if (_attackCheckerForHuman == null) return;
        var InputMouseLeft = _inputSystemControl.Player.Shoot.IsPressed();
        //directonAtEnemy
        if (_attackCheckerForHuman.AtZoneForAttack && (InputMouseLeft || _attackButton.IsAttack))
        {
            _directionAttack = _attackCheckerForHuman.Target.transform.position;
        }
             
        if (_attackCheckerForHuman.Target != null)
            _forceOfAttraction.Direction = _directionAttack;

        //Animations
        if ((InputMouseLeft /*|| _attackButton.IsAttack*/))
            _anim.SetBool(_onAttackHash, true);
        else
        {
            if(Time.time >= _holdClickForAttackTime)
            {
                _anim.SetBool(_onAttackHash, false);
                _holdClickForAttackTime = Time.time + _startValueHoldClickForAttackTime;
            }
        }            

        if (_animAttackPlaying)
            SpeedMove = 0;         
        else
            SpeedMove = _startSpeed;

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1")) 
            _anim.SetBool(_stateAttack2Hash, true);
        else
            _anim.SetBool(_stateAttack2Hash, false);
        
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            _anim.SetBool(_stateAttack3Hash, true);        
        else
            _anim.SetBool(_stateAttack3Hash, false);
    }

    private void StateAnimAttack()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
            _animAttackPlaying = true;
        else
            _animAttackPlaying = false;
    }
}
