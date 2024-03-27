using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Vector3 amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CameraFollow.instance.DecreaseOffset(amount);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CameraFollow.instance.IncreaseOffset(amount);
        }

    }
}
