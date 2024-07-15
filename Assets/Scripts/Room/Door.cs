using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Door{0}";

    public void Render(RoomSize roomSize, WallLocation doorType)
    {
        Load(doorType);
        Positionate(roomSize, doorType);
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

    private void Positionate(RoomSize roomSize, WallLocation doorType)
    {
        var wallZ = roomSize.wallZ;
        var wallX = roomSize.wallX;
        var center = roomSize.center;

        var renderer = mesh.GetComponent<Renderer>();
        var door_yDim = renderer.bounds.extents.y;

        switch (doorType)
        {
            case WallLocation.TOP:
                mesh.transform.position = center + new Vector3(0, door_yDim, wallZ);
                break;
            case WallLocation.LEFT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(-wallX, door_yDim, 0);
                break;
            case WallLocation.RIGHT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(wallX, door_yDim, 0);
                break;
            case WallLocation.BOTTOM:
                mesh.transform.position = center + new Vector3(0, door_yDim, -wallZ);
                break;
        }
    }
}
