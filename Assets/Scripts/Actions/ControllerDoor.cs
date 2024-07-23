using UnityEngine;
using System.Collections;


public enum DoorSate
{
    OPEN, CLOSED, MOVING
}
public class ControllerDoor : MonoBehaviour
{
    public float rotationTime = 1f;
    public int rotationDirection = 1;

    private Vector3 pivot;
    private DoorSate doorState;
    private bool canRotate = true;

    private void Start()
    {
        doorState = DoorSate.CLOSED;

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

    public void Open()
    {
        if (doorState == DoorSate.CLOSED)
        {
            canRotate = false;
            rotationDirection *= -1;
            doorState = DoorSate.MOVING;
            StartCoroutine(ApplyRotate(DoorSate.OPEN));
        }
    }

    public void Close()
    {
        if (doorState == DoorSate.OPEN)
        {
            canRotate = false;
            rotationDirection *= -1;
            doorState = DoorSate.MOVING;
            StartCoroutine(ApplyRotate(DoorSate.CLOSED));
        }
    }

    IEnumerator ApplyRotate(DoorSate setState)
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
        doorState = setState;
        canRotate = true;
    }
}
