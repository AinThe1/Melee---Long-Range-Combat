using UnityEngine;
using System.Runtime.InteropServices;

public class InputController : MonoBehaviour
{
    [SerializeField] public bool InputIsPc = false;
    [SerializeField] public bool InputIsPhone = false;

    [DllImport("__Internal")]
    private static extern bool GetDevice();

    private void Start() => ChooseInputDevice();

    private void ChooseInputDevice()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        var device = GetDevice();
        if(device) // GetDevice getting mobile
            InputIsPhone = true;
        else
            InputIsPc = true;
#endif
    }
}