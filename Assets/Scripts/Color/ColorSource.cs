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
        _renderer.material.color = ColorStateManager.instance.StateToColor(sourceColor);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(PlayerConstants.ACTION))
        {
            ColorStateManager.instance.UpdateColorState(sourceColor);
        }
    }
}
