using UnityEngine;

public class ColorSource : MonoBehaviour
{
    public ColorState sourceColor;
    public BackPackState backPackState;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        if ((int)backPackState > (int)BackpackController.backPackState)
            _renderer.material.color = ColorUtil.StateToColor(sourceColor);
        else
            _renderer.material.color = ColorConstants.GrayColor;
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerUtil.PlayerAction(other, (int)BackpackController.backPackState < (int)backPackState, () =>
        {
            BackpackController.backPackState = backPackState;
            _renderer.material.color = ColorConstants.GrayColor;
        });
    }
}
