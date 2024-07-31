using UnityEngine;

public class SourceCage : MonoBehaviour
{
    public static bool isCageSourceActive = false;

    private RaycastHit hit;
    private ControllerCage cage;
    private Collider previousHitCollider;
    
    private float rayDistance = 30f;

    void Update()
    {
        if (!isCageSourceActive)
            return;

        var layerMask = LayerMask.GetMask("CageLayer");

        if (Physics.Raycast(transform.position, -transform.forward, out hit, rayDistance, layerMask))
        {
            Debug.DrawRay(transform.position, -transform.forward * rayDistance, Color.blue);
            if (hit.collider.isTrigger && hit.collider.gameObject.tag == "Cage")
            {
                cage = hit.collider.gameObject.GetComponent<ControllerCage>();
                cage.MoveUp();
            }
            previousHitCollider = hit.collider;
        }
        else
        {
            if (previousHitCollider != null)
            {
                cage?.MoveDown();   
            }
        }
    }
}
