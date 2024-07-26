using UnityEngine;

public class SoundStep : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSourceForStep;
    [SerializeField] private AudioClip[] _clipsForStep;
    [SerializeField] private float _volumeForStep;

    private void PlaySoundStep() // using in Events(Animation)
    {
        var randomStepSound = UnityEngine.Random.Range(0, _clipsForStep.Length);
        _audioSourceForStep.PlayOneShot(_clipsForStep[randomStepSound], _volumeForStep);
        Debug.Log("step");
    }
}
