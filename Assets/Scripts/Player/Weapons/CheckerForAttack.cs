using UnityEngine;

public class CheckerForAttack : MonoBehaviour
{
    [SerializeField] private MeleeWeapon _meleeWeapon;
    [SerializeField] private AudioSource _audioSourceForHit;
    [SerializeField] private AudioClip _clipsForHit;
    [SerializeField] private float _volumeForHit;
    private bool _pauseSoundHitting = true;

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.CompareTag("Enemy") && _meleeWeapon.AnimAttackIsPlaying && _pauseSoundHitting)
       {
           _audioSourceForHit.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
           _audioSourceForHit.PlayOneShot(_clipsForHit, _volumeForHit);          
            _pauseSoundHitting = false;
            Debug.Log("hit");
       }
    }

    private void OnTriggerExit(Collider other) => Invoke("UnpauseHit", 0.4f);
    private void UnpauseHit() => _pauseSoundHitting = true;
}
