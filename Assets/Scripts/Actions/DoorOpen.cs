using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour
{
    public GameObject pivot;
    private bool canRotate = true;
    public float rotationTime = 1f;
    public int rotationDirection = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (canRotate)
            {
                canRotate = false;
                rotationDirection *= -1;
                StartCoroutine(ApplyRotate());
            }
        }
    }

    IEnumerator ApplyRotate()
    {
        float angle = 0f;
        float elapsedTime = 0.0f;
        float rotationAngle = 90f;

        while (elapsedTime < rotationTime)
        {
            var nextAngle = Mathf.LerpAngle(0f, rotationAngle, elapsedTime / rotationTime);

            transform.RotateAround(pivot.transform.position, Vector3.up, rotationDirection * (nextAngle - angle));
            elapsedTime += Time.deltaTime;
            angle = nextAngle;
            yield return null;
        }
        transform.RotateAround(pivot.transform.position, Vector3.up, rotationDirection * (rotationAngle - angle));
        canRotate = true;
    }
}
