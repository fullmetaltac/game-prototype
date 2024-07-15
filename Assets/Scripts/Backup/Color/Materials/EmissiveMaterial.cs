using UnityEngine;

public class EmissiveMaterial : MonoBehaviour, IColorSubscriber
{
    public Material material;

    private void Awake()
    {
        OnColorStateChange();
    }

    public void OnColorStateChange()
    {
        var color = ColorUtil.StateToColor(ColorStateManager.colorState);
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color);
    }
}
