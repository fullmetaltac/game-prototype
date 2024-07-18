using UnityEngine;
using Random = System.Random;

public enum FurnitureLocation
{
    TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT
}

public class Furniture : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Furniture{0}";

    public void Render(RoomSize roomSize, FurnitureLocation location)
    {
        Load(location);
        Positionate(roomSize, location);
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load(FurnitureLocation location)
    {
        if (new Random().Next(1, 100) > 50)
            mesh = Instantiate(Resources.Load<GameObject>("Models/clock"));
        else if (new Random().Next(1, 100) > 50)
            mesh = Instantiate(Resources.Load<GameObject>("Models/closet"));
        else if (new Random().Next(1, 100) > 50)
            mesh = Instantiate(Resources.Load<GameObject>("Models/fireplace"));
        else
            mesh = Instantiate(Resources.Load<GameObject>("Models/desktop"));
        mesh.name = string.Format(meshName, location.ToString());
    }

    private void Positionate(RoomSize roomSize, FurnitureLocation location)
    {
        var cageZ = roomSize.cageZ;
        var cageX = roomSize.cageX;
        var center = roomSize.center;

        var renderer = mesh.GetComponent<Renderer>();

        switch (location)
        {
            case FurnitureLocation.TOP_LEFT:
                mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
                mesh.transform.position = center + new Vector3(-cageX, 0, cageZ);
                break;
            case FurnitureLocation.TOP_RIGHT:
                mesh.transform.rotation = Quaternion.Euler(0, 180, 0);
                mesh.transform.position = center + new Vector3(cageX, 0, cageZ);
                break;
            case FurnitureLocation.BOTTOM_LEFT:
                mesh.transform.rotation = Quaternion.Euler(0, 0, 0);
                mesh.transform.position = center + new Vector3(-cageX, 0, -cageZ);
                break;
            case FurnitureLocation.BOTTOM_RIGHT:
                mesh.transform.rotation = Quaternion.Euler(0, -90, 0);
                mesh.transform.position = center + new Vector3(cageX, 0, -cageZ);
                break;
        }
    }
}
