using UnityEngine;
using Cinemachine;
public class TransitionBetweenAiming : MonoBehaviour
{
    [Header("Aim")]  
    [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
    [SerializeField] private CinemachineFreeLook _aimCamera;
    [HideInInspector] public bool IsAim = true;
    [HideInInspector] public bool ExitAim = false;
    private Aim _canvasAim;

    private void Start() => _canvasAim = FindObjectOfType<Aim>();

    public void Aiming()
    {
        _canvasAim.CanvasAim.enabled = true;
        _cinemachineFreeLook.Priority = 0;
        _aimCamera.Priority = 1;
        ExitAim = true;
        
        if (IsAim)
        {
            _aimCamera.m_XAxis.Value = _cinemachineFreeLook.m_XAxis.Value;
            _aimCamera.m_YAxis.Value = _cinemachineFreeLook.m_YAxis.Value;
            IsAim = false;
        }
       
    }

    public void ExitAiming()
    {
        _canvasAim.CanvasAim.enabled = false;
        _aimCamera.Priority = 0;
        _cinemachineFreeLook.Priority = 1;
        IsAim = true;
        
        if(ExitAim) 
        {
            _cinemachineFreeLook.m_XAxis.Value = _aimCamera.m_XAxis.Value;
            _cinemachineFreeLook.m_YAxis.Value = _aimCamera.m_YAxis.Value;
            ExitAim = false;
        }
    }
       
}
