using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float raycastLength = 1f;
    public LayerMask platformLayer;

    private Rigidbody rb;
    private Quaternion platformRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        platformLayer = LayerMask.GetMask("BedLayer");
    }

    void Update()
    {
        if (IsOnPlatform())
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, raycastLength, platformLayer))
            {
                platformRotation = hit.transform.rotation;
            }
            rb.MoveRotation(platformRotation);
        }
    }

    bool IsOnPlatform()
    {
        return Physics.Raycast(transform.position, -Vector3.up, raycastLength, platformLayer);
    }
}
