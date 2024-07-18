using UnityEditorInternal;
using UnityEngine;


public enum WallLocation
{
    TOP, BOTTOM, LEFT, RIGHT
}

public enum WallType
{
    CAM, NO_CAM
}

public class Wall : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Wall{0}-{1}";

    public void Render(RoomSize roomSize, WallLocation location, WallType type)
    {
        Load(location, type);
        Positionate(roomSize, location);
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load(WallLocation location, WallType type)
    {
        var model_name = "wall";
        if (type == WallType.CAM)
            model_name += "_cam";
        if (type == WallType.NO_CAM)
            model_name += "_no_cam";
        mesh = Instantiate(Resources.Load<GameObject>("Models/" + model_name));
        mesh.name = string.Format(meshName, location.ToString(), type.ToString());
    }

    private void Positionate(RoomSize roomSize, WallLocation location)
    {
        var wallZ = roomSize.wallZ;
        var wallX = roomSize.wallX;
        var center = roomSize.center;

        var renderer = mesh.GetComponent<Renderer>();
        var wall_yDim = renderer.bounds.extents.y;

        switch (location)
        {
            case WallLocation.TOP:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(0, wall_yDim, wallZ);
                break;
            case WallLocation.LEFT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(-wallX, wall_yDim, 0);
                break;
            case WallLocation.RIGHT:
                mesh.transform.rotation = Quaternion.Euler(0, 180, 0);
                mesh.transform.position = center + new Vector3(wallX, wall_yDim, 0);
                break;
            case WallLocation.BOTTOM:
                mesh.transform.position = center + new Vector3(0, wall_yDim, -wallZ);
                break;
        }
    }
}
