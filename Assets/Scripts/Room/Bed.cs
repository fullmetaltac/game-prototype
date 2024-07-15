using UnityEngine;

public class Bed : MonoBehaviour
{
    GameObject mesh;

    public void Render(RoomSize roomSize)
    {
        Load();
        Positionate(roomSize);
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
        var yDim = renderer.bounds.extents.y;

        mesh.transform.rotation = Quaternion.Euler(0, 90, 0);
        mesh.transform.position = roomSize.center + new Vector3(0, yDim, 0);

    }

}
