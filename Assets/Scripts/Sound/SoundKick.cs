using UnityEngine;

public class SoundKick : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSourceForKick;
    [SerializeField] private AudioClip _clipsForKick;
    [SerializeField] private float _volumeForKick;

    private void PlaySoundKick() // using in Events(Animation)
    {
        _audioSourceForKick.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
        _audioSourceForKick.PlayOneShot(_clipsForKick, _volumeForKick);
    }
}
