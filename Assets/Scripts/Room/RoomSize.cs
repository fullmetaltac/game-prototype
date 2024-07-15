using System;
using UnityEngine;

public class RoomSize : MonoBehaviour
{

    public Vector3 center;
    public float xDim, zDim;
    public float wallX, wallZ;

    private GameObject roomMesh, wallMesh;

    public void Render()
    {
        wallMesh = Instantiate(Resources.Load<GameObject>("Models/wall_size"));
        roomMesh = Instantiate(Resources.Load<GameObject>("Models/floor_size"));
    }

    public void DeRender()
    {
        Destroy(roomMesh); 
        Destroy(wallMesh); 
    }

    public void Calculate(Tuple<int, int> Index)
    {
        Render();
        CalculateWidthAndLength();
        CalculateCenter(Index);
        DeRender();
    }

    public void CalculateWidthAndLength()
    {
        var renderer = roomMesh.GetComponent<Renderer>();
        xDim = renderer.bounds.extents.x;
        zDim = renderer.bounds.extents.z;

        renderer = wallMesh.GetComponent<Renderer>();
        wallX = renderer.bounds.extents.x;
        wallZ = renderer.bounds.extents.z;


    }
    public void CalculateCenter(Tuple<int, int> Index)
    {
        int x = Index.Item1;
        int z = Index.Item2;

        float center_x = x * xDim * 2 - MapManager.center.Item1 * xDim * 2;
        float center_z = z * zDim * 2 - MapManager.center.Item2 * zDim * 2;

        center = new Vector3(center_x, 0, center_z);
    }
}