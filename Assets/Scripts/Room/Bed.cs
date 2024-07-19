using UnityEngine;

public class Bed : MonoBehaviour
{
    GameObject mesh;

    public void Render(RoomSize roomSize)
    {
        Load();
        Positionate(roomSize);
        ApplyControls();
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load()
    {
        mesh = Instantiate(Resources.Load<GameObject>("Models/bed"));
    }

    private void Positionate(RoomSize roomSize)
    {
        var renderer = mesh.GetComponent<Renderer>();
        mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
        mesh.transform.position = roomSize.center;

    }

    private void ApplyControls()
    {
        mesh.AddComponent<RotateObject>();
    }

}
