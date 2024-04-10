using UnityEngine;

public class ColorMaterial : MonoBehaviour, IColorSubscriber
{
    public Material material;

    private void Awake()
    {
        OnColorStateChange();
    }
    public void OnColorStateChange()
    {
        material.color = ColorUtil.StateToColor(ColorStateManager.colorState);
    }
}
