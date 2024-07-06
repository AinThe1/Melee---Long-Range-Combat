using Cinemachine;
using UnityEngine;

public class CineTouch : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cineCam;   
    //[SerializeField] private bool _isAimCamera;
    private InputController _inputController;
    private TouchField _touchField;
    private float _senstivityX = 0.1f;
    private float _senstivityY = -0.1f;

    private void Start()
    {
        _inputController = FindObjectOfType<InputController>();
        _touchField = FindObjectOfType<TouchField>();
        //ChangeSensivity();
    }
        
    
    void Update()
    {
        //if (_inputController.InputIsPhone)
        //{
        //    _cineCam.m_XAxis.Value += _touchField.TouchDist.x * 200 * _senstivityX * Time.deltaTime;
        //    _cineCam.m_YAxis.Value += _touchField.TouchDist.y * _senstivityY * Time.deltaTime;
        //}
        //
        //if(_inputController.InputIsPc) 
        //{
        //    _cineCam.m_XAxis.m_MaxSpeed = _senstivityX;
        //    _cineCam.m_YAxis.m_MaxSpeed = -_senstivityY / 100;
        //}
    }

    //public void ChangeSensivity()
    //{
    //    //SaveGame.Load();
    //    _senstivityX = SaveGame.Instance.PlayerInfo.FreeCameraSensivity;
    //    _senstivityY = SaveGame.Instance.PlayerInfo.FreeCameraSensivity - (SaveGame.Instance.PlayerInfo.FreeCameraSensivity * 2);
    //    if (_isAimCamera)
    //    {
    //        _senstivityX = SaveGame.Instance.PlayerInfo.AimCameraSensivity;
    //        _senstivityY = SaveGame.Instance.PlayerInfo.AimCameraSensivity - (SaveGame.Instance.PlayerInfo.AimCameraSensivity * 2);
    //    }
    //}
}
