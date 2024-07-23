using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour
{
    public float rotationTime = 1f;
    public int rotationDirection = 1;

    private Vector3 pivot;
    private bool canRotate = true;

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        var xDim = renderer.bounds.extents.x;
        var zDim = renderer.bounds.extents.z;
        var depth = xDim > zDim ? zDim : xDim;

        pivot = transform.position - RoomSize.center;
        pivot += pivot.normalized * depth / 2; 

        if (zDim > xDim)
            pivot += new Vector3(0, 0, zDim);
        else
            pivot += new Vector3(-xDim, 0, 0);

        if (transform.name.Contains("LEFT") || transform.name.Contains("BOTTOM"))
            rotationDirection *= -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canRotate)
        {
                canRotate = false;
                rotationDirection *= -1;
                StartCoroutine(ApplyRotate());
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

            transform.RotateAround(pivot, Vector3.up, rotationDirection * (nextAngle - angle));
            elapsedTime += Time.deltaTime;
            angle = nextAngle;
            yield return null;
        }
        transform.RotateAround(pivot, Vector3.up, rotationDirection * (rotationAngle - angle));
        canRotate = true;
    }
}
