using UnityEngine;

/// <summary>
/// Apply color state to the quest object
/// </summary>
public class ColorStateApplier: MonoBehaviour
{
    public ColorState sourceColor;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = ColorStateManager.instance.StateToColor(sourceColor);
    }
}
