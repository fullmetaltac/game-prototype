using UnityEngine;

public class Cage : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Cage{0}";

    public void Render(RoomSize roomSize, WallLocation location)
    {
        Load(location);
        Positionate(roomSize, location);
        ApplyControls();
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load(WallLocation doorType)
    {
        mesh = Instantiate(Resources.Load<GameObject>("Models/cage"));
        mesh.name = string.Format(meshName, doorType.ToString());
    }

    private void Positionate(RoomSize roomSize, WallLocation location)
    {
        var cageZ = roomSize.cageZ;
        var cageX = roomSize.cageX;
        var center = RoomSize.center;

        var renderer = mesh.GetComponent<Renderer>();
        var cage_yDim = renderer.bounds.extents.y;

        switch (location)
        {
            case WallLocation.TOP:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(0, -cage_yDim, cageZ);
                break;
            case WallLocation.LEFT:
                mesh.transform.position = center + new Vector3(-cageX, -cage_yDim, 0);
                break;
            case WallLocation.RIGHT:
                mesh.transform.position = center + new Vector3(cageX, -cage_yDim, 0);
                break;
            case WallLocation.BOTTOM:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(0, -cage_yDim, -cageZ);
                break;
        }
    }

    private void ApplyControls()
    {
        mesh.AddComponent<ControllerCage>();
    }
}
