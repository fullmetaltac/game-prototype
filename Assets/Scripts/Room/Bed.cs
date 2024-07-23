using UnityEngine;

public class Bed : MonoBehaviour
{
    GameObject mesh;

    public void Render()
    {
        Load();
        Positionate();
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

    private void Positionate()
    {
        var renderer = mesh.GetComponent<Renderer>();
        mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
        mesh.transform.position = RoomSize.center;
    }

    private void ApplyControls()
    {
        mesh.AddComponent<RotateObject>();
    }

}
