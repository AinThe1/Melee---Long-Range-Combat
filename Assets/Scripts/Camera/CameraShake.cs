using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    [HideInInspector] public CinemachineFreeLook[] _Cameras;
    private float _shakeTimer;

    private void Start() => _Cameras = FindObjectsOfType<CinemachineFreeLook>();
    private void Update() => TimerShakeCamera();
    public void TryShakeCamera(float intensity,float time)
    {
        foreach(var cams in _Cameras)
        {
            CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig0 =
              cams.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig1 =
              cams.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig2 =
              cams.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            CinemachineBasicMultiChannelPerlinRig0.m_FrequencyGain = intensity;
            CinemachineBasicMultiChannelPerlinRig1.m_FrequencyGain = intensity;
            CinemachineBasicMultiChannelPerlinRig2.m_FrequencyGain = intensity;
        }

        _shakeTimer = time;
    }
    
    private void TimerShakeCamera()
    {
        if(_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0) //end
            {
                foreach(var cams in _Cameras)
                {
                    CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig0 =
                      cams.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                    CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig1 =
                      cams.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                    CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlinRig2 =
                      cams.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                   
                    CinemachineBasicMultiChannelPerlinRig0.m_FrequencyGain = .5f;
                    CinemachineBasicMultiChannelPerlinRig1.m_FrequencyGain = .5f;
                    CinemachineBasicMultiChannelPerlinRig2.m_FrequencyGain = .5f;
                }              
            }
        }
    }
}
