using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public DoorType doorType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && PlayerController.instance.canTeleport)
        {
            GameManager.instance.RenderNextRoom(doorType);
            StartCoroutine(DoorAction());
        }
    }

    private IEnumerator DoorAction()
    {
        PlayerController.instance.SetTeleportVector(transform.right);
        PlayerController.instance.canTeleport = false;
        yield return new WaitForSeconds(TeleportConstants.teleportDuration);
        PlayerController.instance.canTeleport = true;
        CameraManager.isRoomEnter = true;
    }
}
