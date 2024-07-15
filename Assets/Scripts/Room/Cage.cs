using UnityEngine;

public class Cage : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Cage{0}";

    public void Render(RoomSize roomSize, WallLocation cageLocation)
    {
        Load(cageLocation);
        Positionate(roomSize, cageLocation);
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

    private void Positionate(RoomSize roomSize, WallLocation doorType)
    {
        var cageZ = roomSize.cageZ;
        var cageX = roomSize.cageX;
        var center = roomSize.center;

        var renderer = mesh.GetComponent<Renderer>();
        var cage_yDim = renderer.bounds.extents.y;

        switch (doorType)
        {
            case WallLocation.TOP:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(0, cage_yDim, cageZ);
                break;
            case WallLocation.LEFT:
                mesh.transform.position = center + new Vector3(-cageX, cage_yDim, 0);
                break;
            case WallLocation.RIGHT:
                mesh.transform.position = center + new Vector3(cageX, cage_yDim, 0);
                break;
            case WallLocation.BOTTOM:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(0, cage_yDim, -cageZ);
                break;
        }
    }
}
