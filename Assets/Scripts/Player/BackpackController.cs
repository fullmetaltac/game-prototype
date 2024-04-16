using UnityEngine;
using System.Collections;
using System;


public enum BackPackState
{
    COLOR_0 = 0, // NONE
    COLOR_1 = 1, // AQUA
    COLOR_2 = 2, // AQUA + VIOLET
    COLOR_3 = 3  // AQUA + VIOLET + ORANGE
}

public class BackpackController : MonoBehaviour
{
    public static BackpackController instance;

    public static int rotationDirection
    {
        get => PlayerPrefs.GetInt("ROTATION_DIRECTION", 1);
        set => PlayerPrefs.SetInt("ROTATION_DIRECTION", value);
    }

    public static BackPackState backPackState
    {
        get => (BackPackState)PlayerPrefs.GetInt("BACKPACK_STATE", 0);
        set => PlayerPrefs.SetInt("BACKPACK_STATE", (int)value);
    }

    public float rotationTime = 0.5f;

    private float targetAngle;
    private float currentAngle;
    private bool canRotate = true;


    private void Awake()
    {
        instance = this;
        currentAngle = ColorToAngle();
        transform.RotateAround(transform.position, rotationDirection * transform.up, currentAngle);
    }

    private void Update()
    {
        if (Input.GetKeyDown(PlayerConstants.ROTATE_BACKPACK) && backPackState > 0 && canRotate)
        {
            canRotate = false;
            StartCoroutine(RotateBackpack());
        }
    }

    private float GetAngularSpeed()
    {
        switch (backPackState)
        {
            case BackPackState.COLOR_0:
                return 0;
            case BackPackState.COLOR_1:  // AQUA
                return 720;
            case BackPackState.COLOR_2:  // AQUA + VIOLET
                if (ColorStateManager.colorState == ColorState.VIOLET && rotationDirection == 1)
                {
                    return 480;
                }
                if (ColorStateManager.colorState == ColorState.AQUA && rotationDirection == -1)
                {
                    return 480;
                }
                return 240;

            case BackPackState.COLOR_3:  // AQUA + VIOLET + ORANGE
                return 240;
        }
        return 0;
    }

    IEnumerator RotateBackpack()
    {
        float safeOffset = 10f;
        float accumulator = 0.0f;
        float elapsedTime = 0.0f;

        var iterationAngle = GetAngularSpeed() * rotationTime;

        while (elapsedTime < rotationTime)
        {
            var rotateAmount = GetAngularSpeed() * Time.deltaTime;
            if (accumulator < iterationAngle - safeOffset)
            {
                transform.RotateAround(transform.position, rotationDirection * transform.up, rotateAmount);
                accumulator += rotateAmount;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.RotateAround(transform.position, rotationDirection * transform.up, iterationAngle - accumulator);

        if (currentAngle >= 360)
            currentAngle = 0;

        currentAngle += GetAngularSpeed() * rotationTime;
        if ((int) backPackState > 1)
            ColorStateManager.instance.UpdateState(GetNextColor());
        canRotate = true;
    }


    private ColorState GetNextColor()
    {
        
        if (backPackState == BackPackState.COLOR_2)
        {
            return (int)ColorStateManager.colorState == 1 ? ColorState.AQUA : ColorState.VIOLET;

        }
        if (backPackState == BackPackState.COLOR_3)
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

        return ColorState.AQUA;
    }

    private float ColorToAngle()
    {
        switch (ColorStateManager.colorState)
        {
            case ColorState.AQUA:
                return 0;
            case ColorState.VIOLET:
                return 120 * rotationDirection;
            case ColorState.ORANGE:
                return 240 * rotationDirection;
        }
        return 0;
    }

    public void InverseRotation()
    {
        rotationDirection *= -1;
    }

    public void UpdateBackPackState(BackPackState state)
    {
        backPackState = state;
    }
}
