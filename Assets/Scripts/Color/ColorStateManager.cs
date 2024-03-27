using UnityEngine;

/// <summary>
/// Character part with dynamic color
/// </summary>
public class ColorStateManager : MonoBehaviour
{
    public Material material;
    public static ColorStateManager instance;

    public static ColorState colorState
    {
        get => (ColorState)PlayerPrefs.GetInt("COLOR_STATE", 0);
        set => PlayerPrefs.SetInt("COLOR_STATE", (int)value);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateColorState(colorState);
    }

    public void UpdateColorState(ColorState colorState)
    {
        material.color = StateToColor(colorState);
        ColorStateManager.colorState = colorState;
        //material.EnableKeyword("_EMISSION");
        //material.SetColor("_EmissionColor", color);
    }

    public Color StateToColor(ColorState colorState)
    {
        switch (colorState)
        {
            case ColorState.AQUA:
                return ColorConstants.AquaColor;
            case ColorState.PINK:
                return ColorConstants.PinkColor;
            case ColorState.VIOLET:
                return ColorConstants.VioletColor;
        }
        return ColorConstants.AquaColor;
    }
}