using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Door{0}";

    public void Render(RoomSize roomSize, WallLocation location)
    {
        Load(location);
        Positionate(roomSize, location);
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load(WallLocation doorType)
    {
        mesh = Instantiate(Resources.Load<GameObject>("Models/door"));
        mesh.name = string.Format(meshName, doorType.ToString()); 
    }

    private void Positionate(RoomSize roomSize, WallLocation location)
    {
        var wallZ = roomSize.wallZ;
        var wallX = roomSize.wallX;
        var center = RoomSize.center;

        var doorLocation = mesh.GetComponent<ControllerDoor>().doorLocation;
        var renderer = mesh.GetComponent<Renderer>();
        var door_yDim = renderer.bounds.extents.y;

        switch (location)
        {
            case WallLocation.TOP:
                mesh.transform.position = center + new Vector3(0, door_yDim, wallZ);
                mesh.GetComponent<ControllerDoor>().doorLocation = WallLocation.TOP;
                break;
            case WallLocation.LEFT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(-wallX, door_yDim, 0);
                mesh.GetComponent<ControllerDoor>().doorLocation = WallLocation.LEFT;
                break;
            case WallLocation.RIGHT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(wallX, door_yDim, 0);
                mesh.GetComponent<ControllerDoor>().doorLocation = WallLocation.RIGHT;
                break;
            case WallLocation.BOTTOM:
                mesh.transform.position = center + new Vector3(0, door_yDim, -wallZ);
                mesh.GetComponent<ControllerDoor>().doorLocation = WallLocation.BOTTOM;
                break;
        }
    }

    public void ToggleCollider(bool flag)
    {
        mesh.GetComponents<BoxCollider>().First(c => !c.isTrigger).enabled = flag;
    }
}
