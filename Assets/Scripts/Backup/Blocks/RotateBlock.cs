using System.Collections;
using UnityEngine;

public class RotateBlock : MonoBehaviour
{
    [Range(-1, 1)]
    [SerializeField]
    public int rotationDirection = 1;

    public int iterationAngle = 90;
    public float rotationTime = 0.5f;
    public RotationAxis rotationAxis = RotationAxis.Y;

    [SerializeField]
    ColorStateApplier colorStateApplier;

    private float startAngle;
    private float targetAngle;
    private bool canRotate = true;


    private void Start()
    {
        targetAngle = iterationAngle;
        colorStateApplier = GetComponent<ColorStateApplier>();


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
    }


    private void OnTriggerStay(Collider other)
    {
        var playerColorState = ColorStateManager.colorState;
        var platformColorState = colorStateApplier.sourceColor;
        if (other.tag == "Player" && Input.GetKeyDown(PlayerConstants.ACTION) && platformColorState == playerColorState && canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateObject());
        }
    }

    IEnumerator RotateObject()
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
