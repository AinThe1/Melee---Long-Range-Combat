using UnityEngine;

public class AttackCheckerFromZombie : MonoBehaviour
{     
    [HideInInspector] public bool AtZoneForAttack;
    [HideInInspector] public Collider Target;
    private LayerMask LayersMask;

   //private void OnTriggerStay(Collider other)
   //{
   //    if (other.gameObject.TryGetComponent<HpHuman>(out HpHuman Human))
   //    {
   //        if(!Human.IsDead)
   //        {
   //            RaycastHit hit;
   //            var direction = new Vector3(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y,
   //                other.transform.position.z - transform.position.z).normalized;
   //
   //            if (Physics.Raycast(transform.position, direction, out hit, 1000))
   //            {
   //                if (hit.collider.gameObject.GetComponent<HpHuman>())
   //                    AtZoneForAttack = true;
   //                else
   //                    AtZoneForAttack = false;
   //            }
   //            Target = other;
   //        }          
   //    }       
   //}
   //
   //private void OnTriggerExit(Collider other)
   //{
   //    if (other.gameObject.TryGetComponent<HpHuman>(out HpHuman Human))
   //    {
   //        AtZoneForAttack = false;
   //        Target = null;
   //    }
   //}
}
