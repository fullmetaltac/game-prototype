using System;
using UnityEngine;


public enum WallLocation
{
    BOTTOM, LEFT, TOP, RIGHT
}

public enum WallType
{
    CAM, NO_CAM
}

public class Wall : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Wall{0}";

    public void Render(RoomSize roomSize, WallLocation location)
    {
        Load(location);
        Positionate(roomSize, location);
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load(WallLocation location)
    {
        var wallLen = Enum.GetValues(typeof(WallLocation)).Length;
        var wallToHide = (int)ManagerGame.instance.wallToHide % wallLen;
        var nextWall = (wallToHide + 1) % wallLen;

        var model_name = "wall";
        if ((int)location == wallToHide || (int)location == nextWall)
            model_name += "_cam";
        else
            model_name += "_no_cam";
        mesh = Instantiate(Resources.Load<GameObject>("Models/" + model_name));
        mesh.name = string.Format(meshName, location.ToString());
    }

    private void Positionate(RoomSize roomSize, WallLocation location)
    {
        var wallZ = roomSize.wallZ;
        var wallX = roomSize.wallX;
        var center = RoomSize.center;

        var renderer = mesh.GetComponent<Renderer>();
        var wall_yDim = renderer.bounds.extents.y;

        switch (location)
        {
            case WallLocation.TOP:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(0, wall_yDim, wallZ);
                break;
            case WallLocation.LEFT:
                mesh.transform.rotation = Quaternion.Euler(0, 0, 0);
                mesh.transform.position = center + new Vector3(-wallX, wall_yDim, 0);
                break;
            case WallLocation.RIGHT:
                mesh.transform.rotation = Quaternion.Euler(0, 180, 0);
                mesh.transform.position = center + new Vector3(wallX, wall_yDim, 0);
                break;
            case WallLocation.BOTTOM:
                mesh.transform.rotation = Quaternion.Euler(0, 270, 0);
                mesh.transform.position = center + new Vector3(0, wall_yDim, -wallZ);
                break;
        }
    }
}
