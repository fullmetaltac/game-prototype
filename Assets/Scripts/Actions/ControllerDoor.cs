using UnityEngine;
using System.Collections;


public enum DoorSate
{
    OPEN, CLOSED, MOVING
}
public class ControllerDoor : MonoBehaviour
{
    public float rotationTime = 1f;
    public int rotationDirection = 1;
    public WallLocation doorLocation;
    public bool canWalkInDoor = false;


    private Vector3 pivot;
    private DoorSate doorState;
    private bool canRotate = true;
    private ManagerGame gameManager;
    private PlayerController player;


    private void Awake()
    {
        gameManager = ManagerGame.instance;
        player = PlayerController.instance;
    }

    private void Start()
    {
        doorState = DoorSate.CLOSED;
        CalculatePivot();
        if (transform.name.Contains("LEFT") || transform.name.Contains("BOTTOM"))
            rotationDirection *= -1;
    }

    private void CalculatePivot()
    {
        var renderer = GetComponent<Renderer>();
        var xDim = renderer.bounds.extents.x;
        var zDim = renderer.bounds.extents.z;
        var depth = xDim > zDim ? zDim : xDim;

        pivot = transform.position - RoomSize.center;
        pivot = transform.position;
        pivot += pivot.normalized * depth / 2;

        if (zDim > xDim)
            pivot += new Vector3(0, 0, zDim);
        else
            pivot += new Vector3(-xDim, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canWalkInDoor && player.isMoving)
        {
            StartCoroutine(WalkThroughDoor());
            ManagerGame.instance.RenderNextRoom(doorLocation);
            //GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator WalkThroughDoor()
    {
        player.SetFixedDirection(transform.position - RoomSize.center);
        player.isMoving = false;
        SourceCage.isCageSourceActive = false;
        SourceLight.isLightSourceActive = false;
        yield return new WaitForSeconds(ManagerCamera.camRotateDelay);
        ManagerCamera.isRoomEnter = true;
        yield return new WaitForSeconds(player.doorMoveDuration);
        StartCoroutine(gameManager.room.ToggleDoorsColliders(true, () => gameManager.DeRenderRoom()));
        player.isMoving = true;
    }

    public void Open()
    {
        if (doorState == DoorSate.CLOSED)
        {
            canRotate = false;
            rotationDirection *= -1;
            doorState = DoorSate.MOVING;
            StartCoroutine(ApplyRotate(DoorSate.OPEN, 2, true));
        }
    }

    public void Close()
    {
        if (doorState == DoorSate.OPEN)
        {
            canRotate = false;
            rotationDirection *= -1;
            doorState = DoorSate.MOVING;
            StartCoroutine(ApplyRotate(DoorSate.CLOSED, 0.5f, false));            
        }
    }

    IEnumerator ApplyRotate(DoorSate setState, float scaleFactor, bool canWalk)
    {
        float angle = 0f;
        float elapsedTime = 0.0f;
        float rotationAngle = 90f;
        while (elapsedTime < rotationTime)
        {
            var nextAngle = Mathf.LerpAngle(0f, rotationAngle, elapsedTime / rotationTime);

            transform.RotateAround(pivot, Vector3.up, rotationDirection * (nextAngle - angle));
            elapsedTime += Time.deltaTime;
            angle = nextAngle;
            yield return null;
        }
        transform.RotateAround(pivot, Vector3.up, rotationDirection * (rotationAngle - angle));
        ModifyBoxCollider(scaleFactor);
        doorState = setState;
        canWalkInDoor = canWalk;
        canRotate = true;
    }

    private void ModifyBoxCollider(float multiplier)
    {
        var boxCollider = GetComponent<BoxCollider>();
        Vector3 newSize = boxCollider.size;
        newSize.x *= multiplier;
        boxCollider.size = newSize;
    }
}
