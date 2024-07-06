using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int _currentMagBullets;
    [SerializeField] private int _totalBullets;
    [SerializeField] private int _wasteOfAmmoPerShot; // for counter
    [SerializeField] private int _bulletsPerTap;
    [SerializeField] private float _volumeForShoot;
    [SerializeField] private float _volumeForHitAtEnemy;
    [SerializeField] private float _volumeForHitAtObjects;
    [SerializeField] private float _timeForReload;
    [SerializeField] public float HoldClickForAttackTime = 1;
    [SerializeField] private float _shakeIntensity;
    [SerializeField] private float _timerShakeIntensity;
    [SerializeField] private int _damage;
    [SerializeField] private bool _isFullAuto;

    [Header("SpreadOfBullets")]
    [SerializeField] private float _RandomSpread; // 0.05 the more value the more spread
    [Header("Objects")]      
    [SerializeField] private AudioSource _audioSourceForShoot;
    [SerializeField] private AudioSource _soundReload;
    [SerializeField] private AudioSource _audioSourceForHitAtObjects;
    [SerializeField] private AudioSource _audioSourceForHit;
    [SerializeField] private AudioClip _clipHitAtGround;
    [SerializeField] private AudioClip _clipForShoot;
    [SerializeField] private ParticleSystem _vfxShoot;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraShake _snakeAimCamera;
    [SerializeField] private ParticleSystem _hitAtGround;
    [SerializeField] private ParticleSystem _hitAtEnemy;
    [SerializeField] private Animator _anim;
    [SerializeField] private LayerMask _ignoreLayerForShooting;  

    [HideInInspector] public int ScoreKills;
    [HideInInspector] public int MagCapacity;
    [HideInInspector] public bool OnReloading = false;
    private HitMarker _hitMarket;
    private CounterBulletsInMag _BulletsInMagazin;
    private ReloadButton _reloadButton;
    private ShotingButton _shootingButton;
    private PlayerControl _inputSystemControl;
    private float _bulletsShot;
    private float _startValueHoldClickForAttackTime;
    private float _reloadTimeLeft;
    private bool _cantShoot = false;
    private bool _canReload = true;
    private int _onReloadHash;

    private void Update() => MyUpdate();
    private void Start() => MyStart();
    private void OnEnable() => _inputSystemControl.Enable();
    private void OnDisable() => _inputSystemControl.Disable();
    private void Awake() => _inputSystemControl = new PlayerControl();

    private void MyUpdate()
    {
        if (_BulletsInMagazin != null)
            _BulletsInMagazin.BulletsInMag.text = _currentMagBullets + "/" + _totalBullets;
        TimeForReloading(Time.deltaTime);
        FunctionBoolAutoReload();
        if (Input.GetKeyDown(KeyCode.R))
            ButtonReload();
        Shoot();  
    }

    private void MyStart()
    {       
        _onReloadHash = Animator.StringToHash("OnReload");
        _shootingButton = FindObjectOfType<ShotingButton>();
        _reloadButton = FindObjectOfType<ReloadButton>();
        _BulletsInMagazin = FindObjectOfType<CounterBulletsInMag>();
        _hitMarket = FindObjectOfType<HitMarker>();
        MagCapacity = _currentMagBullets;
        _startValueHoldClickForAttackTime = HoldClickForAttackTime;       
        if (_reloadButton != null)
            _reloadButton.MyStart();
    }

    public void Shoot()
    {       
        if (!_cantShoot)
        {
            bool InputMouseLeft;
            if(_isFullAuto)
                InputMouseLeft = _inputSystemControl.Player.Shoot.IsPressed();
            else
                InputMouseLeft = _inputSystemControl.Player.Shoot.WasPressedThisFrame();

            if ((InputMouseLeft /*|| _shootingButton.Shoot*/) && _currentMagBullets > 0 && Time.time >= HoldClickForAttackTime)
            {
                _vfxShoot.Play();             
                HoldClickForAttackTime = Time.time + _startValueHoldClickForAttackTime;            
                _audioSourceForShoot.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
                _audioSourceForShoot.PlayOneShot(_clipForShoot, _volumeForShoot);
                _snakeAimCamera.TryShakeCamera(_shakeIntensity, _timerShakeIntensity);
                _currentMagBullets -= _wasteOfAmmoPerShot;
                _bulletsShot = _bulletsPerTap;
                ShootingByBullets();
            }
            else
                _hitMarket.ImageHitMarker.enabled = false;
        }
    }


    private void ShootingByBullets()
    {
        RaycastHit hit;
        var x = UnityEngine.Random.Range(-_RandomSpread, _RandomSpread);
        var y = UnityEngine.Random.Range(-_RandomSpread, _RandomSpread);
       
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward + _camera.transform.right * x + _camera.transform.up * y,
                            out hit, 100, _ignoreLayerForShooting))
        {          
            // hit at something untagged
            if (hit.collider.CompareTag("Untagged"))
            {
                Instantiate(_hitAtGround, hit.point, Quaternion.LookRotation(hit.normal));
                _audioSourceForHitAtObjects.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
                _audioSourceForHitAtObjects.PlayOneShot(_clipHitAtGround, _volumeForHitAtObjects);
            }
        }
        _bulletsShot--;
        if (_bulletsShot > 0 && _currentMagBullets > 0)
        {
            Invoke("ShootingByBullets", 0);
        }
    }

    public void Reloading()
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
            _cantShoot = true;
            _reloadTimeLeft -= dt;
            _anim.SetBool(_onReloadHash, true);
        }
        //reloaded
        if (_reloadTimeLeft < 0f)
        {
            Reloading();
            _reloadTimeLeft = 0f;
            _cantShoot = false;
            OnReloading = false;
            _anim.SetBool(_onReloadHash, false);
        }          

        if (_totalBullets <= 0)
            _totalBullets += 999;
    }

    public void ButtonReload()
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
