using UnityEngine;
using System.Collections;

public class ManagerCamera : MonoBehaviour
{
    public Vector3 offset;
    public Transform target;
    public float smoothSpeed = 5f;

    public static ManagerCamera instance;

    private float currentAngle;
    private float rotationTime = .5f;
    private int angularSpeed = 180; // deg per second

    public static bool canRotate = true;
    public static bool isRoomEnter = false;

    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        if (isRoomEnter && canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateCamera());
        }

        if (canRotate)
        {
            Vector3 desiredPosition = target.position;
            transform.position = Vector3.Lerp(target.position, desiredPosition, smoothSpeed *  Time.deltaTime);
        }
    }

    IEnumerator RotateCamera()
    {
        var safeOffset = 5f;
        var accumulator = 0.0f;
        var elapsedTime = 0.0f;
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
        isRoomEnter = false;
        canRotate = true;
    }
}
