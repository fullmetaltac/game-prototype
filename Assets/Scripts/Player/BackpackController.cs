using System;
using UnityEngine;
using System.Collections;

public class BackpackController : MonoBehaviour
{
    public static BackpackController instance;

    public static int rotationDirection
    {
        get => PlayerPrefs.GetInt("ROTATION_DIRECTION", 1);
        set => PlayerPrefs.SetInt("ROTATION_DIRECTION", value);
    }

    public int anglePerSecond = 240;
    public float rotationTime = 0.5f;
    private float iterationAngle; // = anglePerSecond * rotationTime

    private float startAngle;
    private float targetAngle;

    private bool canRotate = true;


    private void Awake()
    {
        instance = this;
        iterationAngle = anglePerSecond * rotationTime;
        var desiredAngle = ColorToAngle(ColorStateManager.colorState);
        transform.RotateAround(transform.position, rotationDirection * transform.up, desiredAngle);
        startAngle = transform.localEulerAngles.y;
    }


    private void Start()
    {
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

        var color = GetNextColor();
        ColorStateManager.instance.UpdateState(color);

        if (targetAngle >= 360)
        {
            targetAngle = 0;
        }

        targetAngle += iterationAngle;
        canRotate = true;
    }

    private ColorState GetNextColor()
    {
        var value = (int)ColorStateManager.colorState + rotationDirection;

        if (value < 0)
        {
            value = Enum.GetNames(typeof(ColorState)).Length - 1;
            return (ColorState)value;
        }

        if (value >= Enum.GetNames(typeof(ColorState)).Length)
        {
            value = 0;
            return (ColorState)value;
        }
        return (ColorState)value;
    }

    private ColorState AngleToColor(float angle)
    {
        switch ((int)angle)
        {
            case 0:
                return ColorState.AQUA;
            case 120:
                return ColorState.VIOLET;
            case 240:
                return ColorState.ORANGE;
            case 360:
                return ColorState.AQUA;
        }
        return ColorState.AQUA;
    }

    private float ColorToAngle(ColorState color)
    {
        switch (color)
        {
            case ColorState.AQUA:
                return 0;
            case ColorState.VIOLET:
                return 120;
            case ColorState.ORANGE:
                return 240;
        }
        return 0;
    }

    public void InverseRotation()
    {
        rotationDirection *= -1;
    }
}
