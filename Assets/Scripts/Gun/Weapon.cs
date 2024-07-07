using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _currentMagBullets;
    [SerializeField] private int _totalBullets;
    [SerializeField] private int _wasteOfAmmoPerShot; // for counter
    [SerializeField] private float _volumeForShoot;
    [SerializeField] private float _volumeForHitAtEnemy;
    [SerializeField] private float _volumeForHitAtObjects;
    [SerializeField] private float _timeForReload;
    [SerializeField] public float HoldClickForAttackTime = 1;
    [SerializeField] private float _shakeIntensity;
    [SerializeField] private float _timerShakeIntensity;
    [SerializeField] private bool _isFullAuto;

    [Header("SpreadOfBullets")]
    [SerializeField] private float _RandomSpread; // 0.05 the more value the more spread
    
    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSourceForShoot;
    [SerializeField] private AudioSource _soundReload;
    [SerializeField] private AudioSource _audioSourceForHitAtObjects;
    [SerializeField] private AudioSource _audioSourceForHit;
    [SerializeField] private AudioClip _clipHitAtWood;
    [SerializeField] private AudioClip _clipHitAtMetall;
    [SerializeField] private AudioClip _clipHitAtEnemy;
    [SerializeField] private AudioClip _clipForShoot;
    [Header("VFX")]
    [SerializeField] private ParticleSystem _vfxShoot;
    [SerializeField] private ParticleSystem _hitAtWood;
    [SerializeField] private ParticleSystem _hitAtMetall;
    [Header("Objects")]
    [SerializeField] private CameraShake _snakeAimCamera;
    [SerializeField] private Camera _camera;  
    [SerializeField] private Animator _anim;
    [SerializeField] private LayerMask _layerCollision;
    [SerializeField] private HitMarker _hitMarket;

    [HideInInspector] public int ScoreKills;
    [HideInInspector] public int MagCapacity;
    [HideInInspector] public bool OnReloading = false;   
    private PlayerControl _inputSystemControl;
    private float _bulletsShot;
    private float _startValueHoldClickForAttackTime;
    private float _reloadTimeLeft;
    private bool _canShoot = true;
    private bool _canReload = true;
    private int _onReloadHash;

    private void Update() => MyUpdate();
    private void Start() => MyStart();
    private void OnEnable() => _inputSystemControl.Enable();
    private void OnDisable() => _inputSystemControl.Disable();
    private void Awake() => _inputSystemControl = new PlayerControl();

    private void MyUpdate()
    {
        TimeForReloading(Time.deltaTime);
        FunctionBoolAutoReload();
        if (Input.GetKeyDown(KeyCode.R))
            Reloading();
        Shoot();  
    }

    private void MyStart()
    {       
        _onReloadHash = Animator.StringToHash("OnReload");
        MagCapacity = _currentMagBullets;
        _startValueHoldClickForAttackTime = HoldClickForAttackTime;
    }

    public void Shoot()
    {       
        if (_canShoot)
        {
            bool InputMouseLeft;
            if(_isFullAuto)
                InputMouseLeft = _inputSystemControl.Player.Shoot.IsPressed();
            else
                InputMouseLeft = _inputSystemControl.Player.Shoot.WasPressedThisFrame();

            if (InputMouseLeft && _currentMagBullets > 0 && Time.time >= HoldClickForAttackTime)
            {
                _vfxShoot.Play();             
                HoldClickForAttackTime = Time.time + _startValueHoldClickForAttackTime;            
                _audioSourceForShoot.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
                _audioSourceForShoot.PlayOneShot(_clipForShoot, _volumeForShoot);
                _snakeAimCamera.TryShakeCamera(_shakeIntensity, _timerShakeIntensity);
                _currentMagBullets -= _wasteOfAmmoPerShot;
                Hit();
            }
            else
                _hitMarket.ImageHitMarker.enabled = false;
        }
    }


    private void Hit()
    {
        RaycastHit hit;
        var x = UnityEngine.Random.Range(-_RandomSpread, _RandomSpread);
        var y = UnityEngine.Random.Range(-_RandomSpread, _RandomSpread);
       
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward + _camera.transform.right * x + _camera.transform.up * y,
        out hit, 100, _layerCollision))
        {          
            if (hit.collider.CompareTag("Untagged"))
            {
                Instantiate(_hitAtWood, hit.point, Quaternion.LookRotation(hit.normal));
                _audioSourceForHitAtObjects.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
                _audioSourceForHitAtObjects.PlayOneShot(_clipHitAtWood, _volumeForHitAtObjects);
            }

            else if (hit.collider.CompareTag("Metall"))
            {
                Instantiate(_hitAtMetall, hit.point, Quaternion.LookRotation(hit.normal));
                _audioSourceForHitAtObjects.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
                _audioSourceForHitAtObjects.PlayOneShot(_clipHitAtMetall, _volumeForHitAtObjects - 0.07f);
            }

            else if (hit.collider.CompareTag("Enemy"))
            {
                Instantiate(_hitAtWood, hit.point, Quaternion.LookRotation(hit.normal));
                _audioSourceForHitAtObjects.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
                _audioSourceForHitAtObjects.PlayOneShot(_clipHitAtEnemy, _volumeForHitAtEnemy - 0.07f);
                _hitMarket.ImageHitMarker.enabled = true;
            }
        }
        _bulletsShot--;
    }

    public void Reload()
    {
        var amount = Mathf.Min(MagCapacity - _currentMagBullets, _totalBullets);
        _totalBullets -= amount;
        _currentMagBullets += amount;
    }

    private void TimeForReloading(float dt)
    {
        //reloading
        if (_reloadTimeLeft > 0f)
        {
            OnReloading = true;
            _canShoot = false;
            _reloadTimeLeft -= dt;
            _anim.SetBool(_onReloadHash, true);
        }
        //reloaded
        if (_reloadTimeLeft < 0f)
        {
            Reload();
            _reloadTimeLeft = 0f;
            _canShoot = true;
            OnReloading = false;
            _anim.SetBool(_onReloadHash, false);
        }          

        if (_totalBullets <= 0)
            _totalBullets += 999;
    }

    public void Reloading()
    {
        if (_canReload && _currentMagBullets < MagCapacity && _totalBullets > 0 && !OnReloading)
        {
            _reloadTimeLeft = _timeForReload;
            _soundReload.Play();
        }
    }

    public void AutoReload()
    {
        if (_currentMagBullets <= 0 && _totalBullets > 0)
        {
            _reloadTimeLeft = _timeForReload;
            _canReload = false;
            _soundReload.Play();
        }
    }

    private void FunctionBoolAutoReload()
    {
        if (_canReload)
            AutoReload();

        if (_currentMagBullets > 0)
            _canReload = true;
    }
}
