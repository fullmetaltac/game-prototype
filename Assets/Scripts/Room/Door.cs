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

        var renderer = mesh.GetComponent<Renderer>();
        var door_yDim = renderer.bounds.extents.y;

        switch (location)
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
