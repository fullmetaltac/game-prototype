using UnityEngine;
using Random = System.Random;

public class Floor : MonoBehaviour
{
    GameObject mesh;

    public void Render(RoomSize roomSize)
    {
        Load();
        AssignMaterials();
        Positionate(roomSize);
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load()
    {
        mesh = Instantiate(Resources.Load<GameObject>("Models/floor"));
    }

    private void Positionate(RoomSize roomSize)
    {
        mesh.transform.position = roomSize.center;
    }

    private void AssignMaterials()
    {
        foreach (Transform child in mesh.transform)
        {
            var renderer = child.gameObject.GetComponent<Renderer>();
            if (new Random().Next(1, 100) > 50)
                renderer.material = Resources.Load<Material>("Materials/mat_floor_dark");
            else
                renderer.material = Resources.Load<Material>("Materials/mat_floor_bright");
        }
    }

}
