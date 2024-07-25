using UnityEngine;

public class DamageDeal : MonoBehaviour
{
    [SerializeField] private CheckerForAttack _checkerForAttack;   
    [SerializeField] private int _forceDamage;
    [SerializeField] private AudioSource _audioSourceForHit;
    [SerializeField] private AudioClip[] _clipsHit;
    [SerializeField] private float _volumeForHit;


    private void HitTarget()// using in animations Events
    {
        if (_checkerForAttack.Target == null )
            return;

        if (_checkerForAttack.AtZoneForAttack)
        {
            var random = UnityEngine.Random.Range(0, _clipsHit.Length);
            _audioSourceForHit.PlayOneShot(_clipsHit[random], _volumeForHit);
        }
    }
}
