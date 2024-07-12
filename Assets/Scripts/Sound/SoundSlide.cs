using UnityEngine;

public class SoundSlide : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSourceForSlide;
    [SerializeField] private AudioClip _clipsForSlide;
    [SerializeField] private float _volumeForSlide;

    private void PlaySoundSlide() // using in Events(Animation)
    {
        _audioSourceForSlide.pitch = UnityEngine.Random.Range(0.95f, 1.15f);
        _audioSourceForSlide.PlayOneShot(_clipsForSlide, _volumeForSlide);
    }
}
