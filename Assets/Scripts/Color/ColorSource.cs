using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Color source object
/// </summary>
public class ColorSource : MonoBehaviour
{
    public ColorState sourceColor;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = ColorUtil.StateToColor(sourceColor);
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerUtil.PlayerAction(other , () => ColorStateManager.instance.UpdateState(sourceColor));
    }
}
