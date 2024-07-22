using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    [Range(-1, 1)]
    [SerializeField]
    public int rotationDirection = 1;

    public int iterationAngle = 90;
    public float rotationTime = 1f;
    public RotationAxis rotationAxis = RotationAxis.Y;

    private float startAngle;
    private float targetAngle;
    private bool canRotate = true;

    private void Start()
    {
        switch (rotationAxis)
        {
            case RotationAxis.Y:
                startAngle = transform.eulerAngles.y;
                break;
            case RotationAxis.X:
                startAngle = transform.eulerAngles.x;
                break;
            case RotationAxis.Z:
                startAngle = transform.eulerAngles.z;
                break;
        }
        targetAngle = startAngle + iterationAngle;
    }


    private void OnTriggerStay(Collider other)
    {
        PlayerUtil.PlayerAction(other, canRotate, () =>
        {

            canRotate = false;
            StartCoroutine(ApplyRotate());
        });
    }

    IEnumerator ApplyRotate()
    {
        float elapsedTime = 0.0f;
        float rotationAmount = targetAngle - startAngle;

        while (elapsedTime < rotationTime)
        {
            float angle = rotationDirection * Mathf.Lerp(startAngle, startAngle + rotationAmount, elapsedTime / rotationTime);

            switch (rotationAxis)
            {
                case RotationAxis.Y:
                    transform.rotation = Quaternion.Euler(0, angle, 0);
                    break;
                case RotationAxis.X:
                    transform.rotation = Quaternion.Euler(angle, 0, 0);
                    break;
                case RotationAxis.Z:
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                    break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        switch (rotationAxis)
        {
            case RotationAxis.Y:
                transform.rotation = Quaternion.Euler(0, rotationDirection * targetAngle, 0);
                break;
            case RotationAxis.X:
                transform.rotation = Quaternion.Euler(rotationDirection * targetAngle, 0, 0);
                break;
            case RotationAxis.Z:
                transform.rotation = Quaternion.Euler(0, 0, rotationDirection * targetAngle);
                break;
        }

        if (targetAngle >= 360)
        {
            targetAngle = 0;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (transform.eulerAngles.x < 0)
        {
            targetAngle = -targetAngle;
        }

        startAngle = targetAngle;
        targetAngle += iterationAngle;

        canRotate = true;
    }
}
