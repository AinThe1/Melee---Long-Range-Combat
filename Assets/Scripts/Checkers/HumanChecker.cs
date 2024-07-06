using UnityEngine;

public class HumanChecker : MonoBehaviour
{
    [HideInInspector] public bool AtZoneForFollow = false;
    [HideInInspector] public Collider Target;

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.TryGetComponent<HpHuman>(out HpHuman Human))
    //    {
    //        RaycastHit hit;
    //        var direction = new Vector3(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y,
    //            other.transform.position.z - transform.position.z).normalized;
    //        // use if you want to change checker (take from checker possibility checks through wolrd)
    //        //if (Physics.Raycast(transform.position, direction, out hit, 1000))
    //        //{
    //        //    if (hit.collider.gameObject.GetComponent<Target>())
    //        //        AtZoneForFollow = true;
    //        //    else
    //        //        AtZoneForFollow = false;
    //        //}
    //        AtZoneForFollow = true;
    //        Target = other;
    //    }
    //}
    //
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.TryGetComponent<HpHuman>(out HpHuman Human))
    //    {
    //        AtZoneForFollow = false;
    //    }
    //}
}
