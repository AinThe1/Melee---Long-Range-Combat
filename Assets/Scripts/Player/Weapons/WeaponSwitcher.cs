using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _meleePersonPrefab;
    [SerializeField] private GameObject _longRangePersonPrefab;
    private void Update() => Switcher();

    private void Switcher()
    {       
        if (Input.GetKeyDown("1"))
        {
            _longRangePersonPrefab.SetActive(false);
            _meleePersonPrefab.SetActive(true);
        }
            
        if (Input.GetKeyDown("2"))
        {
            _longRangePersonPrefab.SetActive(true);
            _meleePersonPrefab.SetActive(false);         
        }           
    }
}
