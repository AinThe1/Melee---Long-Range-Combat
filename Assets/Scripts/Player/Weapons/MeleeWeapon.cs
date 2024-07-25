using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private BaseMovement _baseMovement;
    [SerializeField] private Animator _anim;
    [SerializeField] private CheckerForAttack _checkerForAttack;
    [SerializeField] private ImpactDirection _impactDirection;
    [SerializeField] private MeleeRotation _meleeRotation;
    [SerializeField] private float _holdClickForAttackTime = 1;   

    private PlayerControl _inputSystemControl;
    private Vector3 _directionAttack;
    private bool _animAttackIsPlaying;
    private int _onAttackHash;
    private int _stateAttack2Hash;
    private int _stateAttack3Hash;
    private float _startJumpForce;

    public Vector3 DirectionAttack { get { return _directionAttack;}}
    public bool AnimAttackIsPlaying { get { return _animAttackIsPlaying;}}

    private void OnEnable() => _inputSystemControl.Enable();
    private void OnDisable() => _inputSystemControl.Disable();
    private void Awake() => _inputSystemControl = new PlayerControl();
    private void Start() => MyStart();
    private void Update() => MyUpdate();

    private void MyUpdate()
    {   
        StateAnimAttack();
        AttackAnimation();
    }

    private void MyStart()
    {
        _startJumpForce = _baseMovement.JumpForce;
        _onAttackHash = Animator.StringToHash("OnAttack");
        _stateAttack2Hash = Animator.StringToHash("StateAttack2");
        _stateAttack3Hash = Animator.StringToHash("StateAttack3");
    }

    private void AttackAnimation()
    {     
        if (_checkerForAttack == null) return;
        var InputMouseLeft = _inputSystemControl.Player.Shoot.WasPressedThisFrame();
        //directonAtEnemy
        if (_checkerForAttack.AtZoneForAttack && InputMouseLeft)
            _directionAttack = _checkerForAttack.Target.transform.position;

        //Animations
        if (InputMouseLeft)
        {          
            _anim.SetBool(_onAttackHash, true);
            CancelInvoke();
        }        
        else       
            Invoke("DisabledAttackAnim", _holdClickForAttackTime);

    
        if (_animAttackIsPlaying)
        {
            _baseMovement.SpeedMove = 0;
            _baseMovement.JumpForce = 0;
        }
        else
            _baseMovement.JumpForce = _startJumpForce;

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || _anim.GetCurrentAnimatorStateInfo(0).IsName("SlideSlash"))
            _anim.SetBool(_stateAttack2Hash, true);
        else
            _anim.SetBool(_stateAttack2Hash, false);
    
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            _anim.SetBool(_stateAttack3Hash, true);
        else
            _anim.SetBool(_stateAttack3Hash, false);
    }
    
    private void DisabledAttackAnim() => _anim.SetBool(_onAttackHash, false);

    private void StateAnimAttack()
    {
        if ((_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || _anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) || _anim.GetCurrentAnimatorStateInfo(0).IsName("SlideSlash") 
           || _anim.GetCurrentAnimatorStateInfo(0).IsName("2Kslash"))
            _animAttackIsPlaying = true;
        else
            _animAttackIsPlaying = false;
    }  
}
