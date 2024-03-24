using UnityEngine;

public class ColorStateManager : MonoBehaviour
{
    public Material material;
    public static ColorStateManager instance;

    private Color color;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateColorState(ColorState colorState)
    {
        Debug.Log("3");
        switch (colorState)
        {
            case ColorState.AQUA:
                color = new Color(0.360f, 0.819f, 0.679f, 1);
                break;
            case ColorState.PINK:
                color = new Color(0.819f, 0.360f, 0.717f, 1);
                break;
            case ColorState.VIOLET:
                color = new Color(0.411f, 0.360f, 0.819f, 1);
                break;
            default:
                break;
        }
        material.color = color;
        //material.EnableKeyword("_EMISSION");
        //material.SetColor("_EmissionColor", color);
    }
}