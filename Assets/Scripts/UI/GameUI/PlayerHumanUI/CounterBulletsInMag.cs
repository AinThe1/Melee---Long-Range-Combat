using UnityEngine;
using TMPro;

public class CounterBulletsInMag : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI BulletsInMag;

    private void Start() => BulletsInMag = GetComponent<TextMeshProUGUI>();
}
