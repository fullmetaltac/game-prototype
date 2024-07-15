using UnityEngine;

public class AlarmController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Game Over screen or scene
        }
    }
}
