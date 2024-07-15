using UnityEngine;


public enum WallType
{
    TOP, BOTTOM, LEFT, RIGHT
}

public class Wall : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Door{0}";

    public void Render(RoomSize roomSize, DoorType doorType)
    {
        Load(doorType);
        Positionate(roomSize, doorType);
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load(DoorType doorType)
    {
        mesh = Instantiate(Resources.Load<GameObject>("Models/door"));
        mesh.name = string.Format(meshName, doorType.ToString());
    }

    private void Positionate(RoomSize roomSize, DoorType doorType)
    {
        var wallZ = roomSize.wallZ;
        var wallX = roomSize.wallX;
        var center = roomSize.center;

        var renderer = mesh.GetComponent<Renderer>();
        var door_yDim = renderer.bounds.extents.y;

        switch (doorType)
        {
            case DoorType.TOP:
                mesh.transform.position = center + new Vector3(0, door_yDim, wallZ);
                break;
            case DoorType.LEFT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(-wallX, door_yDim, 0);
                break;
            case DoorType.RIGHT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(wallX, door_yDim, 0);
                break;
            case DoorType.BOTTOM:
                mesh.transform.position = center + new Vector3(0, door_yDim, -wallZ);
                break;
        }
    }
}
