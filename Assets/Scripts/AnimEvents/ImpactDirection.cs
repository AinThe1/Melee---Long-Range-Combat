using UnityEngine;
using System.Collections;

public class ImpactDirection : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _dashSpeed;

    private bool _canDirectPlayerAttack;
    public bool CanDirectPlayerAttack { get { return _canDirectPlayerAttack ;}}

    private void ForceImpactDirection(float DashTime) // using in Events(Animation)
    {
        StartCoroutine(Dash(DashTime, _dashSpeed));
        _canDirectPlayerAttack = true;
        Invoke("DisableBool",.1f);
    }

    private void DisableBool() => _canDirectPlayerAttack = false;

    public IEnumerator Dash(float DashTime, float DashSpeed)
    {
        float startTime = Time.time;
        while (Time.time < startTime + DashTime)
        {
            Debug.Log("dash");
            var Direction = transform.forward;
            _characterController.Move(Direction * DashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
