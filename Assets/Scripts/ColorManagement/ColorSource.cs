using UnityEngine;

public class ColorSource : MonoBehaviour
{
    public Color color;
    public ColorState colorState;


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(PlayerConstants.ACTION))
        {
            ColorStateManager.instance.UpdateColorState(colorState);
        }
    }
}
