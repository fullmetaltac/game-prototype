using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public Transform destination;

    [SerializeField]
    ColorStateApplier colorStateApplier;
    private ColorState portalColor;
    private BoxCollider collirder;

    public Vector3 portalDirection = Vector3.back;

    private void Start()
    {
        collirder = GetComponent<BoxCollider>();
        colorStateApplier = GetComponent<ColorStateApplier>();
        portalColor = colorStateApplier.sourceColor;
    }

    private void Update()
    {
        if (ColorStateManager.colorState == portalColor)
        {
            collirder.isTrigger = true;
        }
        else
        {
            collirder.isTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerColorState = ColorStateManager.colorState;
        if (other.tag == "Player" && PlayerController.instance.canTeleport && playerColorState == portalColor)
        {
            if (destination != null)
            {
                StartCoroutine(TeleportAction());
            }
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
            PlayerController.instance.moveSpeed /= 2;
        }

        PlayerController.instance.canTeleport = false;
        yield return new WaitForSeconds(TeleportConstants.teleportDuration);
        PlayerController.instance.canTeleport = true;
        PlayerController.instance.moveSpeed *= 2;
    }
}
