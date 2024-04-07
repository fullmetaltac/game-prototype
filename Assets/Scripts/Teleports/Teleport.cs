using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public Transform destination;

    public Vector3 portalDirection = Vector3.back;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && PlayerController.instance.canTeleport)
        {
            StartCoroutine(TeleportAction());
        }
    }


    private IEnumerator TeleportAction()
    {
        Vector3 portalToPlayer = player.transform.position - transform.position;
        float dotProcut = Vector3.Dot(transform.up, portalToPlayer);

        if (dotProcut < 0f)
        {
            float rotationDiff = -Quaternion.Angle(transform.rotation, destination.rotation);
            rotationDiff += 180;
            player.transform.Rotate(Vector3.down, rotationDiff);

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
            player.transform.position = destination.position + positionOffset;
            PlayerController.instance.SetTeleportVector(portalDirection);
        }

        PlayerController.instance.canTeleport = false;
        yield return new WaitForSeconds(TeleportConstants.teleportDuration);
        PlayerController.instance.canTeleport = true;
    }

}
