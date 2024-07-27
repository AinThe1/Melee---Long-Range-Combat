using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private BaseMovement _baseMovement;
    [SerializeField] private Animator _anim;
    [SerializeField] private ImpactDirection _impactDirection;
    [SerializeField] private MeleeRotation _meleeRotation;
    [SerializeField] private float _holdClickForAttackTime = 1;   

    private PlayerControl _inputSystemControl;
    private bool _animAttackIsPlaying;
    private int _onAttackHash;
    private float _startJumpForce;

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
    }

    private void AttackAnimation()
    {     
        var InputMouseLeft = _inputSystemControl.Player.Shoot.WasPressedThisFrame();

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
    }
    
    private void DisabledAttackAnim() => _anim.SetBool(_onAttackHash, false);

    private void StateAnimAttack()
    {
        if (_anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            _animAttackIsPlaying = true;
        else
            _animAttackIsPlaying = false;
    }  
}
