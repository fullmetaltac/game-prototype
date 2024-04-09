using UnityEngine;
using System.Collections;

public class BackpackRotation : MonoBehaviour
{
    public int anglePerSecond = 240;
    public float rotationTime = 0.5f;
    private float iterationAngle;

    private float startAngle;
    private float targetAngle;
    private Vector3 defaultAngles;

    private bool canRotate = true;


    private void Start()
    {
        defaultAngles = transform.eulerAngles;
        startAngle = defaultAngles.y;
        iterationAngle = anglePerSecond * rotationTime;
        targetAngle = startAngle + iterationAngle;
    }



    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.ROTATE_BACKPACK) && canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateObject());
        }
    }

    IEnumerator RotateObject()
    {
        float safeOffset = 10f;
        float accumulator = 0.0f;
        float elapsedTime = 0.0f;
       
        while (elapsedTime < rotationTime)
        {
            var rotateAmount = anglePerSecond * Time.deltaTime;
            if(accumulator < iterationAngle - safeOffset)
            {
                transform.RotateAround(transform.position, transform.up, rotateAmount);
                accumulator += rotateAmount;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.RotateAround(transform.position, transform.up, iterationAngle - accumulator);

     
        if (targetAngle >= 360)
        {
            targetAngle = 0;
            transform.eulerAngles = defaultAngles;
        }

        targetAngle += iterationAngle;
        canRotate = true;
    }
}
