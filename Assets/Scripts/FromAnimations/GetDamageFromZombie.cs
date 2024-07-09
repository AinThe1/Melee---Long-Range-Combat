using System;
using UnityEngine;

public class GetDamageFromZombie : MonoBehaviour
{
    [SerializeField] private CheckerForAttack _attackCheckerFromZombie;   
    [SerializeField] public float ForceDamage;
    [HideInInspector] public int IntForseDamage;
    [SerializeField] private AudioSource _audioSourceForHit;
    [SerializeField] private AudioClip[] _clipsHit;
    [SerializeField] private float _volumeForHit;

    private void Start() => IntForseDamage = Convert.ToInt32(ForceDamage);

    private void HitHuman()// using in animations Events
    {
        if (_attackCheckerFromZombie.Target == null)
            return;

        if (_attackCheckerFromZombie.AtZoneForAttack)
        {
            var random = UnityEngine.Random.Range(0, _clipsHit.Length);
            _audioSourceForHit.PlayOneShot(_clipsHit[random], _volumeForHit);
        }
    }
}
