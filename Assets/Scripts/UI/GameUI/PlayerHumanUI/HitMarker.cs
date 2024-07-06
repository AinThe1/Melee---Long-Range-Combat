using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    [HideInInspector] public Image ImageHitMarker;
    private void Start() => ImageHitMarker = GetComponent<Image>();
}
