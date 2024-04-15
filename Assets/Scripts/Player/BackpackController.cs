using System;
using UnityEngine;
using System.Collections;

public class BackpackController : MonoBehaviour
{
    [Range(-1, 1)]
    [SerializeField]
    public int rotationDirection = 1;

    public int anglePerSecond = 240;
    public float rotationTime = 0.5f;
    private float iterationAngle;

    public static BackpackController instance;

    private float startAngle;
    private float targetAngle;

    private bool canRotate = true;


    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        startAngle = transform.localEulerAngles.y;
        iterationAngle = anglePerSecond * rotationTime;
        targetAngle = startAngle + iterationAngle;
    }



    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.ROTATE_BACKPACK) && canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateBackpack());
        }
    }

    IEnumerator RotateBackpack()
    {
        float safeOffset = 10f;
        float accumulator = 0.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < rotationTime)
        {
            var rotateAmount = anglePerSecond * Time.deltaTime;
            if (accumulator < iterationAngle - safeOffset)
            {
                transform.RotateAround(transform.position, rotationDirection * transform.up, rotateAmount);
                accumulator += rotateAmount;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.RotateAround(transform.position, rotationDirection * transform.up, iterationAngle - accumulator);

        ColorStateManager.instance.UpdateState(AngleToColor(targetAngle));

        if (targetAngle >= 360)
        {
            targetAngle = 0;
        }

        targetAngle += iterationAngle;
        canRotate = true;
    }

    private ColorState AngleToColor(float angle)
    {
        switch ((int)angle)
        {
            case 0:
                return ColorState.AQUA;
            case 120:
                return ColorState.ORANGE;
            case 240:
                return ColorState.VIOLET;
            case 360:
                return ColorState.AQUA;
        }
        return ColorState.AQUA;
    }

    public void InverseRotation()
    {
        rotationDirection *= -1;
    }
}
