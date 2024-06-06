
using UnityEngine;
using System.Collections;

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


    private Vector3 CalculateOffset(float scale = 10)
    {
        var direction = transform.position - target.position;
        return direction.normalized * scale;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateCamera());
        }

        if (canRotate)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(target.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    private float currentAngle;
    private bool canRotate = true;
    private int angularSpeed = 180; // deg per second
    private float rotationTime = .5f;


    IEnumerator RotateCamera()
    {
        float safeOffset = 10f;
        float accumulator = 0.0f;
        float elapsedTime = 0.0f;
        
        var iterationAngle = angularSpeed * rotationTime;

        while (elapsedTime < rotationTime)
        {
            var rotateAmount = angularSpeed * Time.deltaTime;
            if (accumulator < iterationAngle - safeOffset)
            {
                transform.RotateAround(target.position, target.transform.up, rotateAmount);
                accumulator += rotateAmount;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.RotateAround(target.position, target.transform.up, iterationAngle - accumulator);

        if (currentAngle >= 360)
            currentAngle = 0;

        currentAngle += angularSpeed * rotationTime;

        offset = CalculateOffset();
        canRotate = true;
    }


    public void IncreaseOffset(Vector3 amount)
    {
        offset += amount;
    }

    public void DecreaseOffset(Vector3 amount)
    {
        offset -= amount;
    }

}
