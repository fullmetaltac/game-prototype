using UnityEngine;

public class LightSource : MonoBehaviour
{
    public Material material;
    private static ColorStateManager instance;

    void Start()
    {
        //material = GetComponent<Material>();
    }

    public void UpdateColor()
    {
        var color = StateToColor(ColorStateManager.colorState);
        //material.color = color;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color);
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
