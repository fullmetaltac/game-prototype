using UnityEngine;

public class ColorInverser : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        PlayerUtil.PlayerAction(other, () =>
        {
            BackpackController.instance.InverseRotation();
            Debug.Log("Rotation inversed.");
        });
    }
}
