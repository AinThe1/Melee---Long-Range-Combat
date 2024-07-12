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
    private int _onAttackHash;
    private int _stateAttack2Hash;
    private int _stateAttack3Hash;

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
        _onAttackHash = Animator.StringToHash("OnAttack");
        _stateAttack2Hash = Animator.StringToHash("StateAttack2");
        _stateAttack3Hash = Animator.StringToHash("StateAttack3");
    }

    private void AttackAnimation()
    {
        // damaging enemy at Events Animations (in the folder "events")
        if (_checkerForAttack == null) return;
        var InputMouseLeft = _inputSystemControl.Player.Shoot.WasPressedThisFrame();
        //directonAtEnemy
        if (_checkerForAttack.AtZoneForAttack && InputMouseLeft)
            _directionAttack = _checkerForAttack.Target.transform.position;
    
        if (_checkerForAttack.Target != null)
            _forceOfAttraction.Direction = _directionAttack;
    
        //var timer += Time.deltaTime
        //Animations
        if (InputMouseLeft)
        {
            _anim.SetBool(_onAttackHash, true);
            CancelInvoke();
        }        
        else       
            Invoke("DisabledAttackAnim", _holdClickForAttackTime);

    
        if (_animAttackIsPlaying)
            _baseMovement.SpeedMove = 0;
    
    
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
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
