using UnityEngine;

public class LightSource : MonoBehaviour, IColorSubscriber
{
    public Material material;

    public void OnColorStateChange()
    {
        var color = ColorUtil.StateToColor(ColorStateManager.colorState);
        //material.color = color;
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color);
    }
}
