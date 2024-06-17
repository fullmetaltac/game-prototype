using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public DoorType doorType;
    private Vector3 moveDirection;

    private void Start()
    {
        moveDirection = transform.position - GameManager.instance.room.center;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && PlayerController.instance.isFreeMove)
        {
            StartCoroutine(DoorAction());
            GameManager.instance.RenderNextRoom(doorType);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator DoorAction()
    {
        PlayerController.instance.SetFixedDirection(moveDirection);
        PlayerController.instance.isFreeMove = false;        
        yield return new WaitForSeconds(TeleportConstants.teleportDuration);
        PlayerController.instance.isFreeMove = true;
        CameraManager.isRoomEnter = true;
    }
}
