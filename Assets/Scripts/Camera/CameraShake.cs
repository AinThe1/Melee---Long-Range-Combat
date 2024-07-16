using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    private CinemachineFreeLook AimCamera;
    private float _shakeTimer;

    private void Start() => AimCamera = GetComponent<CinemachineFreeLook>();
    private void Update() => TimerShakeCamera();

    public void TryShakeCamera(float intensity,float time)
    {
        Shake(intensity);
        _shakeTimer = time;
    }
    
    private void TimerShakeCamera()
    {
        if(_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0) //end
                Shake(.5f);
        }
    }

    private void Shake(float Intensity)
    {
        CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig0 =
          AimCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig1 =
          AimCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig2 =
          AimCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        CinemachineBasicMultiChannelPerlinRig0.m_FrequencyGain = Intensity;
        CinemachineBasicMultiChannelPerlinRig1.m_FrequencyGain = Intensity;
        CinemachineBasicMultiChannelPerlinRig2.m_FrequencyGain = Intensity;
    }
}
