
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.08f;
    public static CameraFollow instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        transform.position = target.position + offset;
    }


    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void IncreaseOffset(Vector3 amount)
    {
        offset = offset + amount;
    }

    public void DecreaseOffset(Vector3 amount)
    {
        offset = offset - amount;
    }

}
