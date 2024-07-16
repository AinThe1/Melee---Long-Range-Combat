using UnityEngine;
using Cinemachine;
public class TransitionBetweenAiming : MonoBehaviour
{
    [Header("Aim")]  
    [SerializeField] private CinemachineFreeLook _cinemachineFreeLook;
    [SerializeField] private CinemachineFreeLook _aimCamera;
    private bool _isAim = true;
    private bool _exitAim = false;
    private Aim _canvasAim;

    private void Start() => _canvasAim = FindObjectOfType<Aim>();

    public void Aiming()
    {
        _canvasAim.CanvasAim.enabled = true;
        _cinemachineFreeLook.Priority = 0;
        _aimCamera.Priority = 1;
        _exitAim = true;
        
        if (_isAim)
        {
            _aimCamera.m_XAxis.Value = _cinemachineFreeLook.m_XAxis.Value;
            _aimCamera.m_YAxis.Value = _cinemachineFreeLook.m_YAxis.Value;
            _isAim = false;
        }
       
    }

    public void ExitAiming()
    {
        _canvasAim.CanvasAim.enabled = false;      
        _cinemachineFreeLook.Priority = 1;
        _aimCamera.Priority = 0;
        _isAim = true;
        
        if(_exitAim) 
        {
            _cinemachineFreeLook.m_XAxis.Value = _aimCamera.m_XAxis.Value;
            _cinemachineFreeLook.m_YAxis.Value = _aimCamera.m_YAxis.Value;
            _exitAim = false;
        }
    }
       
}
