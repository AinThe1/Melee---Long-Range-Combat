using UnityEngine;

public class Aim : MonoBehaviour
{
    [HideInInspector] public Canvas CanvasAim;
    private void Start() => CanvasAim = GetComponent<Canvas>();
}
