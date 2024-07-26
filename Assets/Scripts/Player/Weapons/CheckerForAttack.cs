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
       //if( _meleeWeapon.AnimAttackIsPlaying)
       //
       //{
       //    _audioSourceForHit.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
       //    _audioSourceForHit.PlayOneShot(_clipsForHit, _volumeForHit);
       //}

    }

    private void OnTriggerExit(Collider other)
    {
       //if (other.gameObject.TryGetComponent<Target>(out Target target))
       //{
       //    _pauseSoundHitting = false;
       //    Invoke("UnpauseSoundHit", 0.4f);
       //}
       //
    }

    private void UnpauseSoundHit() => _pauseSoundHitting = true;
}
