using UnityEngine;
using Random = System.Random;
using System.Collections.Generic;

public class Floor : MonoBehaviour
{
    GameObject mesh;
    string meshName = "Floor";

    public void Render()
    {
        Load();
        AssignMaterials();
        Positionate();
        AssignButton();
    }

    public void DeRender()
    {
        Destroy(mesh);
    }

    private void Load()
    {
        mesh = Instantiate(Resources.Load<GameObject>("Models/floor"));
        mesh.name = meshName;
    }

    private void Positionate()
    {
        mesh.transform.position = RoomSize.center;
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

    private void AssignButton()
    {
        List<GameObject> tiles = new();
        foreach (Transform child in mesh.transform)
        {
            if (child.gameObject.name.StartsWith("tile_button"))
                tiles.Add(child.gameObject);
        }

        int rndIdx = new Random().Next(0, tiles.Count);
        var tile = tiles[rndIdx];
        
        tile.AddComponent<BoxCollider>();
        tile.AddComponent<ControllerTile>();
        
        var boxCollider = tile.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y * 50, boxCollider.size.z);
        boxCollider.isTrigger = true;
        
        // DEBUG
        var renderer = tile.GetComponent<Renderer>();
        renderer.material = Resources.Load<Material>("Materials/mat_floor_button");
        
    }

}
