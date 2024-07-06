using UnityEngine;
using System.Collections;
public class CharachterSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceCharachterSounds;
    [SerializeField] private AudioClip[] _audioClips;
    //Random Values For repeat clip
    [SerializeField] private int _minRandomTimelock;
    [SerializeField] private int _maxRandomTimelock;

    private void Start() => StartCoroutine(RepeatClip());
    private IEnumerator RepeatClip()
    {
        while (true)
        {
            var RandomTime = Random.Range(_minRandomTimelock, _maxRandomTimelock);
            yield return new WaitForSeconds(RandomTime);
            var RandomAudioClip = Random.Range(0,_audioClips.Length);
            _audioSourceCharachterSounds.PlayOneShot(_audioClips[RandomAudioClip],0.8f);
        }
    }
}
