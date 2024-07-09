using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private BaseMovement _baseMovement;
    [SerializeField] private Animator _anim;
    [SerializeField] private CheckerForAttack _checkerForAttack;
    [SerializeField] private ForceOfAttraction _forceOfAttraction;
    [SerializeField] private float _holdClickForAttackTime = 1;

    private PlayerControl _inputSystemControl;
    private Vector3 _directionAttack;
    private bool _animAttackIsPlaying;
    private float _startValueHoldClickForAttackTime;
    private int _onAttackHash;
    private int _currentSpeed;
    private int _stateAttack2Hash;
    private int _stateAttack3Hash;

    public CheckerForAttack CheckerForAttack { get { return _checkerForAttack; } private set { } }
    public Vector3 DirectionAttack { get { return _directionAttack; } private set {} }
    public bool AnimAttackIsPlaying { get { return _animAttackIsPlaying;} private set {} }

    private void OnEnable() => _inputSystemControl.Enable();
    private void OnDisable() => _inputSystemControl.Disable();
    private void Awake() => _inputSystemControl = new PlayerControl();
    private void Start() => MyStart();
    private void Update() => MyUpdate();

    private void MyUpdate()
    {
        StateAnimAttack();
        AttackAnimation();
        _currentSpeed = _baseMovement.SpeedMove;
    }

    private void MyStart()
    {
        
        _startValueHoldClickForAttackTime = _holdClickForAttackTime;
        _onAttackHash = Animator.StringToHash("OnAttack");
        _stateAttack2Hash = Animator.StringToHash("State2Attack");
        _stateAttack3Hash = Animator.StringToHash("State3Attack");
    }

    private void AttackAnimation()
    {
        // damaging enemy at Events Animations (in the folder "events")
        if (_checkerForAttack == null) return;
        var InputMouseLeft = _inputSystemControl.Player.Shoot.IsPressed();
        //directonAtEnemy
        if (_checkerForAttack.AtZoneForAttack && InputMouseLeft)
        {
            _directionAttack = _checkerForAttack.Target.transform.position;
        }
    
        if (_checkerForAttack.Target != null)
            _forceOfAttraction.Direction = _directionAttack;
    
        //Animations
        if (InputMouseLeft)
            _anim.SetBool(_onAttackHash, true);
        else
        {
            if (Time.time >= _holdClickForAttackTime)
            {
                _anim.SetBool(_onAttackHash, false);
                _holdClickForAttackTime = Time.time + _startValueHoldClickForAttackTime;
            }
        }
    
        //if (_animAttackIsPlaying)
        //    _baseMovement.SpeedMove = 0;
        //else
        //    _baseMovement.SpeedMove = _currentSpeed;
    
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
            _animAttackIsPlaying = true;
        else
            _animAttackIsPlaying = false;
    }
}
