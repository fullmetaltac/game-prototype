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

    public void Render(RoomSize roomSize, WallLocation wallLocation, WallType wallType)
    {
        Load(wallLocation, wallType);
        Positionate(roomSize, wallLocation);
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load(WallLocation wallLocation, WallType wallType)
    {
        var model_name = "wall";
        if (wallType == WallType.CAM)
            model_name += "_cam";
        if (wallType == WallType.NO_CAM)
            model_name += "_no_cam";
        mesh = Instantiate(Resources.Load<GameObject>("Models/" + model_name));
        mesh.name = string.Format(meshName, wallLocation.ToString(), wallType.ToString());
    }

    private void Positionate(RoomSize roomSize, WallLocation wallLocation)
    {
        var wallZ = roomSize.wallZ;
        var wallX = roomSize.wallX;
        var center = roomSize.center;

        var renderer = mesh.GetComponent<Renderer>();
        var wall_yDim = renderer.bounds.extents.y;

        switch (wallLocation)
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
