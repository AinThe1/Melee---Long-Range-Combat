using UnityEngine;
using System.Collections;

public class ImpactDirection : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;
    private float _startDashSpeed;

    private bool _canDirectPlayerAttack;
    public bool CanDirectPlayerAttack { get { return _canDirectPlayerAttack ;}}
    private void Start() => _startDashSpeed = _dashSpeed;

    private void ForceImpactDirection(float SmoothSpeed) // using in Events(Animation)
    {
        StartCoroutine(Dash(SmoothSpeed, _dashSpeed));
        _canDirectPlayerAttack = true;
        Invoke("DisableBool",.1f);
    }

    private void DisableBool() => _canDirectPlayerAttack = false;

    public IEnumerator Dash(float SmoothSpeed, float DashSpeed)
    {
        float startTime = Time.time;
        while (Time.time < startTime + _dashTime)
        {
            //smoothDash
            DashSpeed -= Time.deltaTime * SmoothSpeed;
            if (DashSpeed < 0)
                DashSpeed = 0;
            //Dash
            var Direction = transform.forward;
            _characterController.Move(Direction * DashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
